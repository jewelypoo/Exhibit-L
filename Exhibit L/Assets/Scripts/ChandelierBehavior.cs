using System.Collections;
using UnityEngine;

public class ChandelierBehavior : MonoBehaviour
{
    [SerializeField] private GameObject chandChains;
    [SerializeField] private GameObject chandObj;

    private Rigidbody chandRB;

    public float fallenChandLifetime = 5f;

    private void Awake()
    {
        chandRB = chandObj.GetComponent<Rigidbody>();
    }

    public void DestroyChandelier()
    {
        Debug.Log("Destroying Chandelier Chains!");
        Destroy(chandChains);
        chandRB.useGravity = true;
        chandRB.WakeUp();
        StartCoroutine("ChandelierCleanup");
    }

    private IEnumerator ChandelierCleanup()
    {
        Debug.Log("Chandelier falling!");
        float timeElapsed = 0f;

        while (timeElapsed < fallenChandLifetime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Chandelier Destroyed!");
        Destroy(gameObject);
    }

    
}
