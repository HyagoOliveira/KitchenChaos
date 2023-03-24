using UnityEngine;
using KitchenChaos.Matches;

namespace KitchenChaos.Orders
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class OrderManager : MonoBehaviour
    {
        [field: SerializeField] public OrderSettings Settings { get; private set; }
        [field: SerializeField] public bool StartOrderingAfterCountDown { get; set; } = true;

        [SerializeField] private MatchSettings matchSettings;

        public static OrderManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            Settings.Initialize(this);
        }

        private void OnEnable()
        {
            if (StartOrderingAfterCountDown)
                matchSettings.CountDown.OnFinished += HandleCountDownFinished;

            matchSettings.TimeLimit.OnFinished += HandleTimeLimitFinished;
        }

        private void OnDisable()
        {
            matchSettings.CountDown.OnFinished -= HandleCountDownFinished;
            matchSettings.TimeLimit.OnFinished -= HandleTimeLimitFinished;
        }

        private void HandleCountDownFinished() => Settings.StartOrdering();
        private void HandleTimeLimitFinished() => Settings.StopOrdering();
    }
}