using System;
using UnityEngine;

namespace KitchenChaos.Score
{
    [CreateAssetMenu(fileName = "ScoreSettings", menuName = EditorPaths.SO + "Score Settings", order = 110)]
    public sealed class ScoreSettings : ScriptableObject
    {
        [SerializeField, Min(0F)] private float deliveredOrdersMultiplier = 20F;
        [SerializeField, Min(0F)] private float failedOrdersMultiplier = 10F;

        public event Action<float> OnScoreIncreased;
        public event Action<float> OnScoreDecreased;

        public uint Tips { get; private set; }
        public uint FailedDeliveries { get; private set; }
        public uint SuccessfulDeliveries { get; private set; }

        public float Score { get; private set; }

        internal void Initialize()
        {
            Tips = 0;
            FailedDeliveries = 0;
            SuccessfulDeliveries = 0;
        }

        public void DeliveryOrder(uint tip)
        {
            Tips += tip;
            SuccessfulDeliveries++;

            var lastScore = Score;
            Score = CalculateScore();

            var deltaScore = Score - lastScore;
            OnScoreIncreased?.Invoke(deltaScore);
        }

        public void DeliveryFailedOrder()
        {
            FailedDeliveries++;

            var lastScore = Score;
            Score = CalculateScore();

            var deltaScore = Score - lastScore;
            OnScoreDecreased?.Invoke(deltaScore);
        }

        private float CalculateScore() =>
            Tips +
            SuccessfulDeliveries * deliveredOrdersMultiplier -
            FailedDeliveries * failedOrdersMultiplier;
    }
}