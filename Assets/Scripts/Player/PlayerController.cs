using Triggers;
using UI;
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
        private ScoreConverterTrigger _scoreConverterTrigger;
        
        private float _directionInput;
        private bool _brakeInput;
        
        public void OnCreate(UIBoostBar boostBarUI, UISpeedBar speedBarUI, UIScore scoreUI) {
            var rb = GetComponent<Rigidbody>();
            _ground = GetComponent<PlayerGround>();
            
            _audio = GetComponent<PlayerAudio>();
            _audio.OnCreate(rb);
            
            _tricks = GetComponent<PlayerTricks>();
            _tricks.OnCreate(scoreUI, _audio);
            
            _jump = GetComponent<PlayerJump>();
            _jump.OnCreate(rb, _audio);
            
            _boost = GetComponent<PlayerBoost>();
            _boost.OnCreate(_audio, boostBarUI);
            
            _movement = GetComponent<PlayerMovement>();
            _movement.OnCreate(rb, _boost, _ground, _audio, speedBarUI);

            _scoreConverterTrigger = GetComponent<ScoreConverterTrigger>();
            _scoreConverterTrigger.OnCreate();
        }

        public void OnStart() {
            _audio.OnStart();
        }

        public void OnUpdate() {
            _tricks.OnUpdate(_ground.IsGrounded());
        }

        public void OnFixedUpdate() {
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