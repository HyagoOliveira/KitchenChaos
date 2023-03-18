using UnityEngine;
using KitchenChaos.Levels;
using ActionCode.PauseSystem;

namespace KitchenChaos.Players
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerSettings settings;
        [SerializeField] private MatchSettings matchSettings;
        [SerializeField] private PauseSettings pauseSettings;
        [SerializeField] private PlayerInputSettings inputSettings;

        private void Awake()
        {
            settings.Initialize();
            inputSettings.Initialize();
        }

        private void Start()
        {
            inputSettings.Enable();
            settings.EnableFirstPlayer();
        }

        private void OnEnable()
        {
            inputSettings.BindActions();

            matchSettings.OnFinished += HandleMatchFinished;

            inputSettings.OnPause += HandlePlayerPause;
            inputSettings.OnSwitch += HandlePlayerSwitch;

            pauseSettings.OnPaused += HandlePaused;
            pauseSettings.OnResumed += HandleResumed;
        }

        private void OnDisable()
        {
            matchSettings.OnFinished -= HandleMatchFinished;

            inputSettings.OnPause -= HandlePlayerPause;
            inputSettings.OnSwitch -= HandlePlayerSwitch;

            pauseSettings.OnPaused -= HandlePaused;
            pauseSettings.OnResumed -= HandleResumed;

            inputSettings.UnBindActions();
        }

        private void HandleMatchFinished()
        {
            settings.DisableAllPlayers();
            settings.DisablePlayerSwitch();
        }

        private void HandlePlayerSwitch()
        {
            inputSettings.ResetAxis();
            settings.Switch();
        }

        private void HandlePlayerPause() => pauseSettings.Pause();

        private void HandlePaused() => inputSettings.Disable();
        private void HandleResumed() => inputSettings.Enable();
    }
}