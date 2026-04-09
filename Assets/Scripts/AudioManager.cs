using UnityEngine;

/// <summary>
/// Аудио менеджер - управляет фоновой музыкой и звуковыми эффектами.
/// </summary>
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;
    public bool isMusic = false;
    
    [HideInInspector] public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    public Sound[] sounds;
    
    [Header("Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Initialize all audio sources
        foreach (Sound sound in sounds)
        {
            GameObject audioObj = new GameObject("Sound_" + sound.name);
            audioObj.transform.SetParent(transform);
            
            sound.source = audioObj.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.isMusic ? musicVolume * sound.volume : sfxVolume * sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = false;
        }
    }
    
    public void Play(string soundName)
    {
        Sound sound = System.Array.Find(sounds, item => item.name == soundName);
        
        if (sound == null)
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
            return;
        }
        
        if (sound.isMusic)
        {
            // Stop other music first
            foreach (Sound s in sounds)
            {
                if (s.isMusic && s.source.isPlaying)
                {
                    s.source.Stop();
                }
            }
            
            sound.source.Play();
        }
        else
        {
            sound.source.PlayOneShot(sound.clip);
        }
    }
    
    public void Stop(string soundName)
    {
        Sound sound = System.Array.Find(sounds, item => item.name == soundName);
        
        if (sound == null)
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
            return;
        }
        
        sound.source.Stop();
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        
        foreach (Sound sound in sounds)
        {
            if (sound.isMusic)
            {
                sound.source.volume = musicVolume * sound.volume;
            }
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        
        foreach (Sound sound in sounds)
        {
            if (!sound.isMusic)
            {
                sound.source.volume = sfxVolume * sound.volume;
            }
        }
    }
}
