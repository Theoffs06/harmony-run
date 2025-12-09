using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private GameObject[] anims;

    private InputAction _next;
    private int _state;

    private void Awake() {
        for (var i = 0; i < anims.Length; i++) anims[i].SetActive(i == 0);

        _next = InputSystem.actions.FindAction("Next");
    }

    private void OnEnable() {
        _next.Enable();
        
        _next.performed += NextAnim;
    }
    
    private void OnDisable() {
        _next.Disable();
        
        _next.performed -= NextAnim;
    }

    private void NextAnim(InputAction.CallbackContext obj) {
        if (++_state >= anims.Length) {
            StartCoroutine(LoadGameAfterAdditive());
            return;
        }
        
        anims[_state - 1].SetActive(false);
        anims[_state].SetActive(true);
    }

    private static IEnumerator LoadGameAfterAdditive() {
        var op = SceneManager.LoadSceneAsync("Circuit1_Art", LoadSceneMode.Additive);
        
        while (op is { isDone: false }) yield return null;
        SceneManager.UnloadSceneAsync("Tuto");
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
    }
}
