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
    private PlayerData playerData;

    private void Awake()
    {
        laser.SetActive(false);
        playerData = GetComponent<PlayerData>();
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
                    if (bounceHit.transform.CompareTag("Player"))
                    {
                        playerData.GameOver();
                    }
                    if (bounceHit.transform.CompareTag("Enemy"))
                    {
                        if (GameManager.Instance.GetEnemyCount() > 0)
                        {
                            GameManager.Instance.ReduceEnemyCount(1);
                        }
                        if (GameManager.Instance.GetEnemyCount() <= 0)
                        {
                            playerData.LevelComplete();
                        }
                        Debug.Log("Enemy Hit");
                    }
                    Debug.Log("Object hit: " + bounceHit.collider.tag);
                }
               
            }
            else if (hit.transform.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);

                if (GameManager.Instance.GetEnemyCount() > 0)
                {
                    GameManager.Instance.ReduceEnemyCount(1);
                }
                if (GameManager.Instance.GetEnemyCount() <= 0)
                {
                    playerData.LevelComplete();
                }
                Debug.Log("Enemy Hit");
            }
            else if (hit.transform.CompareTag("Art"))
            {
                Destroy(hit.collider.gameObject);
                GameManager.Instance.SetArtDestroyed(1);
            }
            
        }
        else
        {
            laser.SetActive(false);
        }
    }
}
