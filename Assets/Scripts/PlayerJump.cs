using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class PlayerJump : MonoBehaviour {
    public event Action<PlayerJump> OnJumped;
    
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Events Jump")]
    [SerializeField] private EventReference jumpEvent;
    
    private Rigidbody _rb;
    
    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    
    public void OnJump(InputAction.CallbackContext obj) {
        if (!IsGrounded() || !obj.performed) return;
        OnJumped?.Invoke(this);
        Jump();
    }

    public void Jump() {
        var jumpVelocity = math.sqrt(2 * Physics.gravity.magnitude * jumpHeight);
        _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.VelocityChange);
        RuntimeManager.PlayOneShotAttached(jumpEvent, gameObject);
    }
    
    public bool IsGrounded() => Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
}