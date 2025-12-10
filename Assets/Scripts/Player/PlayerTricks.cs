using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Player {
    [RequireComponent(typeof(Animator))]
    public class PlayerTricks : MonoBehaviour {
        [SerializeField] private UIBoostBar boostBar;
        [SerializeField] private float scoreConverterFactor = 0.001f;

        public UnityEvent OnPlayerFailedTrick;

        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");

        private PlayerAudio _audio;
        private Animator _animator; 
        private PlayerSyncActions _syncActions; 
        private UIScore _uiScore;
        private int _trickCounter;

        public void OnCreate(UIScore uiScore, PlayerAudio  audioManager) {
            _audio = audioManager;
            _uiScore = uiScore;

            _animator = GetComponent<Animator>();
            _syncActions = GetComponentInParent<PlayerSyncActions>();
        }

        public int GetTrickCounter() => _trickCounter;

        public void OnUpdate(bool isGrounded) {
            _animator.SetBool(IsGrounded, isGrounded);
            if (isGrounded) {
                _trickCounter = 0;
            }
        }

        public void LaunchTrick()
        {
            _animator.Play("Kickflip");
            _audio.OnFigureTry();
        }

        public void SucceedTrick()
        {
            _trickCounter++;

            var trick = _syncActions.SucceedTricks(gameObject.name == "Player 1" ? 0 : 1);

            //_uiScore.IncreaseScore(trick.Item1);
            boostBar.IncreaseBoost(trick.Item1 * scoreConverterFactor);
            _audio.OnFigureSuccess(_trickCounter);
        }

        public void FailedTrick() {
            _uiScore.ResetScore();
            
            boostBar.DecreaseBoost(100 * scoreConverterFactor);
            _audio.OnFigureFail();

            _trickCounter = 0;
            OnPlayerFailedTrick.Invoke();
        }
    }
}
