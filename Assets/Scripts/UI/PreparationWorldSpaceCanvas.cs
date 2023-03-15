using UnityEngine;
using KitchenChaos.Counters;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class PreparationWorldSpaceCanvas : MonoBehaviour
    {
        [SerializeField] private PreparationTimer timer;
        [SerializeField] private Animation animation;
        [SerializeField] private AudioSource audioSource;

        private AbstractIngredientPreparator preparator;

        private void Reset()
        {
            animation = GetComponent<Animation>();
            audioSource = GetComponent<AudioSource>();
            timer = GetComponentInChildren<PreparationTimer>();
        }

        private void Awake() => preparator = GetComponentInParent<AbstractIngredientPreparator>();

        private void Start() => timer.Hide();

        private void OnEnable()
        {
            preparator.OnPreparationStarted += HandlePreparationStarted;
            preparator.OnPreparationUpdated += HandlePreparationUpdated;
            preparator.OnPreparationCompleted += HandlePreparationCompleted;
            preparator.OnIngredientRejected += HandleIngredientRejected;
        }

        private void OnDisable()
        {
            preparator.OnPreparationStarted -= HandlePreparationStarted;
            preparator.OnPreparationUpdated -= HandlePreparationUpdated;
            preparator.OnPreparationCompleted -= HandlePreparationCompleted;
            preparator.OnIngredientRejected -= HandleIngredientRejected;
        }

        private void HandlePreparationStarted() => timer.Show();
        private void HandlePreparationUpdated(float progress) => timer.Progress = progress;
        private void HandlePreparationCompleted() => timer.Hide();

        private void HandleIngredientRejected()
        {
            animation.Play();
            audioSource.Play();
        }
    }
}