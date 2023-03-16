using UnityEngine;

namespace KitchenChaos.Levels
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class MatchManager : MonoBehaviour
    {
        [SerializeField] private MatchSettings settings;

        private void Awake() => settings.Initialize();
        private void Start() => Invoke(nameof(StartInitialCountDown), 0.2F);

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

        private void StartInitialCountDown() => settings.StartCountDown();

        private void HandleCountDownFinished() => settings.StartTimeLimit();

        private void HandleTimeLimitFinished() => settings.FinishTimeLimit();
    }
}