using System.Collections.Generic;
using UnityEngine;

namespace Framework.Tools
{
    public sealed class TimerManager : MonoBehaviour
    {
        private List<Timer> timerList;
        private List<int> completedIndexList;
        private int timerCount, completedIndexCount;

        private static TimerManager instance;

        public static TimerManager GetSingleton()
        {
            if (null == instance)
            {
                instance = FindObjectOfType<TimerManager>();
                if (null == instance)
                {
                    GameObject timerGameObj = new GameObject("TimerManager");
                    instance = timerGameObj.AddComponent<TimerManager>();
                    DontDestroyOnLoad(timerGameObj);
                }
            }

            return instance;
        }

        public static void DestroySingleton()
        {
            Destroy(instance.gameObject);
            instance = null;
        }

        private void Awake()
        {
            timerList = new List<Timer>(10);
            timerCount = 0;
            completedIndexList = new List<int>(10);
            completedIndexCount = 0;
        }

        public void AddTimer(Timer InTimer)
        {
            lock (timerList)
            {
                timerList.Add(InTimer);
                ++timerCount;
            }
        }

        private float time;
        private void Update()
        {
            if (timerCount == 0) return;

            time += Time.deltaTime;

            if (time < TimerConfig.DELTA_TIME) return;
            time -= TimerConfig.DELTA_TIME;

            for (int i = 0; i < timerCount; i++)
            {
                timerList[i].Update();
                if (!timerList[i].IsDone) continue;
                ++completedIndexCount;
                completedIndexList.Add(i);
            }

            lock (timerList)
            {
                for (int i = 0; i < completedIndexCount; i++)
                {
                    --timerCount;
                    timerList.RemoveAt(completedIndexList[i]);
                }
            }

            if (completedIndexCount > 0) completedIndexList.Clear();
        }
    }
}