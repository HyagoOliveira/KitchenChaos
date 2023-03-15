using System;
using System.Collections;
using UnityEngine;
using ActionCode.AwaitableCoroutines;

namespace KitchenChaos.Levels
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = EditorPaths.SO + "Level Settings", order = 110)]
    public sealed class LevelSettings : ScriptableObject
    {
        [SerializeField, Min(0)] private uint totalCountDown = 3;

        public event Action OnLevelStarted;
        public event Action OnLevelFinished;

        public event Action OnCountDownStarted;
        public event Action<uint> OnCountDownUpdated;
        public event Action OnCountDownFinished;

        internal void Initialize() => OnLevelStarted?.Invoke();

        internal void StartCountDown() => _ = AwaitableCoroutine.Run(CountDownRoutine());

        private IEnumerator CountDownRoutine()
        {
            var countDown = totalCountDown;
            var waitOneSecond = new WaitForSecondsRealtime(1F);

            OnCountDownStarted?.Invoke();

            do
            {
                OnCountDownUpdated?.Invoke(countDown);
                yield return waitOneSecond;
                countDown--;
            } while (countDown > 0);

            OnCountDownFinished?.Invoke();
        }
    }
}