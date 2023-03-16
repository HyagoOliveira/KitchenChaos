using System;
using UnityEngine;
using ActionCode.AwaitableCoroutines;

namespace KitchenChaos.Levels
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = EditorPaths.SO + "Level Settings", order = 110)]
    public sealed class LevelSettings : ScriptableObject
    {
        [SerializeField, Min(0)] private uint initialCountDown = 3;
        [SerializeField, Min(0)] private uint timeLimit = 120;

        public event Action OnLevelStarted;
        public event Action OnLevelFinished;

        public TimeDown TimeLimit { get; private set; }
        public TimeDown InitialCountDown { get; private set; }

        internal void Initialize()
        {
            TimeLimit = new TimeDown(timeLimit);
            InitialCountDown = new TimeDown(initialCountDown);

            OnLevelStarted?.Invoke();
        }

        internal void StartTimeLimit() => _ = AwaitableCoroutine.Run(TimeLimit.CountDownRoutine());

        internal void StartInitialCountDown() => _ = AwaitableCoroutine.Run(InitialCountDown.CountDownRoutine());
    }
}