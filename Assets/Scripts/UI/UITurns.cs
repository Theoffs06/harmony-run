using TMPro;
using UnityEngine;

namespace UI {
    public class UITurns : MonoBehaviour {
        private TMP_Text _turnsTxt;
        
        public void OnCreate() {
            _turnsTxt = GetComponent<TMP_Text>();
        }
        
        public void UpdateTurns(int turns, int maxTurns) {
            _turnsTxt.SetText($"{turns}/{maxTurns}");
        }
    }
}