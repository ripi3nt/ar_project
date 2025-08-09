using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<PetData> ownedPets = new List<PetData>();
    public PetData selectedPet;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddPet(PetData newPet)
    {
        if (!ownedPets.Contains(newPet))
            ownedPets.Add(newPet);
    }

    public void SelectPet(PetData pet)
    {
        selectedPet = pet;
    }
}
