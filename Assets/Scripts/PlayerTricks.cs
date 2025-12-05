using UI;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerTricks : MonoBehaviour {
    [SerializeField] private PlayerSyncActions playerSync;
    [SerializeField] private UIScore uiScore;
    
    private Animator _animator;
    private PlayerJump _playerJump;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _playerJump = GetComponent<PlayerJump>();
    }

    private void Update() {
        _animator.SetBool("IsGrounded", _playerJump.IsGrounded());
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
        uiScore.IncreaseScore(playerSync.SucedTricks(playerId));
    }
}
