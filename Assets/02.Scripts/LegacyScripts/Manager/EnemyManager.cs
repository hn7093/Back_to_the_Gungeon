using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance { get { return instance; } }

    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] List<Rect> spawnAreas;
    [SerializeField] private Color gizmoColor = new Color(1, 1, 1, 0.3f);
    public List<BaseController> activeEnemies = new List<BaseController>(10);
    private bool enemySpawnComplite; // 생성 코루틴 진행 여부

    [SerializeField] private float timeBetweenSpawns = 0.2f; // 스폰당 대기 시간
    [SerializeField] private float timeBetweenWaves = 1f; // 웨이브 당 대기 시간

    private Coroutine waveRoutine; // 웨이브 소환 코루틴

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        // test
        SpawnRandomEnemy();
        // SpawnRandomEnemy();
        // SpawnRandomEnemy();
        // SpawnRandomEnemy();
        // SpawnRandomEnemy();
        // SpawnRandomEnemy();
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

        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        Vector2 randomPosition = new Vector2(
            Random.Range(randomArea.xMin, randomArea.xMax),
            Random.Range(randomArea.yMin, randomArea.yMax)
        );

        // 생성
        GameObject spawnEntity = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y), Quaternion.identity);
        EnemyController enemyController = spawnEntity.GetComponent<EnemyController>();
        // 목표를 플레이어로
        //enemyController.ConnectUIManager(GameManager.Instance.player.transform);
        enemyController.Init(FindObjectOfType<PlayerController>().transform);
        activeEnemies.Add(enemyController);

        // 플레이어의 타겟 갱신
        FindObjectOfType<PlayerController>().SetEnemyList(activeEnemies);
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
