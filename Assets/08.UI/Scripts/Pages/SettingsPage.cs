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
    
    // public Toggle VFXToggle;
    // public Slider VFXVolumeSlider;
    
    public Button ExitButton;
    
    private void Start()
    {
        // Toggle BGMx
        BGMToggle.onValueChanged.AddListener(isChecked => audioManager.TurnBGMOn(isChecked));
        
        // BGM Slider
        BGMVolumeSlider.onValueChanged.AddListener(volume =>
        {
        float normalizedVolume = volume / 100f;
        audioManager._audioSource.volume = normalizedVolume; // do: setter로 캡슐화 알아보기
        });
        BGMVolumeSlider.value = 50f;
        
        // BGM selector
        BGMSelector.options.Clear();
        BGMSelector.options.AddRange(audioManager.bgmList.Select(list => new TMP_Dropdown.OptionData(list.name)));
        BGMSelector.value = audioManager.bgmList.FindIndex(list => list.name == audioManager.currentBGM.name);
        BGMSelector.RefreshShownValue();
        BGMSelector.onValueChanged.AddListener((value) => audioManager.ChangeBGM(value));
        
        // Exit Button
        ExitButton.onClick.AddListener(() => uiManager.GoTo(PageType.HOME_PAGE));
    }
}
