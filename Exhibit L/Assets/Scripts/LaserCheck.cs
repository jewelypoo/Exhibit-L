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
        Debug.Log(other.tag);

        if (other.CompareTag("Enemy") || other.CompareTag("Art"))
        {
            lasserBehavior.Laser();
        }
    }

}
