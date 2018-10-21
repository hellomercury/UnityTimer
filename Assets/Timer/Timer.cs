using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class Timer
{
    private float duration;
    private float timeRemain;
    private bool isLooped;
    public bool isCompleted;
    public bool isPaused;
    public bool isCancelled;
    public bool isDone;

    public Action onFinishedAction;
    public Action<float> onUpdate;

    private TimerManager timerManager;
    private TimerManager timerMgr
    {
        get
        {
            if (null == timerManager)
            {
                timerManager = Object.FindObjectOfType<TimerManager>();
                if(null == timerManager) timerManager = new GameObject("TimeManager").AddComponent<TimerManager>();
            }

            return timerManager;
        }
    }
    
    private Timer(float InDuration, Action InOnFinishedAction)
    {
        duration = InDuration;
        timeRemain = InDuration;
        isLooped = false;
        isCompleted = false;
        isPaused = false;
        isCancelled = false;
        isDone = false;

        onFinishedAction = InOnFinishedAction;
    }

    private Timer(float InDuration, Action<float> InOnUpdate, Action InOnFinishedAction)
    {
        duration = InDuration;
        timeRemain = InDuration;
        isLooped = false;
        isCompleted = false;
        isPaused = false;
        isCancelled = false;
        isDone = false;

        onUpdate = InOnUpdate;
        onFinishedAction = InOnFinishedAction;
    }

    public void Run()
    {
        timerMgr.AddTimer(this);
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void AddTimer(float InTime)
    {
        if(isDone) Debug.LogWarning("Timer already done.");
        else
        {
            duration += InTime;
            timeRemain += InTime;
        }
    }

    public void SubTimer(float InTime)
    {
        if (isDone) Debug.LogWarning("Timer already done.");
        else
        {
            duration -= InTime;
            timeRemain -= InTime;
        }
    }

    public void Cancel(bool InIsCompleteActionInvoke = false)
    {
        isCancelled = true;
        if (InIsCompleteActionInvoke)
        {
            onFinishedAction.Invoke();
        }
    }

    public static Timer Register(float InDuration,
        Action<float> InOnUpdate,
        Action InOnComplete)
    {
        Timer timer = new Timer(InDuration, InOnUpdate, InOnComplete);
        timer.Run();
        return timer;
    }
    
    public void Update()
    {
        if(null != onUpdate) onUpdate.Invoke(TimerConfig.DELTA_TIME);

        timeRemain -= TimerConfig.DELTA_TIME;

        Debug.LogError(timeRemain);

        if (timeRemain <= 0)
        {
            if (isLooped)
            {
                timeRemain = duration;
                isCompleted = false;
            }
            else
            {
                isCompleted = true;
            }

            if (onFinishedAction != null) onFinishedAction.Invoke();
        }
    }
}
