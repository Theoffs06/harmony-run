using UI;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerTricks : MonoBehaviour {
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    
    [SerializeField] private UIScore uiScore;
    
    private Animator _animator; 
    private PlayerSyncActions _playerSync;
    private PlayerJump _playerJump;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _playerSync = GetComponentInParent<PlayerSyncActions>();
        _playerJump = GetComponent<PlayerJump>();
    }

    private void Update() {
        _animator.SetBool(IsGrounded, _playerJump.IsGrounded());
    }

    public void OnTrick(InputAction.CallbackContext obj) {
        if (_playerJump.IsGrounded() || !obj.performed) return;
        _animator.Play("Kickflip");
    }

    public void FailedTrick() {
        uiScore.ResetScore();
    }

    public void SuceedTrick() {
        var playerId = gameObject.name == "Player 1" ? 0 : 1;
        uiScore.IncreaseScore(_playerSync.SucedTricks(playerId));
    }
}
