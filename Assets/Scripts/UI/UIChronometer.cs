using TMPro;
using UnityEngine;

namespace UI {
    public class UIChronometer : MonoBehaviour {
        private TMP_Text _chronometerTxt;

        public void OnCreate() {
            _chronometerTxt = GetComponent<TMP_Text>();
        }

        public void OnUpdate() {
            _chronometerTxt.SetText($"{Chronometer.Minutes:00}:{Chronometer.Seconds:00}:{Chronometer.Milliseconds:00}");
        }
    }
}