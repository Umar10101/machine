using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public RecordingManager recordingManager;
    public SessionManager sessionManager;

    public Button[] stepButtons; // Assign in Inspector: 16 step buttons
    public string[] stepAssignments = new string[16]; // Stores padName for each step
    private int? selectedStepIndex = null;
    private bool isSequencerRecording = false;
    private Coroutine sequenceCoroutine;

    public float stepInterval = 0.5f; // Time between steps

    private void Start()
    {
        // Deactivate step buttons at start
        SetStepButtonsInteractable(false);
    }

    public void OnStartRecording()
    {
        recordingManager.StartRecording();
    }

    public void OnStopRecording()
    {
        recordingManager.StopRecording();
    }

    public void OnPlay()
    {
        recordingManager.StartPlayback();
        PlaySequence();
    }

    public void OnStop()
    {
        recordingManager.StopPlayback();
        StopSequence();
    }

    public void OnSave()
    {
        sessionManager.SaveSession();
    }

    public void OnLoad()
    {
        sessionManager.LoadSession();
    }

    public void OnPadUIButton(string padName)
    {
        recordingManager.TriggerPadFromUI(padName);

        // If a step is selected, assign this pad to the step
        if (isSequencerRecording && selectedStepIndex.HasValue)
        {
            stepAssignments[selectedStepIndex.Value] = padName;
            selectedStepIndex = null;
            HighlightSelectedStep(-1);
        }
    }

    // Call this from each step button's onClick, passing its index (0-15)
    public void OnStepButton(int stepIndex)
    {
        if (!isSequencerRecording) return;
        selectedStepIndex = stepIndex;
        HighlightSelectedStep(stepIndex);
    }

    // Call this from the UI "Record" button
    public void OnSequencerRecord()
    {
        isSequencerRecording = true;
        SetStepButtonsInteractable(true);
    }

    // Call this to exit record mode (optional)
    public void OnSequencerRecordStop()
    {
        isSequencerRecording = false;
        SetStepButtonsInteractable(false);
        selectedStepIndex = null;
        HighlightSelectedStep(-1);
    }

    private void SetStepButtonsInteractable(bool interactable)
    {
        foreach (var btn in stepButtons)
            btn.interactable = interactable;
    }

    private void HighlightSelectedStep(int stepIndex)
    {
        // Optional: visually highlight the selected step button
        // Example: change button color
        for (int i = 0; i < stepButtons.Length; i++)
        {
            var colors = stepButtons[i].colors;
            colors.normalColor = (i == stepIndex) ? Color.yellow : Color.white;
            stepButtons[i].colors = colors;
        }
    }

    private void PlaySequence()
    {
        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);
        sequenceCoroutine = StartCoroutine(SequenceCoroutine());
    }

    private void StopSequence()
    {
        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);
        sequenceCoroutine = null;
    }

    private IEnumerator SequenceCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < stepAssignments.Length; i++)
            {
                string padName = stepAssignments[i];
                if (!string.IsNullOrEmpty(padName))
                {
                    recordingManager.TriggerPadFromUI(padName);
                }
                yield return new WaitForSeconds(stepInterval);
            }
        }
    }
}
