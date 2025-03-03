using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>, ISaveable
{
    public CanvasGroup fadeCanvasGroup;
    
    public float fadeDuration;
    
    private bool isFade;
    

    private void Start()
    {
        // 游戏开始加载主菜单场景
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name != "Menu" && !isFade)
        {
            UIManager.Instance.gameOverPanel.SetActive(!UIManager.Instance.gameOverPanel.activeInHierarchy);
        }
    }

    public void NewGame()
    {
        // UIManager.Instance.scoreText.gameObject.SetActive(true);
        // player.SetActive(true);
        EventHandler.CallStartNewGameEvent();
        Transition("Menu", "Game 1");
    }
    
    public void Transition(string from, string to)
    {
        if(!isFade)
            StartCoroutine(TransitionToScene(from, to));
    }

    public IEnumerator TransitionToScene(string from, string to)
    {
        yield return Fade(1);
        EventHandler.CallSceneLoadedEvent();
        yield return SceneManager.UnloadSceneAsync(from);
        yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        
        // 按照当前激活的场景数量来设置激活场景, 设置新场景为激活场景
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
        
        // 找到场景中的起始位置
        GameObject start = GameObject.FindGameObjectWithTag("Start");
        
        // 将人物移动到新场景的起始位置
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player!= null)
        {
            player.transform.position = start.transform.position;
        }
        EventHandler.CallAfterSceneLoadedEvent();
        yield return Fade(0);
        
    }
    
    /// <summary>
    /// 场景的渐入渐出
    /// </summary>
    /// <param name="targetAlpha">1是黑色， 0是白色</param>
    /// <returns></returns>
    public IEnumerator Fade(float targetAlpha)
    {
        isFade = true;
        
        fadeCanvasGroup.blocksRaycasts = true;
        
        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        
        fadeCanvasGroup.blocksRaycasts = false;
        
        isFade = false;
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.currentScene = SceneManager.GetActiveScene().name;
        return saveData;
    }

    public void RestoreGameData(GameSaveData gameSaveData)
    {
        Transition("Menu", gameSaveData.currentScene);
    }
}
