using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        transform.LookAt(player);
    }
}
