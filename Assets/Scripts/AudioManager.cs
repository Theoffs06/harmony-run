using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    // Événements FMOD que l'on souhaite garder actifs entre les scènes
    [Header("Events permanents entre scènes")]
    public EventReference musicEvent;
    public EventReference ambienceEvent;
    public EventReference uiEvent;

    private EventInstance _musicInstance;
    private EventInstance _ambienceInstance;
    private EventInstance _uiInstance;

    // Init gameobject persistant entre les scènes
    private void Awake() {
        if (Instance) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitPersistentEvents();
        RuntimeManager.StudioSystem.setParameterByName("MenuFactor", 1f);
    }

    // Lance les événements FMOD persistants
    private void InitPersistentEvents() {
        // Musique
        _musicInstance = RuntimeManager.CreateInstance(musicEvent);
        _musicInstance.start();

        // Ambiance
        _ambienceInstance = RuntimeManager.CreateInstance(ambienceEvent);
        _ambienceInstance.start();

        // Sons UI
        _uiInstance = RuntimeManager.CreateInstance(uiEvent);
    }

    public void PlayUISound() {
        _uiInstance.start();
    }
}

