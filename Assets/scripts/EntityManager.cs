using UnityEngine;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour {
    public PetData petData;
    public static EntityManager Instance { get; private set; }
    public GameObject PetInstance;
     
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }

    public void SpawnPet()
    {
        if (PetInstance != null)
        {
            //Debug.Log("Pet already spawned.");
            return;
        }

        if (petData == null || petData.petPrefab == null)
        {
            Debug.LogError("PetData or petPrefab is missing!");
            return;
        }

        PetInstance = Instantiate(petData.petPrefab);
        Debug.Log("Pet has been instantiated: " + PetInstance.name);

        PetInstance.SetActive(false); // Will be activated later by CubePlacement

        Animator animator = PetInstance.GetComponent<Animator>();
        if (animator == null)
        {
            //Debug.LogWarning("Animator not found on pet prefab.");
        }
        else
        {
            animator.runtimeAnimatorController = petData.animator;
            Debug.Log("Animator assigned.");
        }
    }


    public void ReplacePet(PetData newData)
    {
        if (PetInstance != null)
        {
            Destroy(PetInstance);
        }

        petData = newData;
        SpawnPet();
    }
}