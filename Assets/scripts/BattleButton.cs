using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonActive : MonoBehaviour
{
    public Button battleButton;
    public float activationDistance = 0.5f;     // Distance for battle
    public float minVisibilityDistance = 0.7f;  // Too close → hide
    public float maxVisibilityDistance = 3f;    // Too far → hide
    public Camera arCamera;
    public string battleSceneName = "BattleScene";

    private Transform player;
    private GameObject closestEnemy;

    void Start()
    {
        arCamera = FindObjectsByType<Camera>(FindObjectsSortMode.None)[1];
        // Ensure AR Camera is assigned
        if (arCamera = null)
        {
            Debug.LogError("AR Camera not assigned!");
            enabled = false;
            return;
        }
        player = arCamera.transform;

        if (battleButton == null)
        {
            Debug.LogError("Battle Button not assigned!");
            enabled = false;
            return;
        }

        battleButton.gameObject.SetActive(false);
        battleButton.onClick.AddListener(StartBattle);
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDist = Mathf.Infinity;
        closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(player.position, enemy.transform.position);

            EnemyIndicatorSimple indicator = enemy.GetComponent<EnemyIndicatorSimple>();
            if (indicator != null)
            {
                // Only show the circle if within visibility range
                bool inVisibilityRange = dist >= minVisibilityDistance && dist <= maxVisibilityDistance;
                indicator.SetIndicatorActive(inVisibilityRange);
            }

            // Track the closest enemy for battle activation
            if (dist < closestDist && dist < activationDistance)
            {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }

        // Show battle button if in range
        if (closestEnemy != null)
        {
            battleButton.gameObject.SetActive(true);
            battleButton.interactable = true;
            BattleManager.Instance.selectedEnemy = closestEnemy;
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
            BattleManager.Instance.PersistEntities();

            // Load another scene
            SceneManager.LoadScene(battleSceneName, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("novime");
        }
        else
        {
            Debug.LogWarning("No enemy nearby to battle.");
        }
    }
}
