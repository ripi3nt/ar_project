using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject petButtonPrefab; 
    public Transform contentParent;    
    public Button useButton;


    

    private void OnEnable()
    {
        BuildInventory();
        useButton.gameObject.SetActive(false);
    }

    public void BuildInventory()
    {
        // Clear old buttons
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (PetData pet in InventoryManager.Instance.ownedPets)
        {
            GameObject newButton = Instantiate(petButtonPrefab, contentParent);
            newButton.GetComponent<Image>().sprite = pet.icon;

            newButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                InventoryManager.Instance.SelectPet(pet);
                useButton.gameObject.SetActive(true);
            });
        }
    }

    public void OnUsePet()
    {
        if (InventoryManager.Instance.selectedPet != null)
        {
            EntityManager.Instance.ReplacePet(InventoryManager.Instance.selectedPet);
            inventoryPanel.SetActive(false);
        }
    }
}
