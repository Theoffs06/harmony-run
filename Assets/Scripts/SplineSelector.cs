using UnityEngine;
using UnityEngine.Splines;

public class SplineSelector : MonoBehaviour
{
    [SerializeField]
    private SplineContainer trackSplineSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.TrackSpline = trackSplineSelected;
        }
    }
}
