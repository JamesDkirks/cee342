using UnityEngine;

public class MusicPlayTrigger : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayMusic()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
