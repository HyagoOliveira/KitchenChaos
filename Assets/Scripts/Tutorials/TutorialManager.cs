using UnityEngine;
using System.Collections;
using KitchenChaos.UI;
using KitchenChaos.Players;
using KitchenChaos.Matches;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class TutorialManager : MonoBehaviour
    {
        [field: SerializeField] public PlayerSettings PlayerSettings { get; private set; }
        [field: SerializeField] public PlayerInputSettings PlayerInputSettings { get; private set; }

        [SerializeField] private MatchSettings matchSettings;
        [SerializeField] private GameObject arrow;
        [SerializeField] private TutorialDescription description;
        [SerializeField, Min(0f)] private float timeToReadCompleteStep = 1f;
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

        internal void ShowArrow(Vector3 position)
        {
            arrow.transform.position = position;
            arrow.SetActive(true);
        }

        internal void HideArrow() => arrow.SetActive(false);

        internal void CompleteStep() => StartCoroutine(CompleteStepRoutine());

        private void GoToNextStep() => StartCoroutine(GoToNextStepRoutine());

        private void TryBeginNextStep()
        {
            if (++currentIndex >= TotalSteps)
            {
                CompleteTutorial();
                return;
            }

            description.SetUncompletedText(CurrentStep.GetDescription());
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
            description.Complete();

            yield return new WaitForSeconds(timeToReadCompleteStep);
            yield return description.FadeOutRoutine();
            yield return GoToNextStepRoutine();
        }

        private IEnumerator GoToNextStepRoutine()
        {
            yield return new WaitForSeconds(timeBetweenSteps);
            TryBeginNextStep();
        }
    }
}