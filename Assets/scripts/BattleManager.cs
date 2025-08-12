using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public GameObject selectedPet;
    public GameObject selectedEnemy;
    public int petHealth;
    public int enemyHealth;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PersistEntities()
    {
        DontDestroyOnLoad(selectedEnemy);
        DontDestroyOnLoad(selectedPet);
    }

}
