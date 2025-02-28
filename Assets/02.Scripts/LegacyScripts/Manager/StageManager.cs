using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Preference;
public class StageManager : MonoBehaviour
{
    public List<GameObject> stage = new List<GameObject>();
    public GameObject currentStage;
    
    public NextStage nextStage;
    public int stageCount;
    private int randomMapIndex;
    private int lastMapIndex = -1; // 첫맵부터 랜덤맵인덱스랑 겹쳐서 같은 맵 판정을 받으면 안되기 때문에 마이너스로 초기화   
    void Start()
    {
        currentStage = null;
        
        stageCount = 0;
        if (nextStage == null)
        {
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
            randomMapIndex = Random.Range(5, stage.Count);
        } while (randomMapIndex == lastMapIndex);
        
        lastMapIndex = randomMapIndex;
        // bgm 설정
        if (stageCount == 10 || stageCount == 20)
        {
            int changeBGM = Random.Range(1, 3); //1,2
            SystemManager.Instance.AudioManager.UpdateBGMSourceClip(changeBGM);
        }
        else
        {
            if( SystemManager.Instance.AudioManager.currentBGMIndex != 0)
                SystemManager.Instance.AudioManager.UpdateBGMSourceClip(0);
        }
        if (stageCount == 0)
        {
            currentStage = Instantiate(stage[4]);
        }
        else if (stageCount % 4 == 0 && stageCount != 20)
        {
            currentStage = Instantiate(stage[0]); // 보상 맵 출현
        }
        else if (stageCount == 10)
        {
            currentStage = Instantiate(stage[1]); // 매직 보스 스테이지 지정
        }
        else if (stageCount == 20)
        {
            currentStage = Instantiate(stage[2]); // 데몬 보스 스테이지 지정
        }
        else if (stageCount == 21) // 엔딩 스테이지
        {
            currentStage = Instantiate(stage[3]); 
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
        Debug.Log(stageCount);
        stageCount++;
        
    }
    
}
