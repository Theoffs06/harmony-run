using Player;
using UnityEngine;
using UnityEngine.Splines;

namespace Triggers {
    public class SplineSelectorTrigger : MonoBehaviour {
        [SerializeField] private SplineContainer trackSplineSelected;

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<PlayerMovement>(out var player)) {
                player.TrackSpline = trackSplineSelected;
            }
        }
    }
}
