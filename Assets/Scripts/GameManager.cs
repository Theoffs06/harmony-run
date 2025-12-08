using Player;
using Triggers;
using UI;
using UnityEngine;

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
    
    private bool _isGameOver;

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
        _isGameOver = true;
    }
}