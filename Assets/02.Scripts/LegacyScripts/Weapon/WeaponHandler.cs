using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 무기 공용 클래스
public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Info")]
    public LayerMask target;
    [SerializeField] private float delay = 1f;
    public float Delay { get => delay; set => delay = value; }

    [SerializeField] private float weaponSize = 1f;
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }

    [SerializeField] private float power = 1f;
    public float Power { get => power; set => power = value; }

    [SerializeField] private float speed = 1f;
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] private float attackRange = 10f;
    public float AttackRange { get => attackRange; set => attackRange = value; }


    [Header("Knock Back Info")]
    [SerializeField] private bool isOnKnockback = false;
    public bool IsOnKnockback { get => isOnKnockback; set => isOnKnockback = value; }

    [SerializeField] private float knockbackPower = 0.1f;
    public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }

    [SerializeField] private float knockbackTime = 0.5f;
    public float KnockbackTime { get => knockbackTime; set => knockbackTime = value; }

    private static readonly int IsAttack = Animator.StringToHash("OnAttack");

    public AudioClip attackSoundClip;

    // componets
    protected BaseController Controller { get; private set; }

    private Animator animator;
    private SpriteRenderer weaponRenderer;

    protected virtual void Awake()
    {
        // 캐릭터 하위에 들어가므로 부모에서 탐색
        Controller = GetComponentInParent<BaseController>();
        animator = GetComponentInChildren<Animator>();
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();

        animator.speed = 1.0f / delay;
        transform.localScale = Vector3.one * weaponSize;
    }

    protected virtual void Start()
    {
        
    }

    public virtual void Attack()
    {
        AttackAnimation();
        if(attackSoundClip != null)
        {
            SoundManager.PlayClip(attackSoundClip);
        }
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
