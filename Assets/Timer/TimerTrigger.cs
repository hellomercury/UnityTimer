using System;
using UnityEngine;

namespace Framework.Timer
{
    public sealed class TimerTrigger : TimerBase
    {
        private readonly Action<float> onUpdate;

        public TimerTrigger(float InDurationFloat, Action InOnCompletedAction,
            MonoBehaviour InMonoOwner = null)
            : base(InDurationFloat, TimerConfig.DELTA_TIME, InOnCompletedAction, InMonoOwner)
        {
        }

        public TimerTrigger(float InDurationFloat, Action InOnCompletedAction,
            Action<float> InOnUpdate,
            MonoBehaviour InMonoOwner = null)
            : base(InDurationFloat, TimerConfig.DELTA_TIME, InOnCompletedAction, InMonoOwner)
        {
            onUpdate = InOnUpdate;
        }

        public override void Update()
        {
            //已完成 || 已取消 || Text脚本已经被销毁
            if (IsDone) return;
            Debug.LogError("->" + IsPaused);
            if (IsPaused) return;
            Debug.LogError("<-" + IsPaused);

            RemainingTimeFloat -= IntervalsTimeFloat;

            if (HasMonoOwner && null == MonoOwner)
            {
                Debug.LogWarning("[TimerBase]    MonoBehaviour owner has been destroyed.");
                IsAutoKilled = true;
            }

            if (onUpdate != null) onUpdate.Invoke(RemainingTimeFloat);

            if (RemainingTimeFloat <= 0)
            {
                if(OnCompletedAction != null) OnCompletedAction.Invoke();
                IsCompleted = true;
            }
        }

        protected override void UpdateTime()
        {
            if (onUpdate != null) onUpdate.Invoke(RemainingTimeFloat);

            if (RemainingTimeFloat <= 0) IsCompleted = true;
        }
    }
}
