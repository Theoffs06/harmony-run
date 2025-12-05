using UnityEngine;

public class PlayerSyncActions : MonoBehaviour {
    [Header("Score Multiplier")]
    [SerializeField] private int baseTrickScore;
    [SerializeField] private float proximityScoreMultiplier;
    [SerializeField] private float maxTimingMultiplierTrigger;
    [SerializeField] private float tricksTimingScoreMultiplier;

    private float _player1LastTrickTime;
    private float _player2LastTrickTime = 1;
    private bool _player1MadeATrick;
    private bool _player2MadeATrick;

    public int SucedTricks(int player) {
        var points = baseTrickScore;
        
        if (player == 0) {
            _player1LastTrickTime = Time.time;
            _player1MadeATrick = true;
        } else {
            _player2LastTrickTime = Time.time;
            _player2MadeATrick = true;
        }

        if (!_player1MadeATrick || !_player2MadeATrick) return points;
        var tricksTimingDifference = Mathf.Abs(_player1LastTrickTime - _player2LastTrickTime);
        
        _player1MadeATrick = false;
        _player2MadeATrick = false;
            
        if (tricksTimingDifference > 0 && tricksTimingDifference <= maxTimingMultiplierTrigger) {
            points = (int)(points * (1 + tricksTimingScoreMultiplier * (maxTimingMultiplierTrigger - tricksTimingDifference)/maxTimingMultiplierTrigger));
        }

        return points;
    }
}
