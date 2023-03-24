using UnityEngine;
using System.Collections;
using KitchenChaos.UI;
using KitchenChaos.Items;
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
        [SerializeField] private GameObject cheeseBurgerStepTitle;
        [SerializeField] private TutorialDescription description;
        [SerializeField, Min(0f)] private float timeBetweenSteps = 0.4f;
        [SerializeField] private GameObject[] arrows;
        [SerializeField] private AbstractTutorialStep[] steps;

        public int TotalSteps => steps.Length;
        public AbstractTutorialStep CurrentStep => steps[currentIndex];

        public Stove Stove { get; private set; }
        public Plate Plate { get; private set; }
        public Counter[] Counters { get; private set; }
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
            cheeseBurgerStepTitle = transform.Find("TutorialManagerCanvas/CheeseBurgerStepTitle").gameObject;

            arrows = new GameObject[2]
            {
                transform.Find("TutorialArrow_00").gameObject,
                transform.Find("TutorialArrow_01").gameObject,
            };
        }

        private void Awake()
        {
            FindItems();
            DisableItems();
            // Disabling all counter so Player can only
            // interact with the current tutorial Counter.
            EnableCounters(false);

            foreach (var step in steps)
            {
                step.Initialize(this);
            }
        }

        private void OnEnable() => matchSettings.CountDown.OnFinished += HandleCountDownFinished;
        private void OnDisable() => matchSettings.CountDown.OnFinished -= HandleCountDownFinished;

        internal void PlaceArrowOverCounter(Counter counter, int index = 0)
        {
            const float height = 1.5F;
            var position = counter.transform.position + Vector3.up * height;

            PlaceArrow(position, index);
        }

        internal void PlaceArrowOverDeliveryCounter(int index = 0)
        {
            const float height = 1.5F;
            var position = DeliveryCounter.transform.position + Vector3.up * height;

            PlaceArrow(position, index);
        }

        internal void PlaceArrowOverPreparator(AbstractIngredientPreparator preparator, int index = 0)
        {
            const float height = 0.5F;
            var position = preparator.transform.position + Vector3.up * height;

            PlaceArrow(position, index);
        }

        internal void PlaceArrowOverPlate(int index = 0)
        {
            const float height = 0.5F;
            var position = Plate.transform.position + Vector3.up * height;

            PlaceArrow(position, index);
        }

        internal void PlaceArrow(Vector3 position, int index)
        {
            arrows[index].transform.position = position;
            arrows[index].SetActive(true);
        }

        internal void HideArrow(int index = 0) => arrows[index].SetActive(false);

        internal void CompleteStep() => StartCoroutine(CompleteStepRoutine());

        internal void EnableCheeseBurgerTitle(bool enabled) => cheeseBurgerStepTitle.SetActive(enabled);

        internal void SetDescription(string text) => description.SetUncompletedText(text);

        internal void EnableCounters(bool enabled)
        {
            foreach (var counter in Counters)
            {
                counter.IsEnabled = enabled;
            }
        }

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

            Counters = FindObjectsByType<Counter>(FindObjectsSortMode.None);

            MeatCounter = TutorialIngredientCounters.FindCounter(IngredientName.Meat);
            BreadCounter = TutorialIngredientCounters.FindCounter(IngredientName.Bread);
            CheeseCounter = TutorialIngredientCounters.FindCounter(IngredientName.Cheese);
        }

        private void DisableItems()
        {
            Plate.IsEnabled = false;
            DeliveryCounter.IsEnabled = false;
            matchSettings.IsAllowToStartTimeLimit = false;
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
            EnableCounters(true);
            Destroy(gameObject);
        }

        private void HandleCountDownFinished() => GoToNextStep();

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