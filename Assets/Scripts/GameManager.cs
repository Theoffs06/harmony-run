using System;
using System.Collections;
using System.Linq;
using FMODUnity;
using Player;
using TMPro;
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
    [SerializeField] private TMP_Text startTxt;

    [Header("Music Distance Parameter")]
    [SerializeField] private float maxDistanceForMusic;
    [SerializeField] private float minDistanceForMusic;
    
    private bool _isGameOver;
    private int _playersArrived;

    private void Awake() {
        _isGameOver = true;
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
        
        StartCoroutine(StartGame());
    }

    private void Start() {
        player1.OnStart();
        player2.OnStart();
    }
    
    private void Update() {
        player1UI.OnUpdate();
        player2UI.OnUpdate();
        
        if (_isGameOver) return;
        if (Chronometer.Seconds >= 180) OnGameOver();
        Chronometer.Update(Time.deltaTime);
        
        cameraManager.OnUpdate();
        chronometerUI.OnUpdate();
        
        player1.OnUpdate();
        player2.OnUpdate();

        // PlayerDistance = 0 if distance <= minDistanceForMusic
        // PlayerDistance = 100 if distance >= maxDistanceForMusic
        RuntimeManager.StudioSystem.setParameterByName("PlayerDistance",
            Mathf.Clamp(
                100*(Vector3.Distance(player1.transform.position, player2.transform.position) - minDistanceForMusic)
                /maxDistanceForMusic
            , 0f, 100f)
        );

        // print(Mathf.Clamp(100*(Vector3.Distance(player1.transform.position, player2.transform.position) - minDistanceForMusic)/maxDistanceForMusic, 0f, 100f));
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
        RuntimeManager.StudioSystem.setParameterByName("MenuFactor", 1f);
        SceneManager.LoadScene("Leaderboard");
    }

    private IEnumerator StartGame() {
        for (var i = 3; i >= 1; i--) {
            startTxt.SetText(i.ToString());
            yield return PlayPopAndWait(1.5f);
        }
        
        startTxt.SetText("GO!");
        yield return PlayPopAndWait(1.75f);
        
        RuntimeManager.StudioSystem.setParameterByName("MenuFactor", 0f);
        _isGameOver = false;
        startTxt.gameObject.SetActive(false);
    }
    
    private IEnumerator PlayPopAndWait(float duration) {
        var popScale = Vector3.one * 1.6f;
        
        if (startTxt.transform && popScale != Vector3.one) {
            var initial = startTxt.transform.localScale;
            const float half = 0.18f;
            
            var t = 0f;
            while (t < half) {
                t += Time.unscaledDeltaTime;
                startTxt.transform.localScale = Vector3.Lerp(initial, popScale, t / half);
                yield return null;
            }
            
            t = 0f;
            while (t < half) {
                t += Time.unscaledDeltaTime;
                startTxt.transform.localScale = Vector3.Lerp(popScale, initial, t / half);
                yield return null;
            }
            
            startTxt.transform.localScale = initial;
        }
        
        yield return new WaitForSecondsRealtime(duration - 0.18f * 2f >= 0f ? duration - 0.18f * 2f : 0f);
    }
    
    private static string GenerateRandomLetters(int length) {
        var random = new System.Random();
        
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}