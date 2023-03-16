using UnityEngine;

namespace KitchenChaos.Levels
{
    [DisallowMultipleComponent]
    public sealed class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelSettings settings;

        private void Start()
        {
            settings.Initialize();
            settings.StartInitialCountDown();
        }

        private void OnEnable()
        {
            settings.InitialCountDown.OnFinished += HandleInitialCountDownFinished;
        }

        private void OnDisable()
        {
            settings.InitialCountDown.OnFinished -= HandleInitialCountDownFinished;
        }

        private void HandleInitialCountDownFinished() => settings.StartTimeLimit();
    }
}