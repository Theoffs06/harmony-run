using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using FMODUnity;
using FMOD.Studio;
using UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    [Header("Spline Driving")]
    [SerializeField] private SplineContainer trackSpline;
    [SerializeField] private float speed = 30f;

    [Header("Player Steering")]
    [SerializeField] private float playerSteerSpeed = 100f;
    [SerializeField] private float maxSteerLength;

    [Header("Safety")]
    [SerializeField] private LayerMask safetyMask;
    
    [Header("Boost")]
    [SerializeField] private float boostSpeedMultiplier = 2.0f;
    [SerializeField] private float boostConsumptionRate = 0.01f;
    
    [Header("Brake")]
    [SerializeField] private float brakeStrength = 20f;

    [Header("UI")]
    [SerializeField] private UIBoostBar boostUI;
    [SerializeField] private UISpeedBar speedUI;

    [Header("Events Fmod")]
    [SerializeField] private EventReference moteurEvent;
    [SerializeField] private EventReference windEvent;
    [SerializeField] private EventReference boostStartEvent;
    [SerializeField] private EventReference boostEndEvent;

    [Header("Mute Sound")]
    [SerializeField] private bool muteSound;
    
    private PlayerJump _playerJump;
    private Rigidbody _rb;

    private float _directionInput;
    private bool _brakeInput;
    private bool _boostInput;
    private bool _forceRotation = true;
    
    private Vector3 _splineForward, _splineRight;
    private float3 _splineNearestPoint;
    private Vector3 _rightMaxSteerPoint;
    private Vector3 _leftMaxSteerPoint;

    private EventInstance _moteurInstance;
    private EventInstance _windInstance;
    
    public SplineContainer TrackSpline { set => trackSpline = value; }

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _playerJump = GetComponent<PlayerJump>();
        _moteurInstance = RuntimeManager.CreateInstance(moteurEvent);
        _windInstance = RuntimeManager.CreateInstance(windEvent);
    }

    private void Start() {
        if (muteSound) return;
        _moteurInstance.start();
        _windInstance.start();
        RuntimeManager.AttachInstanceToGameObject(_moteurInstance, gameObject, _rb);
        RuntimeManager.AttachInstanceToGameObject(_windInstance, gameObject, _rb);
    }
    
    private void FixedUpdate() {
        SplineUtility.GetNearestPoint(trackSpline.Spline, transform.position, out _splineNearestPoint, out var t);
        _rightMaxSteerPoint = (Vector3)_splineNearestPoint + _splineRight * maxSteerLength;
        _leftMaxSteerPoint = _rightMaxSteerPoint +  -2 * maxSteerLength * _splineRight;
        _splineForward = math.normalize(trackSpline.Spline.EvaluateTangent(t));
        _splineRight = math.normalize(math.cross(new float3(0, 1, 0), _splineForward));
        
        Advance();
        if(Vector3.Distance(_splineNearestPoint, transform.position) >= maxSteerLength * 5) {
            transform.position = _splineNearestPoint;
        }
    }
    
    private void OnCollisionEnter(Collision other) { 
        if (other.gameObject.layer == safetyMask) _forceRotation = false;
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.layer == safetyMask) _forceRotation = true;
    }
    
    public void OnMove(InputAction.CallbackContext obj) => _directionInput = obj.ReadValue<float>();
    public void OnBoost(InputAction.CallbackContext obj) {
        if (obj.performed) {
            _boostInput = true;
            
            if (muteSound) return;
            RuntimeManager.PlayOneShotAttached(boostStartEvent, gameObject); 
            _windInstance.setParameterByName("Boost", 1f);
            _moteurInstance.setParameterByName("Boost", 1f);

        }
        else if (obj.canceled) {
            _boostInput = false;
            
            if (muteSound) return;
            RuntimeManager.PlayOneShotAttached(boostEndEvent, gameObject);
            _windInstance.setParameterByName("Boost", 0f);
            _moteurInstance.setParameterByName("Boost", 0f);
        }
    }

    public void OnBrake(InputAction.CallbackContext obj) => _brakeInput = obj.ReadValue<float>() <= -1f;
    
    private void Advance() {
        if (_forceRotation) _rb.MoveRotation(Quaternion.FromToRotation(Vector3.forward, _splineForward));
        
        var actualSpeed = speed;
        if(_boostInput && boostUI.CurrentBoost > 0f) {
            actualSpeed = speed * boostSpeedMultiplier;
            boostUI.DecreaseBoost(Time.fixedDeltaTime * boostConsumptionRate);
        }
        
        var verticalVelocity = _rb.linearVelocity.y;
        var steerVelocity = _directionInput * playerSteerSpeed;
        if (Vector3.Distance(_leftMaxSteerPoint, transform.position) >= maxSteerLength * 2 && steerVelocity > 0 || Vector3.Distance(_rightMaxSteerPoint, transform.position) >= maxSteerLength * 2 && steerVelocity < 0) {
            steerVelocity = 0;
        }

        var horizontalVelocity = _splineForward * actualSpeed + _splineRight * steerVelocity;
        
        if (_brakeInput && !_boostInput && _playerJump.IsGrounded()) horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, brakeStrength * Time.fixedDeltaTime);
        
        _rb.linearVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
        speedUI.UpdateSpeed(actualSpeed, speed * boostSpeedMultiplier);
        
        if (!_moteurInstance.isValid() || !_windInstance.isValid()) return;
        _moteurInstance.setParameterByName("Speed", actualSpeed * 100 / speed);
        _windInstance.setParameterByName("Speed", actualSpeed * 100 / speed);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_rightMaxSteerPoint, _leftMaxSteerPoint);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _splineForward);
        Gizmos.DrawLine(transform.position, _splineNearestPoint);
    } 
}
