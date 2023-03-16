using System;
using System.Collections;
using UnityEngine;

namespace KitchenChaos.Levels
{
    public sealed class TimeDown
    {
        public event Action OnStarted;
        public event Action OnFinished;
        public event Action<uint> OnUpdated;

        private readonly uint totalTime;
        private readonly WaitForSeconds waitOneSecond;

        public TimeDown(uint totalTime)
        {
            this.totalTime = totalTime;
            waitOneSecond = new WaitForSeconds(1F);
        }

        internal IEnumerator CountDownRoutine()
        {
            var current = totalTime;

            OnStarted?.Invoke();

            do
            {
                OnUpdated?.Invoke(current);
                yield return waitOneSecond;
                current--;
            } while (current > 0);

            OnFinished?.Invoke();
        }
    }
}