using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

//nov comment
public class InputHandler : MonoBehaviour
{
    public Button resetButton;
    public Button micButton;
    public Button busyMicButton;
    public ARSession aRSession;
    public MicrophoneRecorder microphone;
    private TextToSpeech tts;
    public TextMeshProUGUI text;

    private GameObject dogObject;
    public PetData petData;
    public Animator dogAnimator;

    void Start()
    {
        tts = new TextToSpeech(this);
        resetButton.onClick.AddListener(handleReset);
        micButton.onClick.AddListener(handleMicrophone);
        EntityManager.Instance.SpawnPet();
        dogObject = EntityManager.Instance.PetInstance;

        if (dogObject != null)
        {
            dogAnimator = dogObject.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("No object assigned!");
        }
    }

    private void handleReset()
    {
        StartCoroutine(DoReset());
    }
    private IEnumerator DoReset()
    {
        Debug.Log("Reset clicked");

        CubePlacement.placed = false;

        if (dogAnimator != null)
        {
            dogAnimator.Rebind();
            dogAnimator.Update(0f);
        }
        else
        {
            Debug.LogWarning("Dog animator not found!");
        }

        aRSession.Reset();

        yield return null;
    }

    private void handleTranscription(String transcription)
    {
        text.text = transcription;
        micButton.gameObject.SetActive(true);
        busyMicButton.gameObject.SetActive(false);

        if (dogAnimator == null)
        {
            Debug.LogWarning("No animator assigned!");
            return;
        }

        string command = transcription.ToLower();
        Debug.Log("COMMAND RECEIVED: " + command);

        if (command.Contains("tail"))
        {
            Debug.Log("Triggering: tail");
            dogAnimator.SetTrigger("tail");
            
        }
        else if (command.Contains("sit"))
        {
            Debug.Log("Triggering: sit");
            dogAnimator.SetTrigger("sit");
           
        }
        else if (command.Contains("paw"))
        {
            Debug.Log("Triggering: paw");
            dogAnimator.SetTrigger("paw");
            
        }
        else if (command.Contains("walk"))
        {
            Debug.Log("Triggering: walk");
            dogAnimator.SetTrigger("walk");
            
        }
        else
        {
            Debug.Log("Invalid command");
            text.text = "\n(Wrong command!)";
        }
    }

    private void handleMicrophone()
    {
        StartCoroutine(doMicrophone());
    }

    private IEnumerator doMicrophone()
    {
        micButton.gameObject.SetActive(false);
        busyMicButton.gameObject.SetActive(true);
        microphone.StartRecording();
        yield return StartCoroutine(finishMicrophoneRecording(10));
        yield return StartCoroutine(tts.getTextFromAudioFile(handleTranscription, Application.persistentDataPath + "/in_audio.wav"));
    }

    private IEnumerator finishMicrophoneRecording(int duration)
    {
        yield return new WaitForSeconds(duration);
        microphone.StopAndSaveRecording();
    }

    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            handleTranscription("tail");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            handleTranscription("sit");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            handleTranscription("paw");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            handleTranscription("walk");
        }
    }
    */

    public void TestAnimationTrigger(string command)
    {
        handleTranscription(command);
    }
}
