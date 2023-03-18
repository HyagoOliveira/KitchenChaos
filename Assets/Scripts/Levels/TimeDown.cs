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

        internal uint totalTime;

        private readonly WaitForSeconds waitOneSecond = new WaitForSeconds(1F);

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

            OnUpdated?.Invoke(current);
            OnFinished?.Invoke();
        }
    }
}