using UnityEngine;

public static class EventManager
{
    /*
     A Static class to:

     * Manage global events and thereby reduce dependencies that can lead to NullReferenceExceptions on events.
     * Avoid needing script ordering to subscribe to and raise events.
    */

    static void PrintNullEventWarning(string in_EventName)
    {
        Debug.LogWarning("Tried to announce " + in_EventName + ", but no subscriptions are active.");
    }

    /*
    ============================
    TUG OF WAR EVENTS
    ============================
    */

    public delegate void TugOfWarEvent();
    public static event TugOfWarEvent OnTugOfWarEnabled;
    public static event TugOfWarEvent OnTugOfWarDisabled;
    public static event TugOfWarEvent OnExceededBoundary;
    public static event TugOfWarEvent OnBadThingMoved;

    static void RaiseEvent(TugOfWarEvent in_Event)
    {
        if (in_Event != null)
        {
            in_Event();
        }
        else
        {
            PrintNullEventWarning("TugOfWarEvent");
        }
    }

    public static void RaiseTugOfWarEnabled()
    {
        RaiseEvent(OnTugOfWarEnabled);
    }
    public static void RaiseTugOfWarDisabled()
    {
        RaiseEvent(OnTugOfWarDisabled);
    }
    public static void RaiseExceededBoundary()
    {
        RaiseEvent(OnExceededBoundary);
    }
    public static void RaiseBadThingMoved()
    {
        RaiseEvent(OnBadThingMoved);
    }

    /*
    ============================
    GAME MANAGER EVENTS 
    ============================
    */

    public delegate void GameManagerEvent();
    public static event GameManagerEvent OnPromptUpdated;

    public delegate void GameManagerEventGO(GameObject in_GameObject);
    public static event GameManagerEventGO OnPlayerInputCorrect;
    public static event GameManagerEventGO OnPlayerInputWrong;

    static void RaiseEvent(GameManagerEvent in_Event)
    {
        if (in_Event != null)
        {
            in_Event();
        }
        else
        {
            PrintNullEventWarning("GameManagerEvent");
        }
    }
    static void RaiseEventGO(GameManagerEventGO in_Event, GameObject in_GO)
    {
        if (in_Event != null)
        {
            in_Event(in_GO);
        }
        else
        {
            PrintNullEventWarning("GameManagerEventGO");
        }
    }

    public static void RaisePromptUpdated()
    {
        RaiseEvent(OnPromptUpdated);
    }
    public static void RaisePlayerInputCorrect(GameObject in_GO)
    {
        RaiseEventGO(OnPlayerInputCorrect, in_GO);
    }
    public static void RaisePlayerInputWrong(GameObject in_GO)
    {
        RaiseEventGO(OnPlayerInputWrong, in_GO);
    }

    /*
    ============================
    EXPLOSION EVENTS 
    ============================
    */

    public delegate void ExplosionEvent();
    public static event ExplosionEvent OnExplosionStarted;
    public static event ExplosionEvent OnExplosionFinished;

    static void RaiseEvent(ExplosionEvent in_Event)
    {
        if (in_Event != null)
        {
            in_Event();
        }
        else
        {
            PrintNullEventWarning("ExplosionEvent");
        }
    }

    public static void RaiseExplosionStarted()
    {
        RaiseEvent(OnExplosionStarted);
    }
    public static void RaiseExplosionFinished()
    {
        RaiseEvent(OnExplosionFinished);
    }
}



