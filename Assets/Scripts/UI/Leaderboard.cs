using TMPro;
using UnityEngine;

namespace UI {
    public class Leaderboard : MonoBehaviour {
        [SerializeField] private TMP_Text uiText;

        public void NewEntry(string player1Name, string player2Name) {
            uiText.SetText($"{uiText.text}\n{Chronometer.Minutes:00}:{Chronometer.Seconds:00}:{Chronometer.Milliseconds:00} {player1Name} | {player2Name}");
        }
    }
}
