using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UIState
{
    Home,
    Game,
    GameOver,

}
public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }
    [SerializeField] private HomeUI homeUI;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private GameOverUI gameOverUI;
    private UIState currentState;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        homeUI.Init();
        gameUI.Init();
        gameOverUI.Init();

        ChangeState(UIState.Home);
    }
    public void SetPlayGame()
    {
        ChangeState(UIState.Game);
    }

    public void SetGameOver()
    {
        ChangeState(UIState.GameOver);
    }

    public void ChangeWave(int waveIndex)
    {
        gameUI.UpdateWaveText(waveIndex);
    }

    public void ChangePlayerHP(float currentHP, float maxHP)
    {
        gameUI.UpdateHPSlider(currentHP / maxHP);
    }

    public void ChangeState(UIState state)
    {
        currentState = state;
        homeUI.SetActive(currentState);
        gameUI.SetActive(currentState);
        gameOverUI.SetActive(currentState);
    }
}
