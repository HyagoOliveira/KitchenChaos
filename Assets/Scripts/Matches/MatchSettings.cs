using System;
using UnityEngine;

namespace KitchenChaos.Matches
{
    [CreateAssetMenu(fileName = "MatchSettings", menuName = EditorPaths.SO + "Match Settings", order = 110)]
    public sealed class MatchSettings : ScriptableObject, IDisposable
    {
        [field: SerializeField] public TimeDown CountDown { get; private set; } = new TimeDown(3);
        [field: SerializeField] public TimeLimit TimeLimit { get; private set; } = new TimeLimit(120, 20);
        [field: NonSerialized] public bool IsAllowToStartTimeLimit { get; set; } = true;

        private MatchManager manager;

        internal void Initialize(MatchManager manager) => this.manager = manager;

        internal void StartTimeLimit()
        {
            if (!IsAllowToStartTimeLimit) return;

            manager.StartCoroutine(TimeLimit.CountDownRoutine());
            manager.StartCoroutine(TimeLimit.FinalSecondsRoutine());
        }

        internal void StartCountDown() => manager.StartCoroutine(CountDown.CountDownRoutine());

        public void Dispose() => manager.StopAllCoroutines();
    }
}