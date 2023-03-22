using System;
using System.Collections;
using UnityEngine;

namespace KitchenChaos.Matches
{
    public sealed class TimeLimit : TimeDown
    {
        [SerializeField, Min(0)] private uint finalSeconds;

        public event Action OnFinalSecondsStarted;

        public TimeLimit(uint time, uint finalSeconds) :
            base(time) => this.finalSeconds = finalSeconds;

        internal IEnumerator FinalSecondsRoutine()
        {
            var timeToFinalSeconds = Time - finalSeconds;
            yield return new WaitForSeconds(timeToFinalSeconds);
            OnFinalSecondsStarted?.Invoke();
        }
    }
}