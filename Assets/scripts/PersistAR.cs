using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistAR : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}
