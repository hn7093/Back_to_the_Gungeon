using Preference;
using UnityEngine;


public class QuestEventDetector : MonoBehaviour
{
    public MonsterName name;

    private void OnDestroy()
    {
        SystemManager.Instance.EventManager.DestroyDetector(name);
    }
}
