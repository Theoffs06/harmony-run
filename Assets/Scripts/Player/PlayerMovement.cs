using UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Player {
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
        
        [Header("Brake")]
        [SerializeField] private float brakeStrength = 20f;

        [Header("UI")]
        [SerializeField] private UISpeedBar speedUI;
        
        private PlayerAudio _audio;
        private PlayerBoost _boost;
        private PlayerGround _ground;
        private Rigidbody _rb;
        
        private bool _forceRotation = true;
    
        private float3 _splineForward, _splineRight;
        private float3 _splineNearestPoint;
        private float3 _rightMaxSteerPoint;
        private float3 _leftMaxSteerPoint;
        
        public SplineContainer TrackSpline { set => trackSpline = value; }

        public void OnCreate(Rigidbody rb, PlayerBoost boost, PlayerGround jump, PlayerAudio audioManager) {
            _rb = rb;
            _boost = boost;
            _ground = jump;
            _audio = audioManager;
        }
        
        public void OnFixedUpdate() {
            SplineUtility.GetNearestPoint(trackSpline.Spline, transform.position, out _splineNearestPoint, out var t);
            _rightMaxSteerPoint = _splineNearestPoint + _splineRight * maxSteerLength;
            _leftMaxSteerPoint = _rightMaxSteerPoint +  -2 * maxSteerLength * _splineRight;
            _splineForward = math.normalize(trackSpline.Spline.EvaluateTangent(t));
            _splineRight = math.normalize(math.cross(new float3(0, 1, 0), _splineForward));
        }
    
        private void OnCollisionEnter(Collision other) { 
            if (other.gameObject.layer == safetyMask) _forceRotation = false;
        }

        private void OnCollisionExit(Collision other) {
            if (other.gameObject.layer == safetyMask) _forceRotation = true;
        }
        
        public void Advance(float directionInput, bool brakeInput) {
            if (_forceRotation) _rb.MoveRotation(Quaternion.FromToRotation(Vector3.forward, _splineForward));
        
            var actualSpeed = speed;
            if(_boost.TryConsumeBoost()) actualSpeed = speed * boostSpeedMultiplier;
            
            var steerVelocity = directionInput * playerSteerSpeed;
            if (math.distance(_leftMaxSteerPoint, transform.position) >= maxSteerLength * 2 && steerVelocity > 0 || math.distance(_rightMaxSteerPoint, transform.position) >= maxSteerLength * 2 && steerVelocity < 0) {
                steerVelocity = 0;
            }

            var horizontalVelocity = _splineForward * actualSpeed + _splineRight * steerVelocity;
        
            if (brakeInput && !_boost.IsBoosted && _ground.IsGrounded()) horizontalVelocity = math.lerp(horizontalVelocity, float3.zero, brakeStrength * Time.fixedDeltaTime);
        
            _rb.linearVelocity = new Vector3(horizontalVelocity.x, _rb.linearVelocity.y, horizontalVelocity.z);
            if(math.distance(_splineNearestPoint, transform.position) >= maxSteerLength * 5) transform.position = _splineNearestPoint;
            
            speedUI.UpdateSpeed(brakeInput ? 0 : actualSpeed,speed * boostSpeedMultiplier);
            _audio.SetSpeed(actualSpeed * 100 / speed);
        }
    
        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_rightMaxSteerPoint, _leftMaxSteerPoint);
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _splineForward);
            Gizmos.DrawLine(transform.position, _splineNearestPoint);
        }
    }
}
