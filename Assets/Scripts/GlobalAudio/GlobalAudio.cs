using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio instance;

    private AudioSource audioSource;


    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("[GlobalAudio] Missing AudioSource component!");
        }

    }

    public void PlayAudio(string name, float volume = 1f)
    {
        AudioClip clip = GetClipByName(name);

        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"[GlobalAudio] Audio clip '{name}' not found.");
        }
    }

    public void PlayAudioPitch(string name, float pitch)
    {
        AudioClip clip = GetClipByName(name);

        if (clip == null)
        {
            Debug.LogWarning($"[GlobalAudio] Audio clip '{name}' not found.");
            return;
        }

        GameObject tempAudioObj = new GameObject($"TempAudio-{name}");
        AudioSource tempSource = tempAudioObj.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.pitch = pitch;
        tempSource.Play();
        Destroy(tempAudioObj, clip.length / Mathf.Abs(pitch));
    }
    public void PlayAudioPitch(string name, float pitch, float volume)
    {
        AudioClip clip = GetClipByName(name);

        if (clip == null)
        {
            Debug.LogWarning($"[GlobalAudio] Audio clip '{name}' not found.");
            return;
        }

        GameObject tempAudioObj = new GameObject($"TempAudio-{name}");
        AudioSource tempSource = tempAudioObj.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.pitch = pitch;
        tempSource.volume = volume;
        tempSource.Play();
        Destroy(tempAudioObj, clip.length / Mathf.Abs(pitch));
    }
    public static bool played = false;
    public void PlayAudioPitchUnsame(string name, float pitch, float volume)
    {
        if (played) return ;
        AudioClip clip = GetClipByName(name);

        if (clip == null)
        {
            Debug.LogWarning($"[GlobalAudio] Audio clip '{name}' not found.");
            return;
        }

        GameObject tempAudioObj = new GameObject($"TempAudio-{name}");
        AudioSource tempSource = tempAudioObj.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.pitch = pitch;
        tempSource.volume = volume;
        tempSource.Play();
        Destroy(tempAudioObj, clip.length / Mathf.Abs(pitch));
        played = true;
        Tween.Delay(0.00001f).OnComplete(()=>played = false);
    }
    private AudioClip GetClipByName(string name)
    {
        return Resources.Load<AudioClip>("Audio/"+name);
    }
}
