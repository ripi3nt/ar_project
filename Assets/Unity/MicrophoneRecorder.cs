using UnityEngine;
using System.IO;
using UnityEngine.Android;

public class MicrophoneRecorder : MonoBehaviour
{
    public int duration = 10;
    public string outputFileName = "in_audio.wav";

    private AudioClip recordedClip;
    private string micDevice;

    void Start()
    {
        if(!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        if (Microphone.devices.Length > 0)
        {
            micDevice = Microphone.devices[0];
        }
        else
        {
            Debug.LogError("No microphone detected.");
        }
    }

    public void StartRecording()
    {
        if (micDevice != null)
        {
            recordedClip = Microphone.Start(micDevice, false, duration, 44100);
            Debug.Log("Recording started...");
        }
    }

    public void StopAndSaveRecording()
    {
        if (Microphone.IsRecording(micDevice))
        {
            Microphone.End(micDevice);
            Debug.Log("Recording stopped. Saving...");
            SavWav.Save(outputFileName, recordedClip);
            Debug.Log("Saved to: " + Path.Combine(Application.persistentDataPath, outputFileName));
        }
    }

}
