using UnityEngine;

public class PianoKeyTouch : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched by: " + other.name);

        if (other.name.ToLower().Contains("index") || other.CompareTag("FingerTip"))
        {
            audioSource.Play();
        }
    }
}
