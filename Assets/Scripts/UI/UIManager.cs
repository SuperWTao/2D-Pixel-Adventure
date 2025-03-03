using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>, ISaveable
{
    public int totalScore;
    private int lastScore;
    // public Text scoreText;

    public GameObject top;
    public GameObject player;
    
    public GameObject gameOverPanel;

    private void Start()
    {
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void OnEnable()
    {
        EventHandler.PlayerDeadEvent += OnPlayerDeadEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        EventHandler.SceneLoadedEvent += OnSceneLoadedEvent;
        EventHandler.RestartGameEvent += OnRestartGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.PlayerDeadEvent -= OnPlayerDeadEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        EventHandler.SceneLoadedEvent -= OnSceneLoadedEvent;
        EventHandler.RestartGameEvent -= OnRestartGameEvent;
    }

    private void OnSceneLoadedEvent()
    {
        lastScore = totalScore;
    }

    private void OnRestartGameEvent()
    {
        UpdateTotalScore();
    }

    private void OnStartNewGameEvent()
    {
        top.SetActive(true);
        player.SetActive(true);
    }

    public void UpdateTotalScore()
    {
        top.transform.GetChild(0).GetComponent<Text>().text = "score:" + totalScore.ToString();
    }

    public void OnPlayerDeadEvent()
    {
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        totalScore = lastScore;
        EventHandler.CallRestartGameEvent();
        gameOverPanel.SetActive(false);
        Scene scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        // SceneManager.UnloadSceneAsync(scene.name);
        // SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Additive);
        // // 场景加载事件
        // EventHandler.CallAfterSceneLoadedEvent();
        StartCoroutine(ReloadScene(scene.name));
    }

    public IEnumerator ReloadScene(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
        // 场景加载事件
        EventHandler.CallAfterSceneLoadedEvent();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.totalScore = totalScore;
        return saveData;
    }

    public void RestoreGameData(GameSaveData gameSaveData)
    {
        totalScore = gameSaveData.totalScore;
    }
}
