using Preference;
using UnityEngine;
using UnityEngine.UI;

public class StartGamePage : UIMonoBehaviour
{
    public Button StartGameBtn;
    public Button SettingsBtn;
    public Button LoadBtn;
    public Button ExitBtn;

    private void Start()
    {
        SettingsBtn.onClick.AddListener(() => uiManager.GoTo(PageType.SETTINGS_PAGE));
        ExitBtn.onClick.AddListener(() => fileManager.ExitGame());
    }
}
