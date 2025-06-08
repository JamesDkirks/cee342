using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections; // <-- Add this for coroutine

public class PianoTutorialManager : MonoBehaviour
{
    public List<TutorialKeyButton> tutorialKeys;
    private int currentKeyIndex = 0;
    public GameObject startButton; // Assign in Inspector 
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogTitle;
    public TextMeshProUGUI dialogText;
    public AudioSource warningAudioSource; // Optional
    public AudioClip warningClip; // Optional
    public TextMeshProUGUI feedbackText; // Assign in Inspector for wrong key feedback

    private int wrongKeyCount = 0;

    void Start()
    {
        Debug.Log("[PianoTutorialManager] Tutorial started.");
        if (tutorialKeys == null || tutorialKeys.Count == 0)
        {
            Debug.LogError("[PianoTutorialManager] No tutorial keys assigned!");
            return;
        }

        // Comment this out if you want to start only via button
        // ActivateKey(currentKeyIndex);
    }

    public void StartTutorial()
    {
        currentKeyIndex = 0;
        wrongKeyCount = 0; // Reset wrong key count at start
        Debug.Log("[PianoTutorialManager] Tutorial started via UI button.");
        if (tutorialKeys == null || tutorialKeys.Count == 0)
        {
            Debug.LogError("[PianoTutorialManager] No tutorial keys assigned!");
            return;
        }
        ActivateKey(currentKeyIndex);
        
        // Optional: Hide the start button after pressing
        if (startButton != null)
            startButton.SetActive(false);
        // Hide dialog and feedback at start
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
        if (feedbackText != null)
            feedbackText.text = "";
    }

    public void OnStartToggleChanged(bool isOn)
    {
        if (isOn)
            StartTutorial();
    }

    public void NotifyKeyPressed(TutorialKeyButton button)
    {
        Debug.Log($"[PianoTutorialManager] NotifyKeyPressed from key: {button.KeyName}, button: {button}, expected: {tutorialKeys[currentKeyIndex]}");
        Debug.Log($"[PianoTutorialManager] CurrentKeyIndex: {currentKeyIndex}");

        if (tutorialKeys[currentKeyIndex] != button)
        {
            wrongKeyCount++;
            Debug.LogWarning($"[PianoTutorialManager] Wrong key pressed: {button.KeyName}. Total wrong: {wrongKeyCount}");
            // Play warning sound if assigned
            if (warningAudioSource != null && warningClip != null)
                warningAudioSource.PlayOneShot(warningClip);
            // Show feedback in UI if assigned
            if (feedbackText != null)
            {
                feedbackText.text = $"Wrong key! ({wrongKeyCount})";
                StopAllCoroutines();
                StartCoroutine(ClearFeedbackAfterDelay(1.5f));
            }
            return;
        }

        // Unhighlight current key
        tutorialKeys[currentKeyIndex].SetAsCurrentTutorialKey(false);

        currentKeyIndex++;

        if (currentKeyIndex < tutorialKeys.Count)
        {
            Debug.Log($"[PianoTutorialManager] Advancing to key {currentKeyIndex}");
            ActivateKey(currentKeyIndex);
        }
        else
        {
            Debug.Log("[PianoTutorialManager] Tutorial complete!");
            ShowFinalFeedback();
        }
    }

    private void ActivateKey(int index)
    {
        if (index < 0 || index >= tutorialKeys.Count)
        {
            Debug.LogWarning($"[PianoTutorialManager] Index {index} out of bounds.");
            return;
        }

        Debug.Log($"[PianoTutorialManager] Activating key {index}: {tutorialKeys[index].KeyName}");
        tutorialKeys[index].SetAsCurrentTutorialKey(true);
    }

    public void ShowSheetMusicDialog()
    {
        dialogPanel.SetActive(true);
        dialogTitle.text = "Twinkle Star - Sheet Music";
        dialogText.text = "♪ C4  C4  G4  G4  A4  A4  G4 ♪\n♪ F4  F4  E4  E4  D4  D4  C4 ♪";
    }

    private void ShowFinalFeedback()
    {
        dialogPanel.SetActive(true);
        dialogTitle.text = "Lesson Complete!";
        float accuracy = 100f * (tutorialKeys.Count - wrongKeyCount) / tutorialKeys.Count;
        dialogText.text = $"Wrong Keys Pressed: {wrongKeyCount}\nAccuracy: {accuracy:F0}%\nGreat job! 🎵";
        if (feedbackText != null)
            feedbackText.text = "";
    }

    private IEnumerator ClearFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (feedbackText != null)
            feedbackText.text = "";
    }
}
