using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    private PetData petData;
    private EnemyData enemyData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        petData = BattleManager.Instance.selectedPet;
        enemyData = BattleManager.Instance.selectedEnemy;

        Instantiate(petData.petPrefab);
        Instantiate(enemyData.enemyPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
