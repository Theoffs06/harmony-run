using UI;
using UnityEngine;

namespace Triggers {
    public class EndTrigger : MonoBehaviour {

        [SerializeField] private int maxTurns = 3;

        private int _currentTurns = 1;
        private int _playersFinishedThisLap = 0;

        private GameManager _manager;
        private UITurns _uiTurns;

        public void OnCreate(GameManager manager, UITurns uiTurns) {
            _manager = manager;
            _uiTurns = uiTurns;

            _uiTurns.UpdateTurns(_currentTurns, maxTurns);
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;
            _playersFinishedThisLap++;
            if (_playersFinishedThisLap < 2) return;
            
            _currentTurns++;
            _playersFinishedThisLap = 0; 

            _uiTurns.UpdateTurns(_currentTurns, maxTurns);
            if (_currentTurns >= maxTurns) {
                _manager.OnGameOver();
            }
        }
    }
}