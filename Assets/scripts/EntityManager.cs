using UnityEngine;

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
        if (PetInstance != null) return;

        if (petData == null || petData.petPrefab == null)
        {
            Debug.LogError("PetData is missing!");
            return;
        }

        PetInstance = Instantiate(petData.petPrefab);
        PetInstance.SetActive(false); // Will be positioned by CubePlacement
        Animator animator = PetInstance.GetComponent<Animator>();
        animator.runtimeAnimatorController = petData.animator;
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