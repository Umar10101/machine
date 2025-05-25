using UnityEngine;
using UnityEngine.UI;

public class DrumPadUIController : MonoBehaviour
{
    public Button[] drumPadButtons; // Assign 16 UI Buttons in Inspector
    public AudioSource[] drumPadAudioSources; // Assign 16 AudioSources in Inspector

    private void Start()
    {
        for (int i = 0; i < drumPadButtons.Length && i < drumPadAudioSources.Length; i++)
        {
            int idx = i; // Capture index for closure
            drumPadButtons[i].onClick.AddListener(() => PlayPad(idx));
        }
    }

    public void PlayPad(int index)
    {
        if (index >= 0 && index < drumPadAudioSources.Length)
        {
            drumPadAudioSources[index].Play();
        }
    }
}
