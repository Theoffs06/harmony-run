using Player;
using UnityEngine;
using UnityEngine.Splines;

public class SplineSelector : MonoBehaviour {
    [SerializeField] private SplineContainer trackSplineSelected;

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<PlayerMovement>(out var player)) {
            player.TrackSpline = trackSplineSelected;
        }
    }
}
