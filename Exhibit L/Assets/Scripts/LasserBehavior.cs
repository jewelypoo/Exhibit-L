using UnityEngine;

public class LasserBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform cameraPos;
    [SerializeField]
    private float maxDistance = 100f;
    [SerializeField]
    private LayerMask hitObjects;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cameraPos.rotation * Quaternion.Euler(90f, 0, 0);

        // Adjust scale based on raycast
        if (Physics.Raycast(cameraPos.position, cameraPos.forward, out RaycastHit hit, maxDistance, hitObjects))
        {
            float dist = hit.distance;
            transform.localScale = new Vector3(transform.localScale.x, dist * 0.15f, transform.localScale.z);
            transform.position = cameraPos.position + cameraPos.forward  * 0.5f;
            transform.position = new Vector3(transform.position.x, cameraPos.position.y - 2f, transform.position.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, maxDistance, transform.localScale.z);
            transform.position = cameraPos.position + cameraPos.forward * 0.5f;
            transform.position= new Vector3(transform.position.x, cameraPos.position.y - 2f, transform.position.z);
        }
    }
}
