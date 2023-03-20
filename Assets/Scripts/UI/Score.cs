using TMPro;
using UnityEngine;
using KitchenChaos.Score;
using KitchenChaos.Matches;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(Canvas))]
    public sealed class Score : MonoBehaviour
    {
        [SerializeField] private ScoreSettings settings;
        [SerializeField] private MatchSettings matchSettings;
        [SerializeField] private Animation animation;
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text current;
        [SerializeField] private TMP_Text delta;
        [SerializeField] private Color increasedColor = Color.green;
        [SerializeField] private Color decreasedColor = Color.red;

        private void Reset()
        {
            animation = GetComponent<Animation>();
            canvas = GetComponent<Canvas>();
        }

        private void Awake() => current.text = "0";

        private void OnEnable()
        {
            settings.OnScoreIncreased += HandleScoreIncreased;
            settings.OnScoreDecreased += HandleScoreDecreased;

            matchSettings.CountDown.OnFinished += HandleCountDownFinished;
        }

        private void OnDisable()
        {
            settings.OnScoreIncreased -= HandleScoreIncreased;
            settings.OnScoreDecreased -= HandleScoreDecreased;

            matchSettings.CountDown.OnFinished -= HandleCountDownFinished;
        }

        private void HandleScoreIncreased(float deltaScore)
        {
            delta.color = increasedColor;
            ShowDelta("+ " + deltaScore);
        }

        private void HandleScoreDecreased(float deltaScore)
        {
            delta.color = decreasedColor;
            ShowDelta("- " + deltaScore);
        }

        private void HandleCountDownFinished() => canvas.enabled = true;

        private void ShowDelta(string deltaScore)
        {
            delta.text = deltaScore;
            current.text = settings.Score.ToString();
            animation.Play();
        }
    }
}