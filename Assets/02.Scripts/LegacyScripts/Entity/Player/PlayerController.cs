using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ControlType
{
    mouse = 0,
    keyboard
}

public class PlayerController : BaseController
{
    [SerializeField] protected GameObject currentSkin;
    [SerializeField] protected GameObject currentWeapon;
    [SerializeField] private List<BaseController> enemyList; // �� ����Ʈ

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isDragging = false;
    private float dragThreshold = 1f;
    private ControlType currentControllType;
    public static readonly string controlTypeKey = "controlTypeKey";
    public static readonly string skinIndexKey = "skinIndexKey";
    public static readonly string weaponIndexKey = "weaponIndexKey";
    private bool isAnyEnemy = false;
    private int currentSkinIndex;
    private int currentWeaponIndex;

    private ResourceController resourceController;
    private WeaponPivotAnimationHandler weaponPivotAnimationHandler;

    private void Start()
    {
        resourceController = GetComponent<ResourceController>();
        weaponPivotAnimationHandler = GetComponentInChildren<WeaponPivotAnimationHandler>();

        currentControllType = (ControlType)PlayerPrefs.GetInt(controlTypeKey, 0);
        currentSkinIndex = SkinManager.Instance.CurrentSkinIndex;
        ChangePlayerSkin(currentSkinIndex);
        currentWeaponIndex = WeaponManager.Instance.CurrentWeaponIndex;
        ChangeWeapon(currentWeaponIndex);
    }

    protected override void Update()
    {
        HandleAction();
        SetCloserTarget();
        SetLookDirection();
        SetIsLeft();
        Rotate(isLeft);
        SetIsAttacking();
        HandleAttackDelay();
    }

    public void NextControlType()
    {
        int currentControllTypeIndex = (int)currentControllType;
        currentControllTypeIndex = (currentControllTypeIndex + 1) % 2;
        currentControllType = (ControlType)currentControllTypeIndex;
        PlayerPrefs.SetInt(controlTypeKey, currentControllTypeIndex);
        PlayerPrefs.Save();
    }

    public void PreviousControlType()
    {
        int currentControllTypeIndex = (int)currentControllType;
        currentControllTypeIndex = (currentControllTypeIndex - 1 + 2) % 2;
        currentControllType = (ControlType)currentControllTypeIndex;
        PlayerPrefs.SetInt(controlTypeKey, currentControllTypeIndex);
        PlayerPrefs.Save();
    }

    protected override void HandleAction()
    {
        if (currentControllType == ControlType.keyboard)
            HandleKeyboardInput();
        else
            HandleMouseInput();
    }

    protected void HandleKeyboardInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

    }

    private void HandleMouseInput()
    {


        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            isDragging = true;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            currentTouchPosition = Input.mousePosition;
            float dragDistance = Vector2.Distance(startTouchPosition, currentTouchPosition);
            if (dragDistance > dragThreshold)
                movementDirection = (currentTouchPosition - startTouchPosition).normalized;
            else
                movementDirection = Vector2.zero;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            movementDirection = Vector2.zero;
        }
    }

    protected override void SetIsAttacking()
    {
        if (_rigidbody.velocity.magnitude == 0)
            isAttacking = true;
        else
            isAttacking = false;
    }

    public void SetEnemyList(List<BaseController> enemies)
    {
        enemyList = enemies;
    }

    public void RemoveEnemyList(BaseController enemy)
    {
        enemyList.Remove(enemy);
    }

    public void SetCloserTarget()
    {
        if (enemyList == null || enemyList.Count == 0) isAnyEnemy = false;

        //Debug.Log("SetCloserTarget : " + enemyList.Count);
        closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        float blockedDistance = Mathf.Infinity;
        Transform bestEnemy = null;
        Transform blockedEnemy = null;

        foreach (var enemy in enemyList)
        {
            // Ȱ��ȭ �� ������Ʈ�� ����
            if (enemy == null || !enemy.gameObject.activeSelf) continue;

            // �񱳿����� ������ ������ ��� - ������ ����
            float dis = (enemy.transform.position - weaponPivot.position).sqrMagnitude;
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(weaponPivot.position, directionToEnemy, Mathf.Sqrt(dis), LayerMask.GetMask("Wall", "innerWall"));

            if (hit.collider == null)//���� ������ bestEnemy�� ����
            {
                if (dis < closestDistance)
                {
                    closestDistance = dis;
                    bestEnemy = enemy.transform;
                }
            }
            else//���� ������ blockedEnemy�� ����
            {
                if (dis < blockedDistance)
                {
                    blockedDistance = dis;
                    blockedEnemy = enemy.transform;
                }
            }
        }

        if (bestEnemy != null)
        {
            closestEnemy = bestEnemy;
            isAnyEnemy = true;
        }
        else if (blockedEnemy != null)
        {
            closestEnemy = blockedEnemy;
            isAnyEnemy = true;
        }
        else
            isAnyEnemy = false;
    }

    protected override void HandleAttackDelay()
    {
        if (_weaponHandler == null)
        {
            Debug.Log("weaponHandler is null");
            return;
        }

        if (timeSinceLastAttack <= _weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        // ���� ���� ���� Ȯ��
        if (isAttacking && timeSinceLastAttack > _weaponHandler.Delay && isAnyEnemy)
        {
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    public override void Damage()
    {
        animationHandler.Damage();
    }

    public override void DisableInvincible()
    {
        animationHandler.EndInvincibility();
    }

    public override void Death()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        animationHandler.Death();
        weaponPivotAnimationHandler.Death();

        // ��� ���ΰ� �ڽ� ������Ʈ ��Ȱ��ȭ
        StartCoroutine(DisableComponentsAfterDelay(2f));

        // ���ӿ��� ȭ�� ȣ��
    }

    private IEnumerator DisableComponentsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        weaponPivot.gameObject.SetActive(false);

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }
    }

    public void NextSkin()
    {
        int newSkinIndex = (currentSkinIndex + 1) % SkinManager.Instance.allSkins.Count;
        ChangePlayerSkin(newSkinIndex);
    }

    public void PrevSkin()
    {
        int newSkinIndex = (currentSkinIndex - 1 + SkinManager.Instance.allSkins.Count) % SkinManager.Instance.allSkins.Count;
        ChangePlayerSkin(newSkinIndex);
    }

    public void ChangePlayerSkin(int skinIndex)
    {

        skinIndex = Mathf.Clamp(skinIndex, 0, SkinManager.Instance.allSkins.Count - 1);
        currentSkinIndex = skinIndex;
        SkinManager.Instance.CurrentSkinIndex = skinIndex;

        if (currentSkin != null)
        {
            Destroy(currentSkin);
        }


        currentSkin = Instantiate(SkinManager.Instance.GetCurrentSkin().skinPrefab, transform);
        currentSkin.transform.localPosition = Vector3.zero;
        Debug.Log($"{SkinManager.Instance.GetCurrentSkin().name} ����");
        StartCoroutine(DelayedSetNewSkin());
    }

    public void SetSkin()
    {
        if (!SkinManager.Instance.IsSkinUnlocked(currentSkinIndex)) return;
        PlayerPrefs.SetInt(skinIndexKey, currentSkinIndex);
        PlayerPrefs.Save();
    }

    public void UnlockSkin()
    {
        SkinManager.Instance.UnlockSkin(currentSkinIndex);
    }

    private IEnumerator DelayedSetNewSkin()
    {
        yield return null; // �� ������ ���
        characterRenderer = currentSkin.GetComponent<SpriteRenderer>();
        animationHandler = currentSkin.GetComponentInChildren<PlayerAnimationHandler>();

        if (animationHandler != null)
        {
            animationHandler.Init();  //  ���ο� ��Ų�� Animator ���Ҵ�
            Debug.Log(" AnimationHandler initialized after skin change.");
        }
        else
        {
            Debug.Log(" AnimationHandler is NULL after skin change!");
        }
    }

    public void NextWeapon()
    {
        int newSkinIndex = (currentWeaponIndex + 1) % WeaponManager.Instance.allWeapons.Count;
        ChangeWeapon(newSkinIndex);
    }

    public void PrevWeapon()
    {
        int newSkinIndex = (currentWeaponIndex - 1 + WeaponManager.Instance.allWeapons.Count) % WeaponManager.Instance.allWeapons.Count;
        ChangeWeapon(newSkinIndex);
    }

    public void ChangeWeapon(int weaponIndex)
    {
        weaponIndex = Mathf.Clamp(weaponIndex, 0, WeaponManager.Instance.allWeapons.Count - 1);
        currentWeaponIndex = weaponIndex;
        WeaponManager.Instance.CurrentWeaponIndex = currentWeaponIndex;
        ClearWeapon();

        currentWeapon = Instantiate(WeaponManager.Instance.GetCurrentWeapon().weaponPrefab, weaponPivot);
        Debug.Log("���� ����: " + weaponIndex);

        StartCoroutine(DelayedSetNewWeapon());
    }

    public void SetWeapon()
    {
        if (!WeaponManager.Instance.IsWeaponUnlocked(currentWeaponIndex)) return;
        PlayerPrefs.SetInt(weaponIndexKey, currentWeaponIndex);
        PlayerPrefs.Save();
    }

    public void UnlockWeapon()
    {
        WeaponManager.Instance.UnlockWeapon(currentWeaponIndex);
    }

    public void ClearWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }

        if (_weaponHandler != null)
            _weaponHandler = null;

        if (weaponRenderer != null)
            weaponRenderer = null;

        if (weaponData != null)
            weaponData = null;
    }

    private IEnumerator DelayedSetNewWeapon()
    {
        yield return null; // �� ������ ���
        FindWeaponRenderer();
        Debug.Log("DelayedSetNewWeapon �����");
        _weaponHandler = currentWeapon.GetComponent<WeaponHandler>();

        if (_weaponHandler != null)
        {
            this.weaponData = _weaponHandler.weaponData; // WeaponSO ��������
            Debug.Log("���� ������ ����: " + this.weaponData.name);
            _weaponHandler.Setup(weaponData);
        }
        else
        {
            Debug.Log("WeaponHandler�� ã�� �� �����ϴ�!");
        }
    }


    // �ɷ�ġ ���� ����
    #region Status Change
    // ü�� ���� - �ۼ�Ʈ
    public void ChangeHealth(float value)
    {
        ResourceController resourceController = GetComponent<ResourceController>();
        if (resourceController != null)
        {
            int healthValue = (int)(value * resourceController.MaxHealth / 100);
            resourceController.ChangeHealth(healthValue);
        }
    }
    // ü�� ���� - ����
    public void ChangeHealth(int value)
    {
        ResourceController resourceController = GetComponent<ResourceController>();
        if (resourceController != null)
        {
            resourceController.ChangeHealth(value);
        }
    }

    // �ִ� ü�� ����, changeHealth�� ���̸� ȸ������ ���� - �ۼ�Ʈ
    public void AddMaxHP(float addHealth, bool changeHealth = false)
    {
        ResourceController resourceController = GetComponent<ResourceController>();
        if (resourceController != null)
        {
            int healthValue = (int)(addHealth * resourceController.MaxHealth / 100);
            resourceController.AddMaxHealth(healthValue);
        }
    }
    // �ִ� ü�� ����, ����
    public void AddMaxHP(int addHealth, bool changeHealth = false)
    {
        ResourceController resourceController = GetComponent<ResourceController>();
        if (resourceController != null)
        {
            resourceController.AddMaxHealth(addHealth, changeHealth);
        }
    }
    // ���ݷ� ���� - �ۼ�Ʈ
    public void AddPower(int percent)
    {
        _weaponHandler.AddPower(percent);
    }
    // ���� �ӵ� ���� - �ۼ�Ʈ
    public void AddAttackSpeed(int percent)
    {
        _weaponHandler.AddAttackSpeed(percent);
    }
    // �̵� �ӵ� ���� - ����
    public void AddSpeed(int value)
    {
        ResourceController resourceController = GetComponent<ResourceController>();
        if (resourceController != null)
        {
            resourceController.AddSpeed(value);
        }
    }


    // �߻� ź�� ����
    public void AddBullet(int value)
    {
        _weaponHandler.AddFrontBullet(value);
    }

    // �Ѿ� �� �ݻ�
    public void SetBounce(bool canBounce)
    {
        _weaponHandler.SetBounce(canBounce);
    }
    // �Ѿ� �� ���
    public void SetThrough(bool canThrough)
    {
        _weaponHandler.SetThrough(canThrough);
    }
    #endregion
}
