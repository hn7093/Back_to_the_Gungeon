using Preference;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPage : UIMonoBehaviour
{
    public Button ExitButton;

    private void Start()
    {
        ExitButton.onClick.AddListener(() => uiManager.OpenPage(PageType.HOME_PAGE));
    }
}
