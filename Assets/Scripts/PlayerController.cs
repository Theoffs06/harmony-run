using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    
    [Header("Player Parameters")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    
    private InputAction _horizontalInput;
    private InputAction _verticalInput;
    private InputAction _forwardInput;
    private InputAction _switchInput;
    
    private bool _isPlayer1 = true;

    private void Awake() {
        _horizontalInput = InputSystem.actions.FindAction("Horizontal");
        _verticalInput = InputSystem.actions.FindAction("Vertical");
        _forwardInput = InputSystem.actions.FindAction("Forward");
        _switchInput = InputSystem.actions.FindAction("Switch");
    }

    private void FixedUpdate() {
        var horizontal = _horizontalInput.ReadValue<float>();
        var vertical = _verticalInput.ReadValue<float>();
        var forward = _forwardInput.ReadValue<float>();
        
        var direction = new Vector3(horizontal, vertical, forward);
        var movement = direction.normalized * (speed * Time.fixedDeltaTime);

        if (_isPlayer1) {
            if (direction.sqrMagnitude > 0.001f)
                player1.rotation = Quaternion.Slerp(player1.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * rotationSpeed);
            
            player1.Translate(movement, Space.World);
        }
        else {
            if (direction.sqrMagnitude > 0.001f) 
                player2.rotation = Quaternion.Slerp(player2.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * rotationSpeed);
            
            player2.Translate(movement, Space.World);
        }
    }

    private void OnEnable() {
        _horizontalInput.Enable();
        _verticalInput.Enable();
        _forwardInput.Enable();
        
        _switchInput.Enable();
        _switchInput.performed += SwitchInputOnPerformed;
    }
    
    private void OnDisable() {
        _horizontalInput.Disable();
        _verticalInput.Disable();
        _forwardInput.Disable();
        
        _switchInput.Disable();
        _switchInput.performed -= SwitchInputOnPerformed;
    }

    private void SwitchInputOnPerformed(InputAction.CallbackContext obj) {
        _isPlayer1 = !_isPlayer1;
    }
}
