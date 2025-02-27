using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public PlayerController player { get; private set; }
    private ResourceController playerResourceController;
    [SerializeField] private int currentWaveIndex = 0;
    public static bool isFirstLoading = true;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        player = FindObjectOfType<PlayerController>();
    }
    void Start()
    {
        if (!isFirstLoading)
        {
            StartGame();
        }
        else
        {
            isFirstLoading = false;
            //playerResourceController = player.GetComponent<ResourceController>();                           
            //_playerResourceController.RemoveHealthChangeEvent(uiManager.ChangePlayerHP);
            //_playerResourceController.AddHealthChangeEvent(uiManager.ChangePlayerHP);
            //playerResourceController.RemoveHealthChangeEvent(UIManager.Instance.ChangePlayerHP);
            //playerResourceController.AddHealthChangeEvent(UIManager.Instance.ChangePlayerHP);
        }
    }

    public void StartGame()
    {
        UIManager.Instance.SetPlayGame();
        StartNextWave();
    }
    private void StartNextWave()
    {
        currentWaveIndex++;
        EnemyManager.Instance.StartWave(1 + currentWaveIndex / 5);
        UIManager.Instance.ChangeWave(currentWaveIndex);
    }
    public void EndWave()
    {
        StartNextWave();
    }
    public void GameOver()
    {
        EnemyManager.Instance.StopWave();
        UIManager.Instance.SetGameOver();
    }
}
