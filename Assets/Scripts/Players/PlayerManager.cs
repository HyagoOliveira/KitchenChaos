using UnityEngine;
using KitchenChaos.Matches;
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
        [SerializeField] private bool canPause = true;

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

            inputSettings.OnSwitch += HandlePlayerSwitch;

            pauseSettings.OnPaused += HandlePaused;
            pauseSettings.OnResumed += HandleResumed;

            matchSettings.TimeLimit.OnFinished += HandleMatchFinished;

            if (canPause) inputSettings.OnPause += HandlePlayerPause;
        }

        private void OnDisable()
        {
            matchSettings.TimeLimit.OnFinished -= HandleMatchFinished;

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