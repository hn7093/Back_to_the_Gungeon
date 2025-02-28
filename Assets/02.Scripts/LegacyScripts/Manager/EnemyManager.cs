using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance { get { return instance; } }

    [SerializeField] private int monsters = 1;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] List<Rect> spawnAreas;
    [SerializeField] bool spawnPair = false; // 여러 영역에서 소환시 골고루 소환 여부
    [SerializeField] private Color gizmoColor = new Color(1, 1, 1, 0.3f);
    public List<BaseController> activeEnemies = new List<BaseController>(10);
    private bool enemySpawnComplite; // 생성 코루틴 진행 여부
    private int spawnIdx = 0; // 스폰 인덱스

    [SerializeField] private float timeBetweenSpawns = 0.2f; // 스폰당 대기 시간
    [SerializeField] private float timeBetweenWaves = 1f; // 웨이브 당 대기 시간

    private Coroutine waveRoutine; // 웨이브 소환 코루틴
    [SerializeField] private PlayerController player;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        // 설정한 수만큼 몬스터 생성
        for (int i = 0; i < monsters; i++)
        {
            SpawnRandomEnemy();
        }


        // SpawnRandomEnemy();
        // SpawnRandomEnemy();
        // SpawnRandomEnemy();
        // SpawnRandomEnemy();
        // SpawnRandomEnemy();
    }
    void Update()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();
        else
            player.SetEnemyList(activeEnemies);
    }

    public void StartWave(int waveCount)
    {
        // 호출 오류 수정
        if (waveCount <= 0)
        {
            GameManager.Instance.EndWave();
            return;
        }

        if (waveRoutine != null)
            StopCoroutine(waveRoutine);
        waveRoutine = StartCoroutine(SpawnWave(waveCount));
    }
    public void StopWave()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        enemySpawnComplite = false;
        yield return new WaitForSeconds(timeBetweenWaves);

        for (int i = 0; i < waveCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnRandomEnemy();
        }
        enemySpawnComplite = true;
    }

    private void SpawnRandomEnemy()
    {
        // 설정 확인
        if (enemyPrefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("Enemy Prefab 또는 Spawn Area가 설정되지 않았습니다.");
        }

        // 랜덤 오브젝트, 램덤 위치
        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        Rect spawnArea;
        if (spawnPair)
        {
            spawnArea = spawnAreas[spawnIdx++];
            spawnIdx = spawnIdx % spawnAreas.Count;
        }
        else
        {
            spawnArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
        }
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnArea.xMin, spawnArea.xMax),
            Random.Range(spawnArea.yMin, spawnArea.yMax)
        );
        // 생성
        GameObject spawnEntity = Instantiate(randomPrefab, new Vector3(spawnPosition.x, spawnPosition.y), Quaternion.identity);
        EnemyController enemyController = spawnEntity.GetComponent<EnemyController>();
        // 목표를 플레이어로
        activeEnemies.Add(enemyController);
        player.SetEnemyList(activeEnemies);
        enemyController.Init(player.transform);
    }

    void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        // 스폰 영역 표시
        Gizmos.color = gizmoColor;
        foreach (var area in spawnAreas)
        {
            Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, area.height);
            Gizmos.DrawCube(center, size);
        }
    }

    // 적 죽음시 처리
    public void RemoveEnemyOnDeath(EnemyController enemy)
    {
        Debug.Log("remove");
        activeEnemies.Remove(enemy);

        // 전부 소환되고 남은 몬스터가 없다면
        if (enemySpawnComplite && activeEnemies.Count == 0)
        {
            FindObjectOfType<PlayerController>().SetEnemyList(null);
            GameManager.Instance.EndWave();
        }
    }
}
