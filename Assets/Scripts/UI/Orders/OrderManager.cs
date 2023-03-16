using UnityEngine;
using ActionCode.Audio;
using KitchenChaos.Items;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class OrderManager : MonoBehaviour
    {
        [SerializeField] private OrderSettings settings;
        [SerializeField] private IngredientSettings ingredientSettings;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSourceDictionary failedSource;
        [SerializeField] private AudioSourceDictionary deliveredSource;

        private void Reset() => audioSource = GetComponent<AudioSource>();
        private void Awake() => settings.Initialize();

        [ContextMenu("Create")]
        public void Create() => settings.CreateRandom(transform);

        [ContextMenu("CreateBurger")]
        public void CreateBurger() => settings.CreateBurger(transform);

        private void OnEnable()
        {
            settings.OnOrderFailed += HandleOrderFailed;
            settings.OnOrderCreated += HandleOrderCreated;
            settings.OnOrderDelivered += HandleOrderDelivered;
        }

        private void OnDisable()
        {
            settings.OnOrderFailed -= HandleOrderFailed;
            settings.OnOrderCreated -= HandleOrderCreated;
            settings.OnOrderDelivered -= HandleOrderDelivered;
        }

        private void HandleOrderFailed(Order _) => PlayFailedSound();
        private void HandleOrderCreated(Order _) => PlayCreatedSound();
        private void HandleOrderDelivered(Order _) => PlayDeliveredSound();

        private void PlayCreatedSound() => audioSource.Play();
        private void PlayFailedSound() => failedSource.PlayRandom();
        private void PlayDeliveredSound() => deliveredSource.PlayRandom();
    }
}