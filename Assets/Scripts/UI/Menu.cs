using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void NewGame()
    {
        TransitionManager.Instance.NewGame();
        EventHandler.CallStartNewGameEvent();
    }

    public void ContinueGame()
    {
        // 加载游戏进度
        SaveLoadManager.Instance.Load();
        UIManager.Instance.top.SetActive(true);
        UIManager.Instance.player.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoBackToMenu()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        TransitionManager.Instance.Transition(currentScene, "Menu");
        // 将player禁用
        UIManager.Instance.player.SetActive(false);
        UIManager.Instance.top.SetActive(false);
        // 保存游戏进度
        SaveLoadManager.Instance.Save();
    }
}
