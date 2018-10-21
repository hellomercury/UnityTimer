using UnityEngine;

public class TimerTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Timer.Register(30,
            InDeltaTime => Debug.LogError(InDeltaTime),
            () => Debug.LogError("Timer completed."));
    }
}
