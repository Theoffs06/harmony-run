using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    [Header("Spline Driving")]
    [SerializeField] private SplineContainer trackSpline;
    [SerializeField] private float speed = 18f;
    [SerializeField] private float turnStrength = 5f;
    [SerializeField] private float maxSteerAngle = 45f;

    [Header("Player Steering")]
    [SerializeField] private float playerSteerRange = 2f;
    [SerializeField] private float playerSteerSpeed = 4f;

    [Header("Lane Keeping")]
    [SerializeField] private float centerOffset;
    [SerializeField] private float laneCorrectionStrength = 10f;

    [Header("Safety")]
    [SerializeField] private float wallAvoidDistance = 2f;
    [SerializeField] private float wallAvoidStrength = 20f;
    [SerializeField] private LayerMask wallMask;

    private InputAction _directionInput;
    private float _desiredOffset;
    private Rigidbody _rb;

    private void Awake() {
        _directionInput = InputSystem.actions.FindAction("Direction");
        _rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        HandlePlayerInput();
    }

    private void FixedUpdate() {
        ApplyLaneOffset();
        FollowSpline();
        AvoidWalls();
        MaintainSpeed();
    }

    private void OnEnable() => _directionInput.Enable();
    private void OnDisable() => _directionInput.Disable();

    private void HandlePlayerInput() {
        var input = _directionInput.ReadValue<float>();
        
        _desiredOffset += input * playerSteerSpeed * Time.deltaTime;
        _desiredOffset = math.clamp(_desiredOffset, -playerSteerRange, playerSteerRange);
    }

    private void ApplyLaneOffset() {
        centerOffset = math.lerp(centerOffset, _desiredOffset, Time.deltaTime * playerSteerSpeed);
    }

    private void FollowSpline() {
        SplineUtility.GetNearestPoint(trackSpline.Spline, transform.position, out var nearestPos, out var t);
        var splineForward = math.normalize(trackSpline.Spline.EvaluateTangent(t));
        var splineRight = math.normalize(math.cross(new float3(0, 1, 0), splineForward));
        
        var angleDiff = SignedAngle(transform.forward, splineForward, new float3(0, 1, 0));
        var steer = math.clamp(angleDiff * turnStrength, -maxSteerAngle, maxSteerAngle);
        _rb.MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y + steer * Time.fixedDeltaTime, 0));

        var offsetPos = nearestPos + splineRight * centerOffset;
        var toTarget = offsetPos - (float3) transform.position;
        _rb.AddForce(toTarget * laneCorrectionStrength, ForceMode.Acceleration);
    }

    private void AvoidWalls() {
        var left = transform.position - transform.right * 0.5f;
        var right = transform.position + transform.right * 0.5f;

        if (Physics.Raycast(left, -transform.right, wallAvoidDistance, wallMask)) _rb.AddForce(transform.right * wallAvoidStrength, ForceMode.Acceleration);
        if (Physics.Raycast(right, transform.right, wallAvoidDistance, wallMask)) _rb.AddForce(-transform.right * wallAvoidStrength, ForceMode.Acceleration);
    }

    private void MaintainSpeed() {
        var horizSpeed = math.length(new float3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z));
        if (horizSpeed < speed) _rb.AddForce(transform.forward * 20f, ForceMode.Acceleration);
    }
    
    private static float SignedAngle(float3 from, float3 to, float3 axis) {
        return math.degrees(math.atan2(math.dot( math.cross(from, to), axis), math.dot(from, to)));
    }
}
