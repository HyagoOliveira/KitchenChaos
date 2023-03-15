using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static KitchenChaos.Players.PlayerInputActions;

namespace KitchenChaos.Players
{
    [CreateAssetMenu(fileName = "PlayerInputSettings", menuName = EditorPaths.SO + "PlayerInputSettings", order = 110)]
    public sealed class PlayerInputSettings : ScriptableObject
    {
        public event Action OnChop;
        public event Action OnDash;
        public event Action OnSwitch;
        public event Action OnPause;
        public event Action<Vector2> OnMove;
        public event Action OnCollectItem;
        public event Action OnInteractWithEnvironment;

        private PlayerActions actions;

        internal void Initialize()
        {
            var input = new PlayerInputActions();
            actions = input.Player;
        }

        public void Enable() => actions.Enable();
        public void Disable() => actions.Disable();

        internal void ResetAxis()
        {
            actions.Move.Disable();
            actions.Move.Enable();
        }

        internal void BindActions()
        {
            actions.Move.started += HandleMovePerformed;
            actions.Move.performed += HandleMovePerformed;
            actions.Move.canceled += HandleMovePerformed;

            actions.Chop.performed += HandleChopPerformed;
            actions.Dash.performed += HandleDashPerformed;
            actions.Switch.performed += HandleSwitchPerformed;
            actions.Pause.started += HandlePausePerformed;
            actions.CollectItem.performed += HandleCollectItemPerformed;
            actions.InteractWithEnvironment.performed += HandleInteractWithEnvironmentPerformed;
        }

        internal void UnBindActions()
        {
            actions.Move.started -= HandleMovePerformed;
            actions.Move.performed -= HandleMovePerformed;
            actions.Move.canceled -= HandleMovePerformed;

            actions.Chop.performed -= HandleChopPerformed;
            actions.Dash.performed -= HandleDashPerformed;
            actions.Switch.performed -= HandleSwitchPerformed;
            actions.Pause.started -= HandlePausePerformed;
            actions.CollectItem.performed -= HandleCollectItemPerformed;
            actions.InteractWithEnvironment.performed -= HandleInteractWithEnvironmentPerformed;
        }

        private void HandleChopPerformed(InputAction.CallbackContext _) => OnChop?.Invoke();
        private void HandleDashPerformed(InputAction.CallbackContext _) => OnDash?.Invoke();
        private void HandleSwitchPerformed(InputAction.CallbackContext _) => OnSwitch?.Invoke();
        private void HandlePausePerformed(InputAction.CallbackContext _) => OnPause?.Invoke();
        private void HandleCollectItemPerformed(InputAction.CallbackContext _) => OnCollectItem?.Invoke();
        private void HandleInteractWithEnvironmentPerformed(InputAction.CallbackContext _) => OnInteractWithEnvironment?.Invoke();

        private void HandleMovePerformed(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>();
            OnMove?.Invoke(input);
        }
    }
}