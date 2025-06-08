using UnityEngine;
using UnityEngine.Events;

public class TutorialKeyButton : MonoBehaviour
{
    [Header("Tutorial Key Settings")]
    public string KeyName;
    public Material HighlightMat;
    public MeshRenderer TargetRenderer;
    public PianoTutorialManager Manager;

    [Header("Events")]
    public UnityEvent OnPressed;

    private Material originalMat;

    void Start()
    {
        if (TargetRenderer != null)
        {
            originalMat = TargetRenderer.material;
            Debug.Log($"[TutorialKeyButton] Renderer assigned and material cached for {KeyName}");
        }
        else
        {
            Debug.LogWarning($"[TutorialKeyButton] No target renderer assigned for {KeyName}");
        }
    }

    public void SetAsCurrentTutorialKey(bool highlight)
    {
        Debug.Log($"[TutorialKeyButton] Highlighting key: {KeyName} = {highlight}");

        if (TargetRenderer == null || HighlightMat == null || originalMat == null)
        {
            Debug.LogWarning($"[TutorialKeyButton] Cannot change highlight state — missing reference on {KeyName}");
            return;
        }

        TargetRenderer.material = highlight ? HighlightMat : originalMat;
    }

    public void OnButtonPressed()
    {
        Debug.Log($"[TutorialKeyButton] OnButtonPressed: {KeyName}");
        Manager?.NotifyKeyPressed(this);
        OnPressed?.Invoke();
    }
}
