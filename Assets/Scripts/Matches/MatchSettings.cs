using System;
using UnityEngine;
using System.Collections;

namespace KitchenChaos.Matches
{
    [CreateAssetMenu(fileName = "MatchSettings", menuName = EditorPaths.SO + "Match Settings", order = 110)]
    public sealed class MatchSettings : ScriptableObject, IDisposable
    {
        [SerializeField, Min(0)] private uint countDown = 3;
        [SerializeField, Min(0)] private uint timeLimit = 120;
        [SerializeField, Min(0)] private uint finalSeconds = 20;

        public event Action OnStarted;
        public event Action OnFinished;
        public event Action OnFinalSecondsStarted;

        public TimeDown TimeLimit { get; private set; } = new TimeDown();
        public TimeDown CountDown { get; private set; } = new TimeDown();

        private MatchManager manager;

        internal void Initialize(MatchManager manager)
        {
            this.manager = manager;

            TimeLimit.totalTime = timeLimit;
            CountDown.totalTime = countDown;

            OnStarted?.Invoke();
        }

        internal void StartTimeLimit()
        {
            manager.StartCoroutine(TimeLimit.CountDownRoutine());
            manager.StartCoroutine(InvokeOnFinalSecondsStartedRoutine());
        }

        internal void StartCountDown() => manager.StartCoroutine(CountDown.CountDownRoutine());

        internal void FinishTimeLimit() => OnFinished?.Invoke();

        private IEnumerator InvokeOnFinalSecondsStartedRoutine()
        {
            var timeToFinalSeconds = timeLimit - finalSeconds;
            yield return new WaitForSeconds(timeToFinalSeconds);
            OnFinalSecondsStarted?.Invoke();
        }

        public void Dispose() => manager.StopAllCoroutines();
    }
}