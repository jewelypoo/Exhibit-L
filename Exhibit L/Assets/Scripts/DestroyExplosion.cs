using UnityEngine;
using System.Collections;

public class DestroyExplosion : MonoBehaviour
{
    IEnumerator DestroyAfterParticlesStop()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        yield return new WaitWhile(() => ps.IsAlive());
        Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(DestroyAfterParticlesStop());
    }
}
