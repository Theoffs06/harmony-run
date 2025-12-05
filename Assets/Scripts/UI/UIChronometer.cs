using TMPro;
using UnityEngine;

namespace UI {
    public class UIChronometer : MonoBehaviour {
        private TMP_Text _chronometerTxt;

        private void Awake() {
            _chronometerTxt = GetComponent<TMP_Text>();
        }

        private void Update() {
            _chronometerTxt.SetText($"{Chronometer.Minutes:00}:{Chronometer.Seconds:00}:{Chronometer.Milliseconds:00}");
            Chronometer.Update(Time.deltaTime);
        }
    }
}