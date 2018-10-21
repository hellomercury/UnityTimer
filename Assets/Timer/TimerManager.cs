using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private  List<Timer> timerList;
    private List<int> completedIndexList;
    private int timerCount, completedIndexCount;

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
        
        if (time < .1f) return;
        time -= .1f;
         
        for (int i = 0; i < timerCount; i++)
        {
            timerList[i].Update();
            if (!timerList[i].isCompleted) continue;
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
        
        if(completedIndexCount > 0) completedIndexList.Clear();

        if(timerCount == 0) Destroy(gameObject);
    }
}
