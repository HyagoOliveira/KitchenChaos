using UnityEngine;
using System.Collections;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class MovePlayerTutorialStep : AbstractTutorialStep
    {
        [SerializeField, Min(0f)] private float movingTime = 5f;

        internal override string GetDescription() =>
            "Use the Gamepad Left Stick, AWSD or Arrows keys to move the Player.";

        internal override void Begin() => StartCoroutine(CheckMovingTimeRoutine());

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