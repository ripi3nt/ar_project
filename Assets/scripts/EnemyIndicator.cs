using UnityEngine;

public class EnemyIndicatorSimple : MonoBehaviour
{
    [Tooltip("The circle object under the enemy")]
    public GameObject circleIndicator;

    public void SetIndicatorActive(bool active)
    {
        if (circleIndicator != null)
            circleIndicator.SetActive(active);
    }
}
