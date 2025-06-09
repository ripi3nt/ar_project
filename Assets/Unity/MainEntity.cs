using System.Collections;
using UnityEngine;

public class MainEntity : MonoBehaviour {
   public MicrophoneRecorder microphone; 
   private TextToSpeech tts;

    void Start()
    {
        tts = new TextToSpeech(this);
        microphone.StartRecording();
        StartCoroutine(sleep(5));
    }


    private IEnumerator sleep(int duration) {
        yield return new WaitForSeconds(duration);
    }
}