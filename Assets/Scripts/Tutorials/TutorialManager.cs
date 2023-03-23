using UnityEngine;
using System.Collections;
using KitchenChaos.UI;
using KitchenChaos.Players;
using KitchenChaos.Matches;
using KitchenChaos.Counters;
using KitchenChaos.Items;

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

        public Stove Stove { get; private set; }
        public Plate Plate { get; private set; }
        public CuttingBoard CuttingBoard { get; private set; }
        public IngredientCounter MeatCounter { get; private set; }
        public IngredientCounter BreadCounter { get; private set; }
        public IngredientCounter CheeseCounter { get; private set; }
        public DeliveryCounter DeliveryCounter { get; private set; }

        private int currentIndex = -1;

        private void Reset()
        {
            steps = GetComponents<AbstractTutorialStep>();
            description = GetComponentInChildren<TutorialDescription>();
            arrow = transform.Find("TutorialArrow").gameObject;
            cheeseBurgerStepTitle = transform.Find("TutorialManagerCanvas/CheeseBurgerStepTitle").gameObject;
        }

        private void Awake()
        {
            matchSettings.IsAllowToStartTimeLimit = false;

            foreach (var step in steps)
            {
                step.Initialize(this);
            }

            FindItems();
            DisableItems();
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

        internal void PlaceArrowOverPlate() => PlaceArrow(Plate.transform.position, 0.5F);

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

        private void FindItems()
        {
            Stove = FindObjectOfType<Stove>();
            Plate = FindObjectOfType<Plate>();
            CuttingBoard = FindObjectOfType<CuttingBoard>();
            DeliveryCounter = FindObjectOfType<DeliveryCounter>();

            MeatCounter = TutorialIngredientCounters.FindCounter(IngredientName.Meat);
            BreadCounter = TutorialIngredientCounters.FindCounter(IngredientName.Bread);
            CheeseCounter = TutorialIngredientCounters.FindCounter(IngredientName.Cheese);
        }

        private void DisableItems()
        {
            Plate.IsEnabled = false;
            DeliveryCounter.IsEnabled = false;
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