using UnityEngine;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class SwitchPlayerTutorialStep : AbstractTutorialStep
    {
        internal override string GetDescription() =>
            $"Use {GetButtonName()} to switch between Players.";

        internal override void Begin() => manager.PlayerInputSettings.OnSwitch += HandlePlayerSwitch;

        private string GetButtonName() => SurrounButtonNameWithSpriteTag(PlayerInput.GetSwitchButtonSpriteName());

        private void HandlePlayerSwitch()
        {
            manager.PlayerInputSettings.OnSwitch -= HandlePlayerSwitch;
            Complete();
        }
    }
}