using UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Spline Driving")]
        [SerializeField]
        private SplineContainer trackSpline;

        [SerializeField]
        private float speed = 30f;

        [Header("Player Steering")]
        [SerializeField]
        private float playerSteerSpeed = 100f;

        [SerializeField]
        private float maxSteerLength;

        [Header("Safety")]
        [SerializeField]
        private LayerMask safetyMask;

        [Header("Boost")]
        [SerializeField]
        private float boostSpeedMultiplier = 2.0f;

        [SerializeField]
        private float proximityBoostMultiplier = 1.0f;

        [Header("Brake")]
        [SerializeField]
        private float brakeStrength = 20f;

        private PlayerAudio _audio;
        private PlayerBoost _boost;
        private PlayerGround _ground;
        private Rigidbody _rb;
        private ProximitySpeedBoost _proximitySpeedBoost;
        
        private bool _forceRotation = true;

        private float3 _splineForward,
            _splineRight;
        private float3 _splineNearestPoint;
        private float3 _rightMaxSteerPoint;
        private float3 _leftMaxSteerPoint;

        public SplineContainer TrackSpline
        {
            set => trackSpline = value;
        }

        public void OnCreate(
            Rigidbody rb,
            PlayerBoost boost,
            PlayerGround jump,
            PlayerAudio audioManager
        )
        {
            _rb = rb;
            _boost = boost;
            _ground = jump;
            _audio = audioManager;
            _proximitySpeedBoost = GetComponent<ProximitySpeedBoost>();
        }

        public void OnFixedUpdate()
        {
            if (!trackSpline || trackSpline.Spline == null)
                return;

            SplineUtility.GetNearestPoint(
                trackSpline.Spline,
                transform.position,
                out _splineNearestPoint,
                out var t
            );

            // Compute frame from spline first
            _splineForward = math.normalize(trackSpline.Spline.EvaluateTangent(t));
            _splineRight = math.normalize(math.cross(new float3(0, 1, 0), _splineForward));

            // Then compute steering boundaries using the freshly updated right vector
            _rightMaxSteerPoint = _splineNearestPoint + _splineRight * maxSteerLength;
            _leftMaxSteerPoint = _rightMaxSteerPoint + -2 * maxSteerLength * _splineRight;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (((1 << other.gameObject.layer) & safetyMask.value) != 0)
                _forceRotation = false;
        }

        private void OnCollisionExit(Collision other)
        {
            if (((1 << other.gameObject.layer) & safetyMask.value) != 0)
                _forceRotation = true;
        }

        public void Advance(float directionInput, bool brakeInput)
        {
            if (_forceRotation)
                _rb.MoveRotation(Quaternion.FromToRotation(Vector3.forward, _splineForward));

            var actualSpeed = GetEffectiveSpeed();

            var steerVelocity = directionInput * playerSteerSpeed;
            if (
                math.distance(_leftMaxSteerPoint, transform.position) >= maxSteerLength * 2
                    && steerVelocity > 0
                || math.distance(_rightMaxSteerPoint, transform.position) >= maxSteerLength * 2
                    && steerVelocity < 0
            )
            {
                steerVelocity = 0;
            }

            var horizontalVelocity = _splineForward * actualSpeed + _splineRight * steerVelocity;
            if (brakeInput && !_boost.IsBoosted && _ground.IsGrounded())
                horizontalVelocity = math.lerp(
                    horizontalVelocity,
                    float3.zero,
                    brakeStrength * Time.fixedDeltaTime
                );

            var currentVel = _rb.linearVelocity;
            _rb.linearVelocity = new Vector3(horizontalVelocity.x, currentVel.y, horizontalVelocity.z);
            if(math.distance(_splineNearestPoint, transform.position) >= maxSteerLength * 5) transform.position = _splineNearestPoint;
            _audio.SetSpeed(brakeInput ? 0 : actualSpeed * 100 / speed * boostSpeedMultiplier * (2 * proximityBoostMultiplier));
        }

        private float GetEffectiveSpeed()
        {
            var current = speed;

            if (_boost && _boost.TryConsumeBoost())
                current *= boostSpeedMultiplier;
            current *= math.clamp(_proximitySpeedBoost.ProximityMultiplier() * proximityBoostMultiplier, 1, 2);
            return current;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_rightMaxSteerPoint, _leftMaxSteerPoint);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _splineForward);
            Gizmos.DrawLine(transform.position, _splineNearestPoint);
        }
    }
}
