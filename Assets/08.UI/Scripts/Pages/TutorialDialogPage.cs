using Preference;
using TMPro;
using UnityEngine;

public class TutorialDialogPage : UIMonoBehaviour
{
    public TMP_Text content;
    
    private bool AllTaskDone = false;
    
    void Start()
    { 
        audioManager.PlayVFXSoundByName("alert");
        content.text = "방향키로 이동하여 지정된 위치까지 이동해보세요.";
    }

    private void Update()
    {
        if (AllTaskDone) return;
        
        if (eventManager.isTutorial1Clear)
        {
            audioManager.PlayVFXSoundByName("alert");
            content.text = "NPC에게 다가가 보상을 받아보세요.";
            eventManager.isTutorial1Clear = false;
        }
        
        if (eventManager.isTutorial2Clear)
        {
            audioManager.PlayVFXSoundByName("alert");
            content.text = "문을 통과하여 다음턴으로 이동하세요.";
            eventManager.isTutorial2Clear = false;
        }

        if (eventManager.isTutorial3Clear)
        {
            AllTaskDone = true;
            gameObject.SetActive(false);
            SystemManager.Instance.UIManager.Clear();
            Destroy(gameObject);
        }
    }
}
