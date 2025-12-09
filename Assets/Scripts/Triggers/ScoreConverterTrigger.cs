using Player;
using UnityEngine;

namespace Triggers {
    public class ScoreConverterTrigger : MonoBehaviour {
        private ScoreConverterManager _scoreConverterManager;

        public void OnCreate() {
            _scoreConverterManager = GetComponentInParent<ScoreConverterManager>();
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer != 8) return;
            _scoreConverterManager.TriggeredScoreConverter(gameObject.name == "Player 1" ? 0 : 1);
        }

    }
}
