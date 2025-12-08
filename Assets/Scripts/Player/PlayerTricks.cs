using UI;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Animator))]
    public class PlayerTricks : MonoBehaviour {
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");

        private PlayerAudio _audio;
        private Animator _animator; 
        private PlayerSyncActions _syncActions; 
        private UIScore _uiScore;

        public void OnCreate(UIScore uiScore, PlayerAudio  audioManager) {
            _audio = audioManager;
            _uiScore = uiScore;
            
            _animator = GetComponent<Animator>();
            _syncActions = GetComponentInParent<PlayerSyncActions>();
        }

        public void OnUpdate(bool isGrounded) => _animator.SetBool(IsGrounded, isGrounded);

        public void LaunchTrick() {
            _animator.Play("Kickflip");
            _audio.OnFigureTry();
        }

        public void SucceedTrick() {
            var trick = _syncActions.SucceedTricks(gameObject.name == "Player 1" ? 0 : 1);
            
            _uiScore.IncreaseScore(trick.Item1);
            _audio.OnFigureSuccess(trick.Item2);
        }

        public void FailedTrick() {
            _uiScore.ResetScore();
            _audio.OnFigureFail();
        }
    }
}
