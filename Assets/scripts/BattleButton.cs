using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ✅ Needed for scene loading

public class ButtonActive : MonoBehaviour
{
    public Button battleButton;             // Assign this via Inspector
    public float activationDistance = 0.5f; // Distance to activate button
    public Camera arCamera;                 // Assign AR Camera manually!
    public string battleSceneName = "BattleScene"; // ✅ Name of the scene to load

    private Transform player;
    private GameObject closestEnemy;

    void Start()
    {
        // Ensure AR Camera is assigned
        if (arCamera != null)
        {
            player = arCamera.transform;
        }
        else
        {
            Debug.LogError("AR Camera not assigned to ButtonActive script!");
            enabled = false; // Stop script if no camera
            return;
        }

        // Ensure button is assigned
        if (battleButton == null)
        {
            Debug.LogError("Battle Button not assigned in ButtonActive script!");
            enabled = false;
            return;
        }

        battleButton.gameObject.SetActive(false);
        battleButton.onClick.AddListener(StartBattle);
    }

    void Update()
    {
        if (arCamera != null)
        {
            player = arCamera.transform;
        }
        else
        {
            Debug.LogError("AR Camera not assigned to ButtonActive script!");
            enabled = false; // Stop script if no camera
            return;
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDist = Mathf.Infinity;
        closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(player.position, enemy.transform.position);
            if (dist < closestDist && dist < activationDistance)
            {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            Debug.LogError("Dist: " + Vector3.Distance(player.position, closestEnemy.transform.position));
            battleButton.gameObject.SetActive(true);
            battleButton.interactable = true;
        }
        else
        {
            battleButton.gameObject.SetActive(false);
        }
    }

    void StartBattle()
    {
        if (closestEnemy != null)
        {
            Debug.Log("Battle started with: " + closestEnemy.name);

            // Load another scene
            SceneManager.LoadScene(battleSceneName);
        }
        else
        {
            Debug.LogWarning("Tried to start battle but no enemy nearby.");
        }
    }
}