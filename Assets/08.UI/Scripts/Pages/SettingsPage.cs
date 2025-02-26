using System.Linq;
using Preference;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPage : UIMonoBehaviour
{
    public Toggle BGMToggle;
    public Slider BGMVolumeSlider;
    public TMP_Dropdown BGMSelector;
    
    public Toggle VFXToggle;
    public Slider VFXVolumeSlider;

    public TMP_Dropdown KeyTypeSelector;

    public Button ExitButton;
    
    // fix: 만약 처음에 활성화되면 순서 문제로 에러 발생
    private void Start()
    {
        // Toggle BGM
        BGMToggle.onValueChanged.AddListener(isChecked => audioManager.TurnOn(isChecked, "BGM"));
        
        // BGM Slider
        BGMVolumeSlider.onValueChanged.AddListener(volume => audioManager.UpdateBGMVolume(volume));
        BGMVolumeSlider.value = 50f;
        
        // BGM selector
        BGMSelector.options.Clear();
        BGMSelector.options.AddRange(audioManager.bgmList.Select(list => new TMP_Dropdown.OptionData(list.name)));
        BGMSelector.value = audioManager.bgmList.FindIndex(list => list.name == audioManager.currentBGM.name);
        BGMSelector.RefreshShownValue();
        BGMSelector.onValueChanged.AddListener((value) => audioManager.UpdateBGMSourceClip(value));
        
        // VFX Toggle
        VFXToggle.onValueChanged.AddListener(isChecked => audioManager.TurnOn(isChecked, "VFX"));
        
        // KeyType Selector
        KeyTypeSelector.onValueChanged.AddListener(value => keyBinding.UpdateKeyType(value));
        
        // Exit Button
        ExitButton.onClick.AddListener(() => uiManager.GoTo(PageType.HOME_PAGE));
    }
}
