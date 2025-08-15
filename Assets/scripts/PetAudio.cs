using UnityEngine;

public class PetRandomSound : MonoBehaviour
{
    public PetData petData; // Assign in Inspector
    private AudioSource audioSource;
    private float nextSoundTime;

    void Start()
    {
        if (petData == null)
        {
            Debug.LogError("PetRandomSound: PetData is not assigned!");
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.clip = petData.petSound;

        ScheduleNextSound();
    }

    void Update()
    {
        // Only play sound if enough time has passed AND not currently playing
        if (Time.time >= nextSoundTime && !audioSource.isPlaying && petData.petSound != null)
        {
            Debug.Log($"{petData.petName} is playing sound at {Time.time:F2}");
            audioSource.Play();
            ScheduleNextSound();
        }
    }

    private void ScheduleNextSound()
    {
        // Randomly schedule between 10–15 seconds
        nextSoundTime = Time.time + Random.Range(10f, 15f);
        Debug.Log($"{petData.petName} next sound scheduled at {nextSoundTime:F2}");
    }
}
