using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    [Header("Spline Driving")]
    [SerializeField] private SplineContainer trackSpline;
    [SerializeField] private float speed = 30f;

    [Header("Player Steering")]
    [SerializeField] private float playerSteerSpeed = 100f;

    [Header("Safety")]
    [SerializeField] private LayerMask safetyMask;
    
    [Header("Boost")]
    [SerializeField] private float boostSpeedMultiplier = 2.0f;
    [SerializeField] private float boostConsumptionRate = 0.01f;

    [Header("UI")]
    [SerializeField] private Slider boostUI;
    
    private Rigidbody _rb;

    private float _input;
    private bool _boostInput;
    private bool _forceRotation = true;
    
    private Vector3 _splineForward, _splineRight;
    private float3 _splineNearestPoint;
    
    public SplineContainer TrackSpline { set => trackSpline = value; }

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate() {
        SplineUtility.GetNearestPoint(trackSpline.Spline, transform.position, out _splineNearestPoint, out var t);
        _splineForward = math.normalize(trackSpline.Spline.EvaluateTangent(t));
        _splineRight = math.normalize(math.cross(new float3(0, 1, 0), _splineForward));
        
        Advance();
    }
    
    private void OnCollisionEnter(Collision other) { 
        if (other.gameObject.layer == safetyMask) _forceRotation = false;
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.layer == safetyMask) _forceRotation = true;
    }
    
    public void OnMove(InputAction.CallbackContext obj) => _input = obj.ReadValue<float>();
    public void OnBoost(InputAction.CallbackContext obj) =>_boostInput = obj.performed;
    
    private void Advance() {
        if (_forceRotation) _rb.MoveRotation(Quaternion.FromToRotation(Vector3.forward, _splineForward));
        
        var actualSpeed = speed;
        if(_boostInput && boostUI.value > 0f) {
            actualSpeed = speed * boostSpeedMultiplier;
            boostUI.value -= Time.fixedDeltaTime * boostConsumptionRate;
        }
        
        var verticalVelocity = _rb.linearVelocity.y;
        var horizontalVelocity = _splineForward * actualSpeed + _splineRight * (_input * playerSteerSpeed);
        _rb.linearVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _splineForward);
        Gizmos.DrawLine(transform.position, _splineNearestPoint);
    }
}
