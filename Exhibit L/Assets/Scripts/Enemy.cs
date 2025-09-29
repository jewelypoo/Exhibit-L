using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Chandelier"))
        {
            Destroy(gameObject);
        }
    }
}
