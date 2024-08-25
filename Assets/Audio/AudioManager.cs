using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource soundObject;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicPlayer;

    [SerializeField] private AudioClip bloodyTears;
    [SerializeField] private AudioClip devilsRevival;

    void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }


    public void PlaySound(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundObject);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        float soundLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, soundLength);
    }

    public void PlayMusic(AudioClip audioClip, float volume)
    {
        musicPlayer.Stop();
        musicPlayer.clip = audioClip;
        musicPlayer.volume = volume;
        musicPlayer.Play();
    }

    public void CheckArea(string sceneName) 
    { 
        if(sceneName.Contains("Alba Forest") && musicPlayer.clip != bloodyTears) { PlayMusic(bloodyTears, 1); }
        if(sceneName.Contains("Village") && musicPlayer.clip != devilsRevival) { PlayMusic(devilsRevival, 1); }
    }

    public void SetSfxVolume(float value)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(value) * 20f);
    }
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(value) * 20f);
    }
}
