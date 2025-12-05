using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

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
    
    [Header("Brake")]
    [SerializeField] private float brakeStrength = 20f;

    [Header("UI")]
    [SerializeField] private Slider boostUI;

    [Header("Events Moteur passive")]
    [SerializeField] private EventReference moteurEvent;
    
    private Rigidbody _rb;

    private float _directionInput;
    private float _brakeInput;
    private bool _boostInput;
    private bool _forceRotation = true;
    
    private Vector3 _splineForward, _splineRight;
    private float3 _splineNearestPoint;

    private EventInstance _moteurInstance;
    
    public SplineContainer TrackSpline { set => trackSpline = value; }

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _moteurInstance = RuntimeManager.CreateInstance(moteurEvent);
    }

    private void Start() {
        _moteurInstance.start();
        RuntimeManager.AttachInstanceToGameObject(_moteurInstance, gameObject, _rb);
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
    
    public void OnMove(InputAction.CallbackContext obj) => _directionInput = obj.ReadValue<float>();
    public void OnBoost(InputAction.CallbackContext obj) =>_boostInput = obj.performed;
    public void OnBrake(InputAction.CallbackContext obj) => _brakeInput = obj.ReadValue<float>();
    
    private void Advance() {
        if (_forceRotation) _rb.MoveRotation(Quaternion.FromToRotation(Vector3.forward, _splineForward));
        
        var actualSpeed = speed;
        if(_boostInput && boostUI.value > 0f) {
            actualSpeed = speed * boostSpeedMultiplier;
            boostUI.value -= Time.fixedDeltaTime * boostConsumptionRate;
        }
        
        var verticalVelocity = _rb.linearVelocity.y;
        var horizontalVelocity = _splineForward * actualSpeed + _splineRight * (_directionInput * playerSteerSpeed);
        
        if (_brakeInput > 0f) horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, _brakeInput * brakeStrength * Time.fixedDeltaTime);
        
        _rb.linearVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
        RuntimeManager.StudioSystem.setParameterByName("Speed", actualSpeed*100/speed);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _splineForward);
        Gizmos.DrawLine(transform.position, _splineNearestPoint);
    }
}
