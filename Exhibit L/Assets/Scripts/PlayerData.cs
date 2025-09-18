using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //[SerializeField] private int health = 3;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Debug.Log("Game Over");
            Destroy(gameObject);
        }
    }
}
