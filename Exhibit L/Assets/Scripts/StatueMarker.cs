using UnityEngine;

public class StatueMarker : MonoBehaviour
{
    [SerializeField] private AudioClip[] statueClips;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Chandelier"))
        {
            Vector3 hitPoint = collision.contacts[0].point;

            DestructionHandler handler = GetComponentInChildren<DestructionHandler>();

            handler.StartDestruction();

            GameManager.Instance.AddArtDestroyed(1);

            GameManager.Instance.PlayImpactAudio(hitPoint, statueClips, 0.9f, 1.1f);

            Destroy(gameObject);
        }
    }
}
