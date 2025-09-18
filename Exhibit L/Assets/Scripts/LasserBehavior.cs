using UnityEngine;
/// <summary>
/// Sharkey, Logan
/// 9/12/2025
/// This will handle all the laser interactions
/// </summary>

public class LasserBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform cameraPos;
    [SerializeField]
    private float maxDistance = 100f;
    [SerializeField]
    private LayerMask hitObjects;
    [SerializeField]
    private GameObject laser;

    private Vector3 laserBounceDir;

    private void Awake()
    {
        laser.SetActive(false);
    }




    // Update is called once per frame
    void Update()
    {
        //transform.rotation = cameraPos.rotation * Quaternion.Euler(90f, 0, 0);

        // Adjust scale based on raycast
        if (Physics.Raycast(cameraPos.position, cameraPos.forward, out RaycastHit hit, maxDistance, hitObjects))
        {
            if (hit.transform.CompareTag("Mirrror"))
            {
                laserBounceDir = Vector3.Reflect(cameraPos.forward, hit.transform.forward);
                laser.SetActive(true);
                laser.transform.position = hit.point;
                laser.transform.forward = -laserBounceDir;
                if (Physics.Raycast(hit.point, -laserBounceDir, out RaycastHit bounceHit, maxDistance, hitObjects))
                {
                    if (bounceHit.transform.CompareTag("Art"))
                    {
                        Destroy(bounceHit.collider.gameObject);
                    }
                }
            }
            else
            {
                Destroy(hit.collider.gameObject);
            }
            
        }
        else
        {
            laser.SetActive(false);
        }
    }
}
