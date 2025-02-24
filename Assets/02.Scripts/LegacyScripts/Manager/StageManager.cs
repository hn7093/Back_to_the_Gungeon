using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public List<GameObject> stage = new List<GameObject>();
    public GameObject currentStage;
    
    public NextStage nextStage;

    private int stageCount = 0;
    private int randomMapIndex;
    private int lastMapIndex = -1; // 첫맵부터 랜덤맵인덱스랑 겹쳐서 같은 맵 판정을 받으면 안되기 때문에 마이너스로 초기화   
    void Start()
    {
        if (nextStage == null)
        {
            Debug.Log("Next Stage Not Found");
            nextStage = FindObjectOfType<NextStage>();
            if(nextStage == null)
                Debug.LogError("Next Stage Not Found");
        }
        GenerateNewStage();
    }

    // Update is called once per frame
    void Update()
    {
        StageClear();
    }

    void StageClear()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) // 현재 씬에 적이 없을 떄
        {
            Debug.Log("StageClear!");
            nextStage.DoorOpen();
        }
        else
        {
            nextStage.DoorClose();
        }
    }
    
    public void GenerateNewStage()
    {
        if (currentStage != null)
        {
            Destroy(currentStage);
        }

        do // 연속으로 같은 맵이 나오지 않도록 방지한다
        {
            randomMapIndex = Random.Range(0, stage.Count);
        } while (randomMapIndex == lastMapIndex);
        
        lastMapIndex = randomMapIndex;

        if (stageCount == 0)
        {
            currentStage = Instantiate(stage[0]);
        }
        else if (stageCount % 4 == 0)
        {
            currentStage = Instantiate(stage[randomMapIndex]); // 보상 맵 출현
        }
        else if (stageCount == 10)
        {
            currentStage = Instantiate(stage[randomMapIndex]); // 보스 스테이지 지정
        }
        else
        {
         currentStage = Instantiate(stage[randomMapIndex]); // 일반맵 생성   
        }
        
        nextStage = currentStage.GetComponentInChildren<NextStage>(); // 새로운 스테이지에서 NextStage 찾기
        if (nextStage == null)
        {
            Debug.LogError("Cannot found Next Stage");
        }
        
        stageCount++;
    }
    
}
