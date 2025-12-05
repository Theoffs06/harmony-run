using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerTricks : MonoBehaviour {

    [SerializeField] private PlayerSyncActions playerSync;

    private Animator _animator;
    private PlayerJump _playerJump;
    private PlayerMovement _playerMovement;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _playerJump = GetComponent<PlayerJump>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        _animator.SetBool("IsGrounded", _playerJump.IsGrounded());
    }

    public void OnTrick(InputAction.CallbackContext obj) {
        if (_playerJump.IsGrounded() || !obj.performed) return;
        _animator.Play("Kickflip");
    }

    public void FailedTrick()
    {
        Debug.Log("You Failed!");
    }

    public void SuceedTrick()
    {
        var playerId = 1;
        if(gameObject.name == "Player 1")
        {
            playerId = 0;
        }
        Debug.Log("Congrats! your score = " + playerSync.SuceedTricks(playerId));
    }

}
