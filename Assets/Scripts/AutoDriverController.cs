using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    [Header("Spline Driving")]
    [SerializeField] private SplineContainer trackSpline;
    [SerializeField] private float speed = 30f;

    [Header("Player Steering")]
    [SerializeField] private float playerSteerSpeed = 100f;

    [Header("Safety")]
    [SerializeField] private float wallAvoidDistance = 2f;
    [SerializeField] private float wallAvoidStrength = 20f;
    [SerializeField] private LayerMask wallMask;

    private InputAction _directionInput;
    private Rigidbody _rb;

    private Vector3 _splineForward, _splineRight;
    private float3 _splineNearestPoint;

    private void Awake() {
        _directionInput = InputSystem.actions.FindAction("Direction");
        _rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        HandlePlayerInput();
    }

    private void FixedUpdate() {
        SplineUtility.GetNearestPoint(trackSpline.Spline, transform.position, out _splineNearestPoint, out var t);
        _splineForward = math.normalize(trackSpline.Spline.EvaluateTangent(t));
        _splineRight = math.normalize(math.cross(new float3(0, 1, 0), _splineForward));

        Advance();
        AvoidWalls();
    }

    private void OnEnable() => _directionInput.Enable();
    private void OnDisable() => _directionInput.Disable();

    private void HandlePlayerInput() {
        var input = _directionInput.ReadValue<float>();
    }

    private void Advance()
    {
        _rb.MoveRotation(Quaternion.FromToRotation(Vector3.forward, _splineForward));
        _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity , _splineForward * speed + _splineRight* _directionInput.ReadValue<float>() * playerSteerSpeed, 0.5f);
    }

    private void AvoidWalls() { // Obsolete
        var left = transform.position - transform.right * 0.5f;
        var right = transform.position + transform.right * 0.5f;

        if (Physics.Raycast(left, -transform.right, wallAvoidDistance, wallMask)) _rb.AddForce(transform.right * wallAvoidStrength, ForceMode.Acceleration);
        if (Physics.Raycast(right, transform.right, wallAvoidDistance, wallMask)) _rb.AddForce(-transform.right * wallAvoidStrength, ForceMode.Acceleration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _splineForward);
        Gizmos.DrawLine(transform.position, _splineNearestPoint);
    }

}
