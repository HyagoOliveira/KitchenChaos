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

        public int Tips { get; private set; }
        public int FailedDeliveries { get; private set; }
        public int SuccessfulDeliveries { get; private set; }

        public float Score { get; private set; }

        internal void Initialize()
        {
            Tips = 0;
            Score = 0f;
            FailedDeliveries = 0;
            SuccessfulDeliveries = 0;
        }

        public void DeliveryOrder(int tip)
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

            var deltaScore = lastScore - Score;
            OnScoreDecreased?.Invoke(deltaScore);
        }

        private float CalculateScore()
        {
            var score =
                Tips +
                SuccessfulDeliveries * deliveredOrdersMultiplier -
                FailedDeliveries * failedOrdersMultiplier;
            return Mathf.Max(0f, score);
        }
    }
}