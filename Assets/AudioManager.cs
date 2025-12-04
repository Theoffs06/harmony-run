using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Événements FMOD que l'on souhaite garder actifs entre les scènes
    [Header("Events permanents entre scènes")]
    public EventReference musicEvent;
    public EventReference ambienceEvent;

    private EventInstance musicInstance;
    private EventInstance ambienceInstance;

    // Init gameobject persistant entre les scènes
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitPersistentEvents();
    }

    // Lance les événements FMOD persistants
    private void InitPersistentEvents()
    {
        // Musique
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();

        // Ambiance
        ambienceInstance = RuntimeManager.CreateInstance(ambienceEvent);
        ambienceInstance.start();
    }
}

