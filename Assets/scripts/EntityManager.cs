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
        SpawnPet();
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
        CubePlacement.placed = false;
        CubePlacement.entity = PetInstance;

        Animator animator = PetInstance.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator not found on pet prefab.");
        }
        else
        {
            animator.runtimeAnimatorController = petData.animator;
            Debug.Log("Animator assigned.");
        }
    }


    public void ReplacePet(PetData newData)
    {
        // Remove current pet if it exists
        if (PetInstance != null)
        {
            Destroy(PetInstance);
            PetInstance = null;
        }

        // Set new pet data
        petData = newData;
        SpawnPet();
    }

}