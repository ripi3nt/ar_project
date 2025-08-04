using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public PetData selectedPet;
    public EnemyData selectedEnemy;
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
}
