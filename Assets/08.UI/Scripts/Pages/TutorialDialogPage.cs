using Preference;
using TMPro;

public class TutorialDialogPage : UIMonoBehaviour
{
    public TMP_Text content;
    
    private bool AllTaskDone = false;
    
    void Start()
    { 
        audioManager.PlayVFXSoundByName("alert");
        content.text = "방향키로 이동하여 빨간 바닥까지 이동해보세요.";
    }

    private void Update()
    {
        if (AllTaskDone) return;
        
        if (eventManager.isTutorial1Clear)
        {
            audioManager.PlayVFXSoundByName("alert");
            content.text = "몬스터를 클릭을 통해 처리해보세요.";
            eventManager.isTutorial1Clear = false;
        }
        
        if (eventManager.isTutorial2Clear)
        {
            audioManager.PlayVFXSoundByName("alert");
            content.text = "NPC에게 보상을 받아보세요.";
            eventManager.isTutorial2Clear = false;
        }

        if (eventManager.isTutorial3Clear)
        {
            AllTaskDone = true;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
