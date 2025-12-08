using System.Linq;
using Player;
using Triggers;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;
    [SerializeField] private DynamicSplitScreen cameraManager;
    [SerializeField] private EndTrigger endTrigger;

    [Header("UI")] 
    [SerializeField] private UIPlayerPosition player1UI;
    [SerializeField] private UIPlayerPosition player2UI;
    
    [SerializeField] private UIScore player1ScoreUI;
    [SerializeField] private UIScore player2ScoreUI;
    
    [SerializeField] private UISpeedBar player1SpeedUI;
    [SerializeField] private UISpeedBar player2SpeedUI;
    
    [SerializeField] private UIBoostBar boostBarUI;
    [SerializeField] private UIChronometer chronometerUI;
    [SerializeField] private UITurns turnsUI;

    [SerializeField] private Leaderboard leaderboard;
    [SerializeField] private GameObject leaderboardObject;

    [SerializeField] private GameObject hud;
    
    private bool _isGameOver;
    private int _playersArrived;

    private void Awake() {
        boostBarUI.OnCreate();
        chronometerUI.OnCreate();
        turnsUI.OnCreate();
        
        player1SpeedUI.OnCreate();
        player2SpeedUI.OnCreate();
        
        endTrigger.OnCreate(this, turnsUI);
        
        player1.OnCreate(boostBarUI, player1SpeedUI, player1ScoreUI);
        player2.OnCreate(boostBarUI, player2SpeedUI, player2ScoreUI);
        
        cameraManager.OnCreate(player1.transform, player2.transform);
        player1UI.OnCreate(player1.transform, cameraManager);
        player2UI.OnCreate(player2.transform, cameraManager);
    }

    private void Start() {
        player1.OnStart();
        player2.OnStart();
    }
    
    private void Update() {
        if (_isGameOver) return;
        if (Chronometer.Seconds >= 180) OnGameOver();
        Chronometer.Update(Time.deltaTime);
        
        cameraManager.OnUpdate();
        chronometerUI.OnUpdate();
        
        player1.OnUpdate();
        player1UI.OnUpdate();
        
        player2.OnUpdate();
        player2UI.OnUpdate();
    }
    
    private void FixedUpdate() {
        if (_isGameOver) return;
        player1.OnFixedUpdate();
        player2.OnFixedUpdate();
    }

    public void OnGameOver() {
        if (++_playersArrived <= 1) return;
        
        hud.SetActive(false);
        leaderboardObject.SetActive(true);
        leaderboard.NewEntry(GenerateRandomLetters(3), GenerateRandomLetters(3));
        _isGameOver = true;
    }

    private static string GenerateRandomLetters(int length) {
        var random = new System.Random();
        
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}