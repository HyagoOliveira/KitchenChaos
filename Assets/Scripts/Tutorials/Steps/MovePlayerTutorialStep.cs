using UnityEngine;
using System.Collections;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class MovePlayerTutorialStep : AbstractTutorialStep
    {
        [SerializeField, Min(0f)] private float movingTime = 5f;

        internal override string GetDescription() =>
            $"Use {GetButtonName()} to move the Player.";

        internal override void Begin() => StartCoroutine(CheckMovingTimeRoutine());

        private string GetButtonName() => SurrounButtonNameWithSpriteTag(PlayerInput.GetMoveButtonSpriteName());

        private IEnumerator CheckMovingTimeRoutine()
        {
            var currentMovingTime = 0f;

            do
            {
                yield return null;
                if (CurrentPlayer.Motor.IsMoving()) currentMovingTime += Time.deltaTime;
            } while (currentMovingTime < movingTime);

            Complete();
        }
    }
}