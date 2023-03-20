using System;
using UnityEngine;

namespace KitchenChaos.Matches
{
    [CreateAssetMenu(fileName = "MatchSettings", menuName = EditorPaths.SO + "Match Settings", order = 110)]
    public sealed class MatchSettings : ScriptableObject, IDisposable
    {
        [SerializeField, Min(0)] private uint countDown = 3;
        [SerializeField, Min(0)] private uint timeLimit = 120;

        public event Action OnStarted;
        public event Action OnFinished;

        public TimeDown TimeLimit { get; private set; } = new TimeDown();
        public TimeDown CountDown { get; private set; } = new TimeDown();

        private MatchManager manager;
        private Coroutine timeLimitRoutine;
        private Coroutine countDownRoutine;

        internal void Initialize(MatchManager manager)
        {
            this.manager = manager;

            TimeLimit.totalTime = timeLimit;
            CountDown.totalTime = countDown;

            OnStarted?.Invoke();
        }

        internal void StartTimeLimit() => timeLimitRoutine = manager.StartCoroutine(TimeLimit.CountDownRoutine());

        internal void StartCountDown() => countDownRoutine = manager.StartCoroutine(CountDown.CountDownRoutine());

        internal void FinishTimeLimit() => OnFinished?.Invoke();

        public void Dispose()
        {
            if (timeLimitRoutine != null) manager.StopCoroutine(timeLimitRoutine);
            if (countDownRoutine != null) manager.StopCoroutine(countDownRoutine);
        }
    }
}