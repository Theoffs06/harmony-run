using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerTricks : MonoBehaviour {
    private Animator _animator;
    private PlayerJump _playerJump;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _playerJump = GetComponent<PlayerJump>();
    }

    private void Update() {
        if (!_playerJump.IsGrounded()) return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Kickflip")) {
            Debug.Log("Looser");
        }
    }

    public void OnTrick(InputAction.CallbackContext obj) {
        if (_playerJump.IsGrounded() || !obj.performed) return;
        _animator.Play("Kickflip");
    }
}
