using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Chandelier"))
        {
            Destroy(gameObject);
        }
    }
}
