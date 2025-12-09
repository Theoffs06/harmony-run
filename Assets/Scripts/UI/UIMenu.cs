using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI {
    public class UIMenu : MonoBehaviour {
        private InputAction _nextAction;
        
        private void Awake() {
            _nextAction = InputSystem.actions.FindAction("Next");
        }

        private void OnEnable() {
            _nextAction.Enable();
            _nextAction.performed += PressStart;
        }
        
        private void OnDisable() {
            _nextAction.Disable();
            _nextAction.performed -= PressStart;
        }
        
        private static void PressStart(InputAction.CallbackContext obj) {
            AudioManager.Instance.PlayUISound();
            SceneManager.LoadScene("Tuto");
            
        }
    }
}
