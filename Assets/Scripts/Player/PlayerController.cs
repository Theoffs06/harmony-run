using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerController : MonoBehaviour {
        private PlayerGround _ground;
        private PlayerMovement _movement;
        private PlayerBoost _boost;
        private PlayerJump _jump;
        private PlayerTricks _tricks;
        private PlayerAudio _audio;
        
        private float _directionInput;
        private bool _brakeInput;
        
        private void Awake() {
            var rb = GetComponent<Rigidbody>();
            _ground = GetComponent<PlayerGround>();
            
            _tricks = GetComponent<PlayerTricks>();
            _tricks.OnCreate();
            
            _audio = GetComponent<PlayerAudio>();
            _audio.OnCreate(rb);
            
            _jump = GetComponent<PlayerJump>();
            _jump.OnCreate(rb, _audio);
            
            _boost = GetComponent<PlayerBoost>();
            _boost.OnCreate(_audio);
            
            _movement = GetComponent<PlayerMovement>();
            _movement.OnCreate(rb, _boost, _ground, _audio);
        }

        private void Start() {
            _audio.OnStart();
        }

        private void Update() {
            _tricks.OnUpdate(_ground.IsGrounded());
        }

        private void FixedUpdate() {
            _movement.OnFixedUpdate();
            _movement.Advance(_directionInput, _brakeInput);
        }
        
        public void OnMove(InputAction.CallbackContext obj) => _directionInput = obj.ReadValue<float>();
        public void OnBrake(InputAction.CallbackContext obj) => _brakeInput = obj.ReadValue<float>() <= -1f;

        public void OnBoost(InputAction.CallbackContext obj) {
            if (obj.performed) _boost.StartBoost();
            else if (obj.canceled) _boost.StopBoost();
        }
        
        public void OnJump(InputAction.CallbackContext obj) {
            if (!_ground.IsGrounded() || !obj.performed) return;
            _jump.Jump();
        }
        
        public void OnTrick(InputAction.CallbackContext obj) {
            if (_ground.IsGrounded() || !obj.performed) return;
            _tricks.LaunchTrick();
        }
    }
}