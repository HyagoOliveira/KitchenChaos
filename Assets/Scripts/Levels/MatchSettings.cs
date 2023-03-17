using System;
using UnityEngine;
using ActionCode.AwaitableCoroutines;

namespace KitchenChaos.Levels
{
    [CreateAssetMenu(fileName = "MatchSettings", menuName = EditorPaths.SO + "Match Settings", order = 110)]
    public sealed class MatchSettings : ScriptableObject
    {
        [SerializeField, Min(0)] private uint countDown = 3;
        [SerializeField, Min(0)] private uint timeLimit = 120;

        public event Action OnStarted;
        public event Action OnFinished;

        public TimeDown TimeLimit { get; private set; } = new TimeDown();
        public TimeDown CountDown { get; private set; } = new TimeDown();

        internal void Initialize()
        {
            TimeLimit.totalTime = timeLimit;
            CountDown.totalTime = countDown;

            OnStarted?.Invoke();
        }

        internal void StartTimeLimit() => _ = AwaitableCoroutine.Run(TimeLimit.CountDownRoutine());

        internal void StartCountDown() => _ = AwaitableCoroutine.Run(CountDown.CountDownRoutine());

        internal void FinishTimeLimit() => OnFinished?.Invoke();
    }
}