using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    [Header("Spline Driving")]
    [SerializeField] private SplineContainer trackSpline;
    [SerializeField] private float speed = 30f;

    [Header("Player Steering")]
    [SerializeField] private float playerSteerSpeed = 100f;

    [Header("Safety")]
    [SerializeField] private LayerMask safetyMask;

    private InputAction _directionInput;
    private Rigidbody _rb;

    private float _input;
    private bool _forceRotation = true;
    
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
    }

    private void OnEnable() => _directionInput.Enable();
    private void OnDisable() => _directionInput.Disable();

    private void OnCollisionEnter(Collision other) { 
        if (other.gameObject.layer == safetyMask) _forceRotation = false;
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.layer == safetyMask) _forceRotation = true;
    }

    private void HandlePlayerInput() {
        _input = _directionInput.ReadValue<float>();
    }

    private void Advance() {
        if (_forceRotation) _rb.MoveRotation(Quaternion.FromToRotation(Vector3.forward, _splineForward));
        
        var verticalVelocity = _rb.linearVelocity.y;
        var horizontalVelocity = _splineForward * speed + _splineRight * (_input * playerSteerSpeed);
        _rb.linearVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _splineForward);
        Gizmos.DrawLine(transform.position, _splineNearestPoint);
    }
}
