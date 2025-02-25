using System;
using Preference;
using UnityEngine.UI;

public class DevilPage : UIMonoBehaviour
{
    public Button ExitButton;

    private void Start()
    {
        ExitButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
