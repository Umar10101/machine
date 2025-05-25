using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public DrumPad[] drumPads;

    public void SetVolume(float value)
    {
        foreach (var pad in drumPads)
        {
            pad.GetComponent<AudioSource>().volume = value;
        }
    }

    public void SetPitch(float value)
    {
        foreach (var pad in drumPads)
        {
            pad.GetComponent<AudioSource>().pitch = value;
        }
    }
}
