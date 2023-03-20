using UnityEngine;

namespace KitchenChaos.Matches
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class MatchManager : MonoBehaviour
    {
        [SerializeField] private MatchSettings settings;

        private void Awake() => settings.Initialize();
        private void Start() => Invoke(nameof(StartCountDown), 0.2F);

        private void OnEnable()
        {
            settings.CountDown.OnFinished += HandleCountDownFinished;
            settings.TimeLimit.OnFinished += HandleTimeLimitFinished;
        }

        private void OnDisable()
        {
            settings.CountDown.OnFinished -= HandleCountDownFinished;
            settings.TimeLimit.OnFinished -= HandleTimeLimitFinished;
        }

        private void StartCountDown() => settings.StartCountDown();

        private void HandleCountDownFinished() => settings.StartTimeLimit();

        private void HandleTimeLimitFinished() => settings.FinishTimeLimit();
    }
}