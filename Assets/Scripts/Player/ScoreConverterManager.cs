using System.Collections;
using UI;
using UnityEngine;

namespace Player {
    public class ScoreConverterManager : MonoBehaviour {
        [Header("Score Converter Parameters")]
        [SerializeField] private float scoreConverterMaxDelay = 2;
        [SerializeField] private float scoreConverterFactor = 0.001f;

        [Header("GameObjects")]
        [SerializeField] private UIBoostBar boostBar;
        [SerializeField] private UIScore player1Score;
        [SerializeField] private UIScore player2Score;

        private float _player1LastTriggerTiming = -100;
        private float _player2LastTriggerTiming = -100;
        private bool _playersAreSync;

        public void TriggeredScoreConverter(int player) {
            if (player == 0) {
                _player1LastTriggerTiming = Time.time;
            }
            else {
                _player2LastTriggerTiming = Time.time;
            }

            if(Mathf.Abs(_player1LastTriggerTiming - _player2LastTriggerTiming) <= scoreConverterMaxDelay) {
                boostBar.IncreaseBoost((player1Score.CurrentScore + player2Score.CurrentScore)*scoreConverterFactor * 2f);
                player1Score.ResetScore();
                player2Score.ResetScore();
                _playersAreSync=true;
            } else {
                StartCoroutine(WaitAndConvertScore(player));
            }
        }
        IEnumerator WaitAndConvertScore(int player) {
            yield return new WaitForSeconds(scoreConverterMaxDelay);
            if (_playersAreSync) {
                _playersAreSync = false;
            } 
            else {
                if (player == 0) {
                    boostBar.IncreaseBoost(player1Score.CurrentScore * scoreConverterFactor);
                    player1Score.ResetScore();
                }
                else {
                    boostBar.IncreaseBoost(player2Score.CurrentScore * scoreConverterFactor);
                    player2Score.ResetScore();
                }
            }
        }
    }
}
