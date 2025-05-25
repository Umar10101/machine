using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DrumPad : MonoBehaviour
{
    public string padName;
    private AudioSource audioSource;

    public System.Action<string> OnPadHit;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Call this from a UI Button
    public void PlayPad()
    {
        audioSource.Play();
        OnPadHit?.Invoke(padName); // This triggers recording
    }
}