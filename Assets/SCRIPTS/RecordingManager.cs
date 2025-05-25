using System.Collections.Generic;
using UnityEngine;

public class RecordingManager : MonoBehaviour
{
    [System.Serializable]
    public class DrumHitEvent
    {
        public string padName;
        public float timeStamp;
    }

    public bool isRecording = false;
    public bool isPlaying = false;

    private float startTime;
    private List<DrumHitEvent> recordedHits = new List<DrumHitEvent>();
    private Dictionary<string, DrumPad> padDict = new Dictionary<string, DrumPad>();

    public List<DrumPad> pads;

    private void Start()
    {
        foreach (var pad in pads)
        {
            pad.OnPadHit += RecordPadHit;
            padDict.Add(pad.padName, pad);
        }
    }

    public void StartRecording()
    {
        recordedHits.Clear();
        isRecording = true;
        startTime = Time.time;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    public void StartPlayback()
    {
        if (recordedHits.Count == 0) return;
        isPlaying = true;
        StartCoroutine(PlaybackCoroutine());
    }

    public void StopPlayback()
    {
        isPlaying = false;
        StopAllCoroutines();
    }

    private void RecordPadHit(string padName)
    {
        if (isRecording)
        {
            float time = Time.time - startTime;
            recordedHits.Add(new DrumHitEvent { padName = padName, timeStamp = time });
        }
    }

    private System.Collections.IEnumerator PlaybackCoroutine()
    {
        float playStart = Time.time;
        foreach (var hit in recordedHits)
        {
            yield return new WaitForSeconds(hit.timeStamp - (Time.time - playStart));
            if (padDict.ContainsKey(hit.padName))
                padDict[hit.padName].PlayPad();
        }

        isPlaying = false;
    }

    public List<DrumHitEvent> GetSessionData()
    {
        return recordedHits;
    }

    public void LoadSessionData(List<DrumHitEvent> hits)
    {
        recordedHits = hits;
    }

    // Call this from UI Button onClick, passing the pad name
    public void TriggerPadFromUI(string padName)
    {
        if (padDict.ContainsKey(padName))
        {
            padDict[padName].PlayPad();
            RecordPadHit(padName);
        }
    }
}
