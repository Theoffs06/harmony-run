using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private TMP_Text UIText;

    public void NewEntry(string player1Name, string player2Name)
    {
        UIText.SetText($"{UIText.text}\n{Chronometer.Minutes:00}:{Chronometer.Seconds:00}:{Chronometer.Milliseconds:00} {player1Name} | {player2Name}");
    }
}
