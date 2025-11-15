using UnityEngine;

public class MirrorTrigger : MonoBehaviour
{
    public GameObject Mirror;

    private void Awake()
    {
        Mirror.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Mirror.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Mirror.SetActive(false);
        }
    }

}
