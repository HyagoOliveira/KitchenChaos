using UnityEngine;
using KitchenChaos.Matches;

namespace KitchenChaos.Players
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Player))]
    public sealed class PlayerMatchController : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private MatchSettings matchSettings;
        [SerializeField] private GameObject normalMovingParticles;
        [SerializeField] private GameObject fastMovingParticles;
        [SerializeField, Min(0)] private float countDownSpeed = 3F;
        [SerializeField, Min(0)] private float normalSpeed = 6F;
        [SerializeField, Min(0)] private float finalSpeed = 8F;

        private void Reset() => player = GetComponent<Player>();

        private void OnEnable()
        {
            matchSettings.CountDown.OnStarted += HandleCountDownStarted;
            matchSettings.CountDown.OnFinished += HandleCountDownFinished;
            matchSettings.TimeLimit.OnFinalSecondsStarted += HandleFinalSecondsStarted;
        }

        private void OnDisable()
        {
            matchSettings.CountDown.OnStarted -= HandleCountDownStarted;
            matchSettings.CountDown.OnFinished -= HandleCountDownFinished;
            matchSettings.TimeLimit.OnFinalSecondsStarted -= HandleFinalSecondsStarted;
        }

        private void HandleCountDownStarted()
        {
            player.Motor.moveSpeed = countDownSpeed;
            normalMovingParticles.SetActive(true);
            fastMovingParticles.SetActive(false);
        }

        private void HandleCountDownFinished() => player.Motor.moveSpeed = normalSpeed;

        private void HandleFinalSecondsStarted()
        {
            player.Motor.moveSpeed = finalSpeed;
            normalMovingParticles.SetActive(false);
            fastMovingParticles.SetActive(true);
        }
    }
}