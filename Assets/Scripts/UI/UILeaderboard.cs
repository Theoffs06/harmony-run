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

        private static void PressRestart(InputAction.CallbackContext obj) {
            SceneManager.LoadScene("Game");
        }
    }
}
