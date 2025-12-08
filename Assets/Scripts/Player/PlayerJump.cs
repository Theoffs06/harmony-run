using System;
using Unity.Mathematics;
using UnityEngine;

namespace Player {
    public class PlayerJump : MonoBehaviour {
        public event Action<PlayerJump> OnJumped;
    
        [SerializeField] private float jumpHeight = 10;
        
        private Rigidbody _rb;
        private PlayerAudio _audio;

        public void OnCreate(Rigidbody rb, PlayerAudio audioManager) {
            _rb = rb;
            _audio = audioManager;
        }
        
        public void Jump(bool isInputPressed = false) {
            if (isInputPressed) OnJumped?.Invoke(this);
            
            var jumpVelocity = math.sqrt(2 * Physics.gravity.magnitude * jumpHeight);
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.VelocityChange);
            _audio.OnJump();
        }
    }
}