using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;


public class InputHandler : MonoBehaviour
{
    public Button resetButton;
    public Button micButton;
    public Button attackButton;
    public Button busyMicButton;


    public ARSession aRSession;
    public MicrophoneRecorder microphone;
    private TextToSpeech tts;
    public TextMeshProUGUI text;

    private GameObject dogObject;
    public PetData petData;
    public Animator dogAnimator;

    //inventory UI
    public Button useButton;
    public Button inventoryButton;
    public GameObject inventoryPanel;
    private bool inventoryVisible = false;
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;

    [Header("Inventory UI Setup")]
    public GameObject petButtonPrefab;   
    public Transform contentParent;      
    private PetData selectedPet;


    void Start()
    {


        tts = new TextToSpeech(this);
        
        //button listeners 
        resetButton.onClick.AddListener(handleReset);
        micButton.onClick.AddListener(handleMicrophone);
        attackButton.onClick.AddListener(() => SceneManager.LoadScene("BattleScene", LoadSceneMode.Single));
        inventoryButton.onClick.AddListener(ToggleInventory);
        useButton.onClick.AddListener(UseSelectedPet);




        // Spawn the first pet in inventory (if any)
        if (InventoryManager.Instance.ownedPets.Count > 0)
        {
            EntityManager.Instance.petData = InventoryManager.Instance.ownedPets[0];
            EntityManager.Instance.SpawnPet();
        }
        else
        {
            Debug.LogWarning("No pets in inventory to spawn!");
        }

        dogObject = EntityManager.Instance.PetInstance;


        //pet animations 
        if (dogObject != null)
        {
            dogAnimator = dogObject.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("No object assigned!");
        }

        /*
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }*/
    }

    //reset
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
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false); 
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

    private void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            bool isActive = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isActive);

            if (isActive)
            {
                PopulateInventoryUI(); 
                useButton.gameObject.SetActive(false); 
            }
        }
    }

    private void PopulateInventoryUI()
    {

        // clear button so we dont get any duplicates 
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // loop through yuor pets 
        foreach (PetData pet in InventoryManager.Instance.ownedPets)
        {
            GameObject newButton = Instantiate(petButtonPrefab, contentParent);

            // Set the buttons image of a pet
            Image img = newButton.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = pet.icon;
            }

            // Add click listener
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectPet(pet));
        }
    }


    private void SelectPet(PetData pet)
    {
        selectedPet = pet;
        useButton.gameObject.SetActive(true);
    }

    public void UseSelectedPet()
    {
        if (selectedPet != null)
        {
            EntityManager.Instance.ReplacePet(selectedPet);
            
            inventoryPanel.SetActive(false);
        }
    }



    public void TestAnimationTrigger(string command)
    {
        handleTranscription(command);
    }
}
