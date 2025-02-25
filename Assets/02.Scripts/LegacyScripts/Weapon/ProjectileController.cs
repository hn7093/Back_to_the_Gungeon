using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    public bool fxOnDestroy;
    [SerializeField] private bool canBounce = true; // 벽 반사 여부
    private int bounceCtn = 2; // 최대 팅김 횟수
    private RangeWeaponHandler rangeWeaponHandler;
    private float currentDuration;
    private Vector2 direction;
    private bool isReady;
    private Transform pivot;


    private float scale = 1.0f; // 데미지 배율

    // components
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        // 첫번째 자식
        pivot = transform.GetChild(0);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady) return;

        currentDuration += Time.deltaTime;
        if (currentDuration > rangeWeaponHandler.Duration)
        {
            // 지속 시간 종료
            DestroyProjectile(transform.position, false);
        }
    }
    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler)
    {
        rangeWeaponHandler = weaponHandler;

        this.direction = direction;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        _spriteRenderer.color = weaponHandler.ProjectileColor;

        // 회전
        transform.right = this.direction;

        if (direction.x < 0)
        {
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        }
        else
        {
            pivot.localRotation = Quaternion.Euler(0, 0, 0);
        }
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.velocity = direction.normalized * rangeWeaponHandler.Speed;
        Debug.Log(_rigidbody.velocity.magnitude);
        isReady = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // levelCollisionLayer는 여러 레이어를 포함 할 수 있으므로, collision의 레이어만큼 1에 시프트 후 OR 연산을 한다.
        // levelCollisionLayer가 10100라면 시프트 후 결과 10000, 00100 모두 OR 후에는 levelCollisionLayer.value와 같다.
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            Debug.Log("벽");
            if (canBounce && bounceCtn > 0)
            {
                // 배율 감소
                scale *= 0.5f;

                // 팅김 횟수 감소
                bounceCtn--;

                // 충돌한 벽의 법선 벡터 가져오기
                Vector2 close = collision.ClosestPoint(transform.position);
                Vector2 dir = transform.position;
                Vector2 normal = (dir - close).normalized;
                // 현재 속도를 법선 방향으로 반사
                _rigidbody.velocity = Vector2.Reflect(_rigidbody.velocity, normal);
            }
            else
            {
                // 벽면등 배경요소에 닿았다면 삭제
                DestroyProjectile(collision.ClosestPoint(transform.position) - direction * 0.2f, fxOnDestroy);
            }

        }
        // target은 rangeWeaponHandler의 레이어
        if (rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer)))
        {
            // 목표 오브젝트
            // 데미지 & 넉백
            ResourceController resourceController = collision.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                // 배율 적용 데미지
                resourceController.ChangeHealth(-rangeWeaponHandler.Power * scale);
                // 넉백
                if (rangeWeaponHandler.IsOnKnockback)
                {
                    BaseController controller = collision.GetComponent<BaseController>();
                    if (controller != null)
                    {
                        controller.ApplyKnockback(transform, rangeWeaponHandler.KnockbackPower, rangeWeaponHandler.KnockbackTime);
                    }
                }
            }
            // 삭제
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    // 탄 삭제
    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        // 파티클 재생
        if (createFx)
        {
            ProjectileManager.Instance.CreateImpactParticlesAtPostion(position, rangeWeaponHandler);
        }
        // 삭제
        Destroy(gameObject);
    }
}
