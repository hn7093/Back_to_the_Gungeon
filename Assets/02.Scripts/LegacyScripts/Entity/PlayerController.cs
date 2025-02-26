using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public enum controlType
{
    keyboard = 0,
    mouse
}

public class PlayerController : BaseController
{
    [SerializeField] List<GameObject> playerSkinPrefabs = new List<GameObject>();
    [SerializeField] List<GameObject> weaponSkinPrefabs = new List<GameObject>();
    [SerializeField] protected GameObject currentSkin;
    [SerializeField] protected GameObject currentWeapon;
    private List<BaseController> enemyList; // 적 리스트

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isDragging = false;
    private float dragThreshold = 1f;
    private controlType currentControllType;
    public static readonly string controlTypeKey = "controlTypeKey";
    public static readonly string skinIndexKey = "skinIndexKey";
    public static readonly string weaponIndexKey = "weaponIndexKey";
    private bool isAnyEnemy = false;
    private int currentSkinIndex;
    private int currentWeaponIndex;

    private void Start()
    {
        currentControllType = (controlType)PlayerPrefs.GetInt(controlTypeKey, 0);
        currentSkinIndex = PlayerPrefs.GetInt(skinIndexKey, 0);
        ChangePlayerSkin(currentSkinIndex);
        currentWeaponIndex = PlayerPrefs.GetInt(weaponIndexKey, 0);
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

    protected override void HandleAction()
    {
        if (currentControllType == 0)
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
            // 활성화 된 오브젝트일 때만
            if (!enemy.gameObject.activeSelf) continue;

            // 비교용으로 차이의 제곱을 사용 - 제곱근 생략
            float dis = (enemy.transform.position - weaponPivot.position).sqrMagnitude;
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(weaponPivot.position, directionToEnemy, Mathf.Sqrt(dis), LayerMask.GetMask("Wall", "innerWall"));

            if (hit.collider == null)//벽이 없으면 bestEnemy로 지정
            {
                if (dis < closestDistance)
                {
                    closestDistance = dis;
                    bestEnemy = enemy.transform;
                }
            }
            else//벽이 있으면 blockedEnemy로 지정
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

        // 공격 가능 여부 확인
        if (isAttacking && timeSinceLastAttack > _weaponHandler.Delay && isAnyEnemy)
        {
            Debug.Log("enter");
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    public override void Death()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        animationHandler.Death();

        // 모든 본인과 자식 컴포넌트 비활성화
        StartCoroutine(DisableComponentsAfterDelay(2f));

        // 게임오버 화면 호출
    }

    private IEnumerator DisableComponentsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 모든 본인과 자식 컴포넌트 비활성화
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }
    }

    public void NextSkin()
    {
        int newSkinIndex = (currentSkinIndex + 1) % playerSkinPrefabs.Count;
        ChangePlayerSkin(newSkinIndex);
    }

    public void PrevSkin()
    {
        int newSkinIndex = (currentSkinIndex - 1 + playerSkinPrefabs.Count) % playerSkinPrefabs.Count;
        ChangePlayerSkin(newSkinIndex);
    }

    public void ChangePlayerSkin(int skinIndex)
    {
        if (playerSkinPrefabs == null || playerSkinPrefabs.Count == 0) return;

        skinIndex = Mathf.Clamp(skinIndex, 0, playerSkinPrefabs.Count - 1);
        currentSkinIndex = skinIndex;

        // 
        PlayerPrefs.SetInt(skinIndexKey, currentSkinIndex);
        PlayerPrefs.Save();

        if (currentSkin != null)
        {
            Destroy(currentSkin);
        }


        currentSkin = Instantiate(playerSkinPrefabs[skinIndex], transform);
        currentSkin.transform.localPosition = Vector3.zero;

        characterRenderer = currentSkin.GetComponent<SpriteRenderer>();
        animationHandler = currentSkin.GetComponentInChildren<AnimationHandler>();

        if (animationHandler != null)
        {
            animationHandler.Init();  //  새로운 스킨의 Animator 재할당
            Debug.Log(" AnimationHandler initialized after skin change.");
        }
        else
        {
            Debug.LogError(" AnimationHandler is NULL after skin change!");
        }
    }

    public void NextWeaponSkin()
    {
        int newSkinIndex = (currentWeaponIndex + 1) % weaponSkinPrefabs.Count;
        ChangeWeapon(newSkinIndex);
    }

    public void PrevWeaponSkin()
    {
        int newSkinIndex = (currentWeaponIndex - 1 + weaponSkinPrefabs.Count) % weaponSkinPrefabs.Count;
        ChangeWeapon(newSkinIndex);
    }

    public void ChangeWeapon(int weaponSkinIndex)
    {
        if (weaponSkinPrefabs == null || weaponSkinPrefabs.Count == 0) return;

        weaponSkinIndex = Mathf.Clamp(weaponSkinIndex, 0, weaponSkinPrefabs.Count - 1);
        currentSkinIndex = weaponSkinIndex;

        // 
        PlayerPrefs.SetInt(weaponIndexKey, currentSkinIndex);
        PlayerPrefs.Save();

        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        if(_weaponHandler != null)
        {
            _weaponHandler = null;
        }


        currentWeapon = Instantiate(weaponSkinPrefabs[weaponSkinIndex], weaponPivot);
        currentWeapon.transform.localPosition = Vector3.zero;

        _weaponHandler = currentWeapon.GetComponent<WeaponHandler>();
        FindWeaponRenderer();
        
    }
}
