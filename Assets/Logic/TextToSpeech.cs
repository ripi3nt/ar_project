using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

class TextToSpeech
{

    private string uploadUrl = "https://api.gladia.io/v2/upload";
    private string textUrl = "https://api.gladia.io/v2/pre-recorded";
    //string uploadHeaders = "{\"x-gladia-key\": \"975487c9-579a-4f75-ab8b-033616d85eb3\"}";
    private string apiKey = "975487c9-579a-4f75-ab8b-033616d85eb3";
    private string audioUrl;
    private string audioId;
    private MonoBehaviour coroutineRunner;

    public TextToSpeech(MonoBehaviour coroutineRunner)
    {
        this.coroutineRunner = coroutineRunner;
    }

    private IEnumerator uploadAudio(string audioFile)
    {
        byte[] audioBytes = File.ReadAllBytes(audioFile);

        //List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        //form.Add(new MultipartFormFileSection("audio", audioBytes, "testAudio.wav", "audio/wav"));
        WWWForm form = new WWWForm();
        form.AddBinaryData("audio", audioBytes, "in_audio.wav", "audio/wav");

        UnityWebRequest request = UnityWebRequest.Post(uploadUrl, form);
        request.SetRequestHeader("x-gladia-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Web request failed: " + request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                Debug.Log("Response body: " + request.downloadHandler.text);
                UploadResponse response = JsonUtility.FromJson<UploadResponse>(request.downloadHandler.text);
                audioUrl = response.audio_url;
            }
            else
            {
                Debug.Log("Server returned status: " + request.responseCode);
            }
        }
    }

    private IEnumerator startTranscription()
    {

        UnityWebRequest request = UnityWebRequest.Post(textUrl, $"{{\"audio_url\": \"{audioUrl}\"}}", "application/json");
        request.SetRequestHeader("x-gladia-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Web request failed: " + request.error);
        }
        else
        {
                Debug.Log("Response body :" + request.downloadHandler.text);
                PreRecoredResult jsonResult = JsonUtility.FromJson<PreRecoredResult>(request.downloadHandler.text);
                audioId = jsonResult.id;
        }
    }

    private IEnumerator pollTranscriptionResult(Action<string> onSucc, int maxAttempts = 20, int delaySeconds = 1)
    {
        string requestUrl = $"{textUrl}/{audioId}";

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            UnityWebRequest response = UnityWebRequest.Get(requestUrl);
            response.SetRequestHeader("x-gladia-key", apiKey);
            yield return response.SendWebRequest();

            if (response.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("API call failed");
            }

            string content = response.downloadHandler.text;

            TranscriptionResponse json = JsonUtility.FromJson<TranscriptionResponse>(content);
            Debug.Log(content);

            if (json.status == "done")
            {
                string transcription = json.result.transcription.full_transcript;
                onSucc(transcription);
                yield break;
            }
            else if (json.status == "error")
            {
                //notify main logic of error
                Debug.Log("Transcription failed: " + json.error_code);
                yield break;
            }

            yield return new WaitForSeconds(delaySeconds);
        }
        yield break;

    }

    public IEnumerator getTextFromAudioFile(Action<string> handleTranscription, string filePath) {
        yield return coroutineRunner.StartCoroutine(uploadAudio(filePath));
        yield return coroutineRunner.StartCoroutine(startTranscription());
        yield return coroutineRunner.StartCoroutine(pollTranscriptionResult(handleTranscription));
    }


}