using System;
using System.Collections;
using UI;
using UnityEngine;

public class ScoreConverterManager : MonoBehaviour
{
    [Header("Score Converter Parameters")]
    [SerializeField] private float ScoreConverterMaxDelay;
    [SerializeField] private float ScoreConverterFactor;

    [Header("GameObjects")]
    [SerializeField] private UIBoostBar boostBar;
    [SerializeField] private UIScore player1Score;
    [SerializeField] private UIScore player2Score;

    private float player1LastTriggerTiming = -100;
    private float player2LastTriggerTiming = -100;

    private bool playersAreSync = false;

    public void TriggeredScoreConverter(int player)
    {
        if (player == 0)
        {
            player1LastTriggerTiming = Time.time;
        }
        else
        {
            player2LastTriggerTiming = Time.time;
        }

        if(Mathf.Abs(player1LastTriggerTiming - player2LastTriggerTiming) <= ScoreConverterMaxDelay)
        {
            boostBar.IncreaseBoost((player1Score.CurrentScore + player2Score.CurrentScore)*ScoreConverterFactor * 2f);
            player1Score.ResetScore();
            player2Score.ResetScore();
            playersAreSync=true;
        } else
        {
            StartCoroutine(WaitAndConvertScore(player));
        }
    }
    IEnumerator WaitAndConvertScore(int player)
    {
        yield return new WaitForSeconds(ScoreConverterMaxDelay);
        if (playersAreSync)
        {
            playersAreSync = false;
        } else
        {
            if (player == 0)
            {
                boostBar.IncreaseBoost(player1Score.CurrentScore * ScoreConverterFactor);
                player1Score.ResetScore();
            }
            else
            {
                boostBar.IncreaseBoost(player2Score.CurrentScore * ScoreConverterFactor);
                player2Score.ResetScore();
            }
        }
    }
}
