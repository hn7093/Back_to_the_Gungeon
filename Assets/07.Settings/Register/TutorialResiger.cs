using Preference;
using UnityEngine;


public class TutorialEvent : MonoBehaviour
{
    [Range(2, 3)]
    public int step;
    private void OnDestroy()
    {
        switch (step)
        {
            case 2:
                SystemManager.Instance.EventManager.isTutorial2Clear = true;
                break;
            case 3:
                SystemManager.Instance.EventManager.isTutorial3Clear = true;
                break;
        }
    }
}
