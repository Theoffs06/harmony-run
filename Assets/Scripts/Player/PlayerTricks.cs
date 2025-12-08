using UI;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Animator))]
    public class PlayerTricks : MonoBehaviour {
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        
        private Animator _animator; 
        private PlayerSyncActions _syncActions; 
        private UIScore _uiScore;

        public void OnCreate(UIScore uiScore) {
            _uiScore = uiScore;
            
            _animator = GetComponent<Animator>();
            _syncActions = GetComponentInParent<PlayerSyncActions>();
        }

        public void OnUpdate(bool isGrounded) => _animator.SetBool(IsGrounded, isGrounded);
        
        public void LaunchTrick() => _animator.Play("Kickflip");
        public void SucceedTrick() => _uiScore.IncreaseScore(_syncActions.SucceedTricks(gameObject.name == "Player 1" ? 0 : 1));
        public void FailedTrick() => _uiScore.ResetScore();
    }
}
