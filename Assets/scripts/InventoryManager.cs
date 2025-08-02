using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<PetData> ownedPets = new List<PetData>();

    public void AddPet(PetData newPet)
    {
        ownedPets.Add(newPet);
    }

    public void RemovePet(PetData pet)
    {
        ownedPets.Remove(pet);
    }
}