using UnityEngine;

namespace Triggers {
    public class EndTrigger : MonoBehaviour {
        private GameManager _manager;
        
        public void OnCreate(GameManager manager) {
            _manager = manager;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) _manager.OnGameOver();
        }
    }
}