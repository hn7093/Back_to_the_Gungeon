using Preference;
using UnityEngine;
using UnityEngine.UI;

public class IntroPage : MonoBehaviour
{
    public Button StartButton;
    
    void Start()
    {
        StartButton.onClick.AddListener(() => SystemManager.Instance.FileManager.StartGame());
    }
}
