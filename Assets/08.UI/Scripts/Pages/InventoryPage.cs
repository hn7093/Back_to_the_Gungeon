using System.Collections;
using Preference;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPage : UIMonoBehaviour
{
    public Button ExitButton;
    public PlayerController PlayerController;
    public Button EquipButton;
    public TMP_Text AlertText;
    
    string weaponIndexKey = "weaponIndexKey";
    
    private void Start()
    {
        ExitButton.onClick.AddListener(() => uiManager.OpenPage(PageType.HOME_PAGE));
        EquipButton.onClick.AddListener(() =>
        {
            int currentWeaponId = PlayerPrefs.GetInt(weaponIndexKey);
            
            PlayerController.SetWeapon();
            if (currentWeaponId != PlayerPrefs.GetInt(weaponIndexKey))
            {
                AlertText.SetText("장비가 해금되지 않았습니다.");
                StartCoroutine(ClearAlertTextAfterDelay(0.5f));
            }
        });
    }
    
    private IEnumerator ClearAlertTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        AlertText.SetText("");
    }

}
