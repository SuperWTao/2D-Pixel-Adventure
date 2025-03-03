using System;
using UnityEngine;

public static class EventHandler
{
    public static event Action EnemyKilledEvent;
    public static void CallEnemyKilledEvent()
    {
        EnemyKilledEvent?.Invoke();
    }
    
    public static event Action PlayerDeadEvent;
    public static void CallPlayerDeadEvent()
    {
        PlayerDeadEvent?.Invoke();
    }
    
    public static Action RestartGameEvent;
    public static void CallRestartGameEvent()
    {
        RestartGameEvent?.Invoke();
    }
    
    public static Action SceneLoadedEvent;
    public static void CallSceneLoadedEvent()
    {
        SceneLoadedEvent?.Invoke();
    }
    
    public static Action AfterSceneLoadedEvent;
    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }
    
    public static Action StartNewGameEvent;

    public static void CallStartNewGameEvent()
    {
        StartNewGameEvent?.Invoke();
    }
}
