using UnityEngine;
using System.Collections;
using KitchenChaos.UI;
using KitchenChaos.Players;
using KitchenChaos.Matches;
using KitchenChaos.Counters;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class TutorialManager : MonoBehaviour
    {
        [field: SerializeField] public PlayerSettings PlayerSettings { get; private set; }
        [field: SerializeField] public PlayerInputSettings PlayerInputSettings { get; private set; }

        [SerializeField] private MatchSettings matchSettings;
        [SerializeField] private GameObject arrow;
        [SerializeField] private GameObject cheeseBurgerStepTitle;
        [SerializeField] private TutorialDescription description;
        [SerializeField, Min(0f)] private float timeBetweenSteps = 2f;
        [SerializeField] private AbstractTutorialStep[] steps;

        public int TotalSteps => steps.Length;
        public AbstractTutorialStep CurrentStep => steps[currentIndex];

        private int currentIndex = -1;

        private void Reset()
        {
            steps = GetComponents<AbstractTutorialStep>();
            description = GetComponentInChildren<TutorialDescription>();
            arrow = transform.Find("TutorialArrow").gameObject;
            cheeseBurgerStepTitle = transform.Find("CheeseBurgerStepTitle").gameObject;
        }

        private void Awake()
        {
            matchSettings.IsAllowToStartTimeLimit = false;

            foreach (var step in steps)
            {
                step.Initialize(this);
            }
        }

        //TODO apagar
        private void Start() => GoToNextStep();

        internal void PlaceArrowOverCounter(Counter counter)
        {
            const float height = 1.5F;
            var position = counter.transform.position + Vector3.up * height;

            PlaceArrow(position);
        }

        internal void PlaceArrowOverPreparator(AbstractIngredientPreparator preparator)
        {
            const float height = 0.5F;
            var position = preparator.transform.position + Vector3.up * height;

            PlaceArrow(position);
        }

        internal void PlaceArrow(Vector3 position, float verticalDistance = 0.2f)
        {
            arrow.transform.position = position + Vector3.up * verticalDistance;
            arrow.SetActive(true);
        }

        internal void HideArrow() => arrow.SetActive(false);

        internal void CompleteStep() => StartCoroutine(CompleteStepRoutine());

        internal void EnableCheeseBurgerTitle(bool enabled) => cheeseBurgerStepTitle.SetActive(enabled);

        internal void SetDescription(string text) => description.SetUncompletedText(text);

        internal IEnumerator CompleteDescriptionRoutine()
        {
            description.Complete();
            yield return description.FadeOutRoutine();
        }

        private void GoToNextStep() => StartCoroutine(GoToNextStepRoutine());

        private void TryBeginNextStep()
        {
            if (++currentIndex >= TotalSteps)
            {
                CompleteTutorial();
                return;
            }

            SetDescription(CurrentStep.GetDescription());
            CurrentStep.Begin();
        }

        private void CompleteTutorial()
        {
            matchSettings.IsAllowToStartTimeLimit = true;

            print("Tutorial is completed");
            Destroy(gameObject);
        }

        private IEnumerator CompleteStepRoutine()
        {
            yield return CompleteDescriptionRoutine();
            yield return GoToNextStepRoutine();
        }

        private IEnumerator GoToNextStepRoutine()
        {
            yield return new WaitForSeconds(timeBetweenSteps);
            TryBeginNextStep();
        }
    }
}