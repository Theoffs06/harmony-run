using System.Collections;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI {
    public class UILeaderboard : MonoBehaviour {
        [SerializeField] private TMP_Text uiList;
        
        private InputAction _nextAction;

        private void Awake() {
            uiList.SetText(LeaderBoard.Entries);
            
            _nextAction = InputSystem.actions.FindAction("Next");
        }

        private void OnEnable() {
            _nextAction.Enable();
            _nextAction.performed += PressRestart;
        }
        
        private void OnDisable() {
            _nextAction.Disable();
            _nextAction.performed -= PressRestart;
        }

        private void PressRestart(InputAction.CallbackContext obj) {
            RuntimeManager.StudioSystem.setParameterByName("MenuFactor", 0f);
            StartCoroutine(LoadGameAfterAdditive());
        }
        
        private static IEnumerator LoadGameAfterAdditive() {
            var op = SceneManager.LoadSceneAsync("Circuit1_Art", LoadSceneMode.Additive);
        
            while (op is { isDone: false }) yield return null;
            SceneManager.UnloadSceneAsync("Leaderboard");
            SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        }
    }
}
