using UnityEngine;

public class DogFollower : MonoBehaviour
{
    [Header("Follow Settings")]
    public float minDistance = 1.5f;       // Minimum distance to camera
    public float maxSpeed = 5f;            // Maximum movement speed
    public float rotationSpeed = 5f;       // How fast the dog rotates

    private Transform cameraTransform;
    private GameObject dog;
    private Animator dogAnimator;

    private Vector3 lastPosition;
    private float currentSpeed;

    void Start()
    {
        cameraTransform = FindAnyObjectByType<Camera>()?.transform;

        if (cameraTransform == null)
            Debug.LogError("DogFollower: No camera found in scene!");
    }

    void Update()
    {
        if (cameraTransform == null) return;

        dog = BattleManager.Instance.selectedPet;
        if (dog == null) return;

        if (dogAnimator == null)
        {
            dogAnimator = dog.GetComponent<Animator>();
            if (dogAnimator == null)
                Debug.LogWarning("DogFollower: No Animator found on dog!");
        }

        Vector3 direction = cameraTransform.position - dog.transform.position;
        float distance = direction.magnitude;

        // Move dog if too far
        Vector3 targetPosition = dog.transform.position;
        if (distance > minDistance)
        {
            targetPosition = cameraTransform.position - direction.normalized * minDistance;
            targetPosition.y = dog.transform.position.y;

            // Smooth movement
            dog.transform.position = Vector3.Lerp(dog.transform.position, targetPosition, maxSpeed * Time.deltaTime);
        }

        // Calculate speed
        currentSpeed = ((dog.transform.position - lastPosition).magnitude) / Time.deltaTime;
        lastPosition = dog.transform.position;

        // Update animator
        /*if (dogAnimator != null)
        {
            Debug.Log("Speed: " + currentSpeed.ToString("F3"));
            if (currentSpeed > 0.01f)
            {
                dogAnimator.SetBool("isWalking", true);
                dogAnimator.SetFloat("walkSpeedMultiplier", Mathf.Clamp(currentSpeed / maxSpeed, 0.5f, 2f));
            }
            else
            {
                dogAnimator.SetBool("isWalking", false);
            }
        }*/

        // Rotate smoothly toward camera
        Vector3 lookDirection = cameraTransform.position - dog.transform.position;
        lookDirection.y = 0;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            dog.transform.rotation = Quaternion.Slerp(dog.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
