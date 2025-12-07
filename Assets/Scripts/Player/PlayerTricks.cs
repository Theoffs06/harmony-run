using UI;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Animator))]
    public class PlayerTricks : MonoBehaviour {
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    
        [SerializeField] private UIScore uiScore;
    
        private Animator _animator; 
        private PlayerSyncActions _syncActions;

        public void OnCreate() {
            _animator = GetComponent<Animator>();
            _syncActions = GetComponentInParent<PlayerSyncActions>();
        }

        public void OnUpdate(bool isGrounded) => _animator.SetBool(IsGrounded, isGrounded);
        
        public void LaunchTrick() => _animator.Play("Kickflip");
        public void SucceedTrick() => uiScore.IncreaseScore(_syncActions.SucceedTricks(gameObject.name == "Player 1" ? 0 : 1));
        public void FailedTrick() => uiScore.ResetScore();
    }
}
