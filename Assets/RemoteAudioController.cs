using UnityEngine;

public class RemoteAudioController : MonoBehaviour
{
    public AudioSource audioSource;

    public void ToggleAudio(bool isOn)
    {
        if (audioSource == null) return;

        if (isOn)
            audioSource.Play();
        else
            audioSource.Pause();
    }
}