using UnityEngine;
using ActionCode.Audio;
using KitchenChaos.Items;
using KitchenChaos.Matches;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class OrderManager : MonoBehaviour
    {
        [SerializeField] private OrderSettings settings;
        [SerializeField] private MatchSettings matchSettings;
        [SerializeField] private IngredientSettings ingredientSettings;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSourceDictionary failedSource;
        [SerializeField] private AudioSourceDictionary deliveredSource;

        private void Reset() => audioSource = GetComponent<AudioSource>();
        private void Awake() => settings.Initialize(this);

        private void OnEnable()
        {
            settings.OnOrderFailed += HandleOrderFailed;
            settings.OnOrderCreated += HandleOrderCreated;
            settings.OnOrderDelivered += HandleOrderDelivered;

            matchSettings.OnFinished += HandleMatchFinished;
            matchSettings.CountDown.OnFinished += HandleCountDownFinished;
        }

        private void OnDisable()
        {
            settings.OnOrderFailed -= HandleOrderFailed;
            settings.OnOrderCreated -= HandleOrderCreated;
            settings.OnOrderDelivered -= HandleOrderDelivered;

            matchSettings.OnFinished -= HandleMatchFinished;
            matchSettings.CountDown.OnFinished -= HandleCountDownFinished;
        }

        public void StartOrderingTutorial(OrderSettings settings)
        {
            this.settings = settings;

            gameObject.SetActive(true);

            this.settings.StartOrdering();
        }

        private void HandleOrderFailed(Order _) => PlayFailedSound();
        private void HandleOrderCreated(Order _) => PlayCreatedSound();
        private void HandleOrderDelivered(Order _) => PlayDeliveredSound();

        private void HandleMatchFinished() => settings.StopOrdering();
        private void HandleCountDownFinished() => settings.StartOrdering();

        private void PlayCreatedSound() => audioSource.Play();
        private void PlayFailedSound() => failedSource.PlayRandom();
        private void PlayDeliveredSound() => deliveredSource.PlayRandom();
    }
}