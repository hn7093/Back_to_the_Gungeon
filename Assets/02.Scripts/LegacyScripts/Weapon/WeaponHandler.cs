using System.Collections;
using UnityEngine;


// 무기 공용 클래스
public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Info")]
    public LayerMask target;
    private float delay = 1f;
    public float Delay { get => delay; set => delay = value; }

    private float weaponSize = 1f;
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }

    private float power = 1f;
    public float Power { get => power; set => power = value; }

    private float speed = 1f;
    public float Speed { get => speed; set => speed = value; }

    private float attackRange = 10f;
    public float AttackRange { get => attackRange; set => attackRange = value; }

    [SerializeField] public WeaponSO weaponData;

    [Header("Knock Back Info")]
    private bool isOnKnockback = false;
    public bool IsOnKnockback { get => isOnKnockback; set => isOnKnockback = value; }

    private float knockbackPower = 0.1f;
    public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }

    private float knockbackTime = 0.5f;
    public float KnockbackTime { get => knockbackTime; set => knockbackTime = value; }

    private static readonly int IsAttack = Animator.StringToHash("OnAttack");

    private AudioClip attackSoundClip;

    // componets
    protected BaseController Controller { get; private set; }

    private Animator animator;
    private SpriteRenderer weaponRenderer;
    private ParticleSystem particleSystem;

    protected virtual void Awake()
    {
        // 캐릭터 하위에 들어가므로 부모에서 탐색
        Controller = GetComponentInParent<BaseController>();
        animator = GetComponentInChildren<Animator>();
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        animator.speed = 1.0f / delay;
        transform.localScale = Vector3.one * weaponSize;
    }

    protected virtual void Start()
    {

    }

    public virtual IEnumerator Attack()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
        AttackAnimation();
        if (attackSoundClip != null)
        {
            SoundManager.PlayClip(attackSoundClip);
        }
        yield return null;
    }

    public void AttackAnimation()
    {
        animator.SetTrigger(IsAttack);
    }

    public virtual void Rotate(bool isLeft)
    {
        weaponRenderer.flipY = isLeft;
    }


    // 데이터 복사 후 세팅
    public virtual void Setup(WeaponSO weaponData)
    {
        Delay = weaponData.delay;
        WeaponSize = weaponData.weaponSize;
        Power = weaponData.power;
        Speed = weaponData.speed;
        AttackRange = weaponData.attackRange;
        IsOnKnockback = weaponData.isOnKnockback;
        KnockbackPower = weaponData.knockbackPower;
        KnockbackTime = weaponData.knockbackTime;
        attackSoundClip = weaponData.attackSoundClip;
    }
}
