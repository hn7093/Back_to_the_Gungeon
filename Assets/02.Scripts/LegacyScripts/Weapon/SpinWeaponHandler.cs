using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 회전체 무기
public class SpinWeaponHandler : WeaponHandler
{
    [SerializeField] GameObject spinWeaponPrefab;
    private int objectIndex; // 인덱스
    public int ObjectIndex { get { return objectIndex; } set => objectIndex = value; }
    private int spintCount = 2; // 회전체 갯수
    private Transform piviot; // 회전 공격체를 자식으로 가지는 오브젝트
    private List<SpinWeaponController> spinWeaponControllers = new(2); // 회전 공격체
    private bool ready = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        piviot = transform.GetChild(0); // 첫번째 자식
        ready = true;
    }

    void Update()
    {
        if (!ready) return;

        // speed = 회전 속도, attackRange = 반지름, delay는 무시
        float rotationSpeed = Speed * 360f; // 초당 회전 각도
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AddWeapon();
        }
    }
    public override IEnumerator Attack()
    {
        yield return null;
    }
    public override void Setup(WeaponSO weaponData)
    {
        base.Setup(weaponData);
        spintCount = weaponData.numberOfObejects;
        for (int i = 0; i < spintCount; i++)
        {
            AddWeapon();
        }
    }

    // 무기 수 증가
    public void AddWeapon()
    {
        var obj = Instantiate(spinWeaponPrefab, piviot).GetComponent<SpinWeaponController>();
        obj.transform.parent = piviot;
        obj.Init(this);
        spinWeaponControllers.Add(obj);
        AlignWeapon();
    }
    private void AlignWeapon()
    {
        for (int i = 0; i < spinWeaponControllers.Count; i++)
        {
            float angle = i * (360f / spinWeaponControllers.Count); // 각도를 일정하게 배분
            float radian = angle * Mathf.Deg2Rad; // 라디안 변환

            // 원의 좌표 계산 (X, Y만 사용하면 2D에서도 사용 가능)
            var position = new Vector3(
                Mathf.Cos(radian) * 2,
                Mathf.Sin(radian) * 2,
                0 // 2D라면 Z는 0
            );
            spinWeaponControllers[i].transform.localPosition = position;
            spinWeaponControllers[i].transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
