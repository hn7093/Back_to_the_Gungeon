using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    [SerializeField] public List<SkinData> allSkins; // 모든 스킨 데이터 저장
    private int currentSkinIndex = 0;
    public int CurrentSkinIndex { get => currentSkinIndex; set { currentSkinIndex = value; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        currentSkinIndex = PlayerPrefs.GetInt(PlayerController.skinIndexKey, 0);
    }

    public void UnlockSkin(int index)
    {
        if (index >= 0 && index < allSkins.Count)
        {
            allSkins[index].isUnlocked = true;
            PlayerPrefs.SetInt("SkinUnlocked_" + index, 1); // 저장
        }
    }

    public bool IsSkinUnlocked(int index)
    {
        return PlayerPrefs.GetInt("SkinUnlocked_" + index, 0) == 1;
    }

    public SkinData GetCurrentSkin()
    {
        return allSkins[currentSkinIndex];
    }

    public void SetSkin(int index)
    {
        if (index >= 0 && index < allSkins.Count && IsSkinUnlocked(index))
        {
            currentSkinIndex = index;
            PlayerPrefs.SetInt("SelectedSkin", index);
        }
    }
}
