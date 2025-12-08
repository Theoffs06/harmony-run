using UnityEngine;

namespace Player {
    public class PlayerGround : MonoBehaviour {
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.4f;
        [SerializeField] private LayerMask groundMask;
        
        public bool IsGrounded() => Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}