using Preference;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StartGamePage : UIMonoBehaviour
{
    public Button StartGameButton;
    public Button SettingsButton;
    // public Button InventoryButton;
    public Button ChangeSkinButton;
    public Button ExitButton;

    private void Start()
    {
        StartGameButton.onClick.AddListener(() => uiManager.OpenPage(PageType.INTRO_PAGE));
        SettingsButton.onClick.AddListener(() => uiManager.OpenPage(PageType.SETTINGS_PAGE));
        ChangeSkinButton.onClick.AddListener(() => uiManager.OpenPage(PageType.INVENTORY_PAGE));
        ExitButton.onClick.AddListener(() => fileManager.ExitGame());
    }
}
