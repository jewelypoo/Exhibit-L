using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AudioClip[] enemyClips;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Chandelier"))
        {
            Vector3 hitPoint = collision.contacts[0].point;

            GameManager.Instance.ReduceEnemyCount(1);

            DestructionHandler handler = GetComponentInChildren<DestructionHandler>();
            handler.StartDestruction();

            GameManager.Instance.PlayImpactAudio(hitPoint, enemyClips, 0.9f, 1.1f);

            Destroy(gameObject);
        }
    }
}
