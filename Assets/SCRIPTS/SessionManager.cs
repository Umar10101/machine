using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class SessionManager : MonoBehaviour
{
    public RecordingManager recordingManager;

    private string filePath => Application.persistentDataPath + "/drum_session.json";

    [Serializable]
    public class SessionDataWrapper
    {
        public List<RecordingManager.DrumHitEvent> events;
    }

    public void SaveSession()
    {
        var data = new SessionDataWrapper { events = recordingManager.GetSessionData() };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Session saved to: " + filePath);
    }

    public void LoadSession()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var data = JsonUtility.FromJson<SessionDataWrapper>(json);
            recordingManager.LoadSessionData(data.events);
            Debug.Log("Session loaded from: " + filePath);
        }
    }
}
