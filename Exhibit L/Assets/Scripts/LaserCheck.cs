using UnityEngine;

public class LaserCheck : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
    private LasserBehavior lasserBehavior;

    private void Awake()
    {
        lasserBehavior = playerRef.GetComponent<LasserBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Art"))
        {
            LaserTriggered(other.gameObject);
        }
    }

    public GameObject LaserTriggered(GameObject obj)
    {
        return obj;
    }
}
