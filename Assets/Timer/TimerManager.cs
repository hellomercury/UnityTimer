using System.Collections.Generic;
using UnityEngine;

namespace Framework.Timer
{
    public sealed class TimerManager : MonoBehaviour
    {
        private static int timerIndex;
        private Dictionary<int, TimerBase> timerDict;
        private List<int> completedIndexList;
        private int timerCount, completedIndexCount;

        private static TimerManager instance;
        private readonly object syncLock = new object();

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
            timerDict = new Dictionary<int, TimerBase>(10);
            timerCount = 0;
            completedIndexList = new List<int>(10);
            completedIndexCount = 0;
        }

        public void AddTimer(TimerBase InTimerBase)
        {
            lock (syncLock)
            {
                timerDict.Add(timerIndex, InTimerBase);
                ++timerIndex;
                ++timerCount;
            }
        }

        private float time;
        private void Update()
        {
            if (timerCount == 0)
            {
                timerIndex = 0;
                return;
            }

            time += Time.deltaTime;

            if (time < TimerConfig.DELTA_TIME) return;
            time -= TimerConfig.DELTA_TIME;
            
            lock (syncLock)
            {
                foreach (KeyValuePair<int, TimerBase> itor in timerDict)
                {
                    itor.Value.Update();
                    if (!itor.Value.IsDone) continue;
                    ++completedIndexCount;
                    completedIndexList.Add(itor.Key);
                }

                for (int i = 0; i < completedIndexCount; i++)
                {
                    --timerCount;
                    timerDict.Remove(completedIndexList[i]);
                }

                if (completedIndexCount > 0)
                {
                    completedIndexCount = 0;
                    completedIndexList.Clear();
                }
            }
        }
    }
}