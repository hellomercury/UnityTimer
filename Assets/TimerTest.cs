using UnityEngine;
using UnityEngine.UI;

namespace Framework.Tools
{
    public sealed class TimerTest : MonoBehaviour
    {
        private Text text;

        private Countdown countdown;

        // Use this for initialization
        void Start()
        {
            text = transform.Find("Countdown").GetComponent<Text>();
            countdown = Timer.RegisterCountdown(66, TimeType.hhmmss, text,
                () => { Debug.LogError("Countdown completed."); },
                1,
                this);
            countdown.Run();
        }

        public void Pause()
        {
            countdown.Pause();
        }

        public void Resume()
        {
            countdown.Resume();
        }

        public void Cancel()
        {
            countdown.Cancel();
        }

        public void AddTime()
        {
            countdown.AddTime(6.5f);
        }

        public void SubTime()
        {
            countdown.SubTimer(6.1f);
        }
    }
}