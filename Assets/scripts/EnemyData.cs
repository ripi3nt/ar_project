using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData", order = 0)]
public class EnemyData : ScriptableObject {
    public string enemyName;
    public Sprite icon;
    public GameObject enemyPrefab;
    public int health;
    public int attackPower;
    public RuntimeAnimatorController animator;
}