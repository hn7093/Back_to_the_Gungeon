using Preference;
using UnityEngine;


public class TutorialEvent : MonoBehaviour
{
    public int step;
    private void OnDestroy()
    {
        Debug.Log(step);
        switch (step)
        {
            case 2:
                SystemManager.Instance.EventManager.isTutorial2Clear = true;
                Debug.Log("asd");
                break;
            case 3:
                SystemManager.Instance.EventManager.isTutorial3Clear = true;
                Debug.Log("qwe");
                break;
        }
    }
}
