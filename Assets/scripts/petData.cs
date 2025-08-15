using UnityEngine;

[CreateAssetMenu(fileName = "NewPet", menuName = "Inventory/Pet")]
public class PetData : ScriptableObject
{
    public string petName;
    public Sprite icon;
    public GameObject petPrefab;
    public int health;
    public int attackPower;
    public RuntimeAnimatorController animator;
    public Vector3 savedPosition;
    public Quaternion savedRotation;
    public AudioClip petSound;
}
