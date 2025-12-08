using TMPro;
using UnityEngine;

namespace UI {
    public class UIScore : MonoBehaviour {
        [SerializeField] private TMP_Text scoreTxt;
        
        public int CurrentScore { get; private set; }
        
        public void IncreaseScore(int score) {
            CurrentScore += score;
            scoreTxt.SetText($"{CurrentScore:00000}");
        }

        public void ResetScore() {
            CurrentScore = 0;
            scoreTxt.SetText($"{CurrentScore:00000}");
        }
    }
}