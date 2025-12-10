using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;

namespace Player
{
    public class PlayerSyncActions : MonoBehaviour
    {
        [Header("Score Multiplier")]
        [SerializeField]
        private int baseTrickScore;

        [SerializeField]
        private float maxTimingMultiplierTrigger;

        [SerializeField]
        private float tricksTimingScoreMultiplier;

        [SerializeField]
        public UnityEvent<int> OnPlayerSucceededTricks = new();

        [SerializeField]
        public UnityEvent OnPlayerSucceededTricksDuo = new();

        private float _player1LastTrickTime;
        private float _player2LastTrickTime = 1;
        private bool _player1MadeATrick;
        private bool _player2MadeATrick;

        [SerializeField]
        private int _timingLevel;

        public int GetTimingLevel() => _timingLevel;

        [Header("Audio References")]
        [SerializeField] private EventReference figureOkayEvent;
        [SerializeField] private EventReference figureGoodEvent;
        [SerializeField] private EventReference figurePerfectEvent;

        public Tuple<int, bool> SucceedTricks(int player)
        {
            var points = baseTrickScore;

            OnPlayerSucceededTricks.Invoke(player);

            if (player == 0)
            {
                _player1LastTrickTime = Time.time;
                _player1MadeATrick = true;
            }
            else
            {
                _player2LastTrickTime = Time.time;
                _player2MadeATrick = true;
            }

            if (!_player1MadeATrick || !_player2MadeATrick)
                return new Tuple<int, bool>(points, false);
            var tricksTimingDifference = math.abs(_player1LastTrickTime - _player2LastTrickTime);

            _player1MadeATrick = false;
            _player2MadeATrick = false;

            if (tricksTimingDifference > 0 && tricksTimingDifference <= maxTimingMultiplierTrigger)
            {
                points = (int)(
                    points
                    * (
                        1
                        + tricksTimingScoreMultiplier
                            * (maxTimingMultiplierTrigger - tricksTimingDifference)
                            / maxTimingMultiplierTrigger
                    )
                );

                _timingLevel = TimingLevelFromTimingDifference(4, tricksTimingDifference);
                if (_timingLevel >= 3) RuntimeManager.PlayOneShotAttached(figurePerfectEvent, gameObject);
                else if (_timingLevel == 2) RuntimeManager.PlayOneShotAttached(figureGoodEvent, gameObject);
                else RuntimeManager.PlayOneShotAttached(figureOkayEvent, gameObject);

                OnPlayerSucceededTricksDuo.Invoke();

            }

            return new Tuple<int, bool>(points, true);
        }

        private int TimingLevelFromTimingDifference(int numberOfLevel, float timingDifference)
        {
            return numberOfLevel
                - (int)Mathf.Floor(timingDifference / maxTimingMultiplierTrigger * numberOfLevel);
        }
    }
}
