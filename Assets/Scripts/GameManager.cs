using System.Linq;
using Player;
using Triggers;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    [SerializeField] private UIBoostBar boostBarUI;
    [SerializeField] private UIChronometer chronometerUI;
    [SerializeField] private UITurns turnsUI;

    [SerializeField] private GameObject hud;
    
    private bool _isGameOver;
    private int _playersArrived;

    private void Awake() {
        Chronometer.Reset();
        
        boostBarUI.OnCreate();
        chronometerUI.OnCreate();
        turnsUI.OnCreate();
        
        endTrigger.OnCreate(this, turnsUI);
        
        player1.OnCreate(boostBarUI, player1ScoreUI);
        player2.OnCreate(boostBarUI, player2ScoreUI);
        
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
        _isGameOver = true;
        
        hud.SetActive(false);
        LeaderBoard.NewEntry(GenerateRandomLetters(3), GenerateRandomLetters(3));
        SceneManager.LoadScene("Leaderboard");
    }

    private static string GenerateRandomLetters(int length) {
        var random = new System.Random();
        
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}