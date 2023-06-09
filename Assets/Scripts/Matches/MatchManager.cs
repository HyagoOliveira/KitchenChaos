using UnityEngine;

namespace KitchenChaos.Matches
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class MatchManager : MonoBehaviour
    {
        [SerializeField] private MatchSettings settings;

        private void Awake() => settings.Initialize(this);
        private void Start() => Invoke(nameof(StartCountDown), 0.2F);
        private void OnEnable() => settings.CountDown.OnFinished += HandleCountDownFinished;
        private void OnDisable() => settings.CountDown.OnFinished -= HandleCountDownFinished;
        private void OnDestroy() => settings.Dispose();

        private void StartCountDown() => settings.StartCountDown();

        private void HandleCountDownFinished() => settings.StartTimeLimit();
    }
}