using UnityEngine;
/// <summary>
/// Sharkey, Logan
/// 9/12/2025
/// This will handle all the laser interactions
/// </summary>

public class LasserBehavior : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private LayerMask hitObjects;
    [SerializeField] private GameObject laser;
    //[SerializeField] private GameObject laserChecker;
    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private ParticleSystem smokeParticle;
    [SerializeField] private float degreesDoubleCheckSteps = 2f;

    private float cameraMagnitude;
    private Quaternion lastCameraRotation;
    private Quaternion cameraRoation;

    [SerializeField] private float minCamDoubleCheckDistance;

    private Vector3 laserBounceDir;
    private PlayerData playerData;
    private UIManager uiManager;

    private void Awake()
    {
        laser.SetActive(false);
        playerData = GetComponent<PlayerData>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void LateUpdate()
    {
        cameraRoation = cameraPos.rotation;
        cameraMagnitude = Quaternion.Angle(lastCameraRotation, cameraRoation);

        float steps = Mathf.CeilToInt(cameraMagnitude / degreesDoubleCheckSteps);

        if (cameraMagnitude > minCamDoubleCheckDistance)
        {
            //Debug.Log("Camera moved too fast:" + cameraMagnitude);

            for (int index = 0; index <= steps; ++index)
            {
                float time = (float) index / steps;

                Quaternion stepRotation = Quaternion.Slerp(lastCameraRotation, cameraRoation, time);

                Vector3 rayDir = stepRotation * Vector3.forward;
                //Debug.Log("Double Checking: " + rayDir);
                GameObject hitObj = LaserDoubleCheck(rayDir);
                if (hitObj != null)
                {
                    Destroy(hitObj);
                    break;
                }

            }


        }
    }

    private void Update()
    {
        lastCameraRotation = cameraRoation;
    }

    public void Laser()
    {
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
                        /*fireParticle.transform.position = hit.point;
                        smokeParticle.transform.position = hit.point;
                        fireParticle.Play();
                        smokeParticle.Play();*/
                        StartCoroutine(uiManager.ShowHitmarker());
                        Destroy(bounceHit.collider.gameObject);
                    }
                    if (bounceHit.transform.CompareTag("Player"))
                    {
                        uiManager.GameOver();
                    }
                    if (bounceHit.transform.CompareTag("Enemy"))
                    {
                        Destroy(hit.collider.gameObject);

                        if (GameManager.Instance.GetEnemyCount() > 0)
                        {
                            GameManager.Instance.ReduceEnemyCount(1);

                            StartCoroutine(uiManager.ShowHitmarker());
                        }
                        if (GameManager.Instance.GetEnemyCount() <= 0)
                        {
                            playerData.LevelComplete();
                        }
                        //Debug.Log("Enemy Hit");
                    }
                    //Debug.Log("Object hit: " + bounceHit.collider.tag);
                }

            }
            else if (hit.transform.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);

                if (GameManager.Instance.GetEnemyCount() > 0)
                {
                    GameManager.Instance.ReduceEnemyCount(1);

                    StartCoroutine(uiManager.ShowHitmarker());
                }
                if (GameManager.Instance.GetEnemyCount() <= 0)
                {
                    playerData.LevelComplete();
                }
                //Debug.Log("Enemy Hit");
            }
            else if (hit.transform.CompareTag("Art"))
            {
                Destroy(hit.collider.gameObject);

                GameManager.Instance.AddArtDestroyed(1);
                StartCoroutine(uiManager.ShowHitmarker());
            }
            else if (hit.transform.CompareTag("ChandelierChain"))
            {
                hit.transform.parent.GetComponent<ChandelierBehavior>().DestroyChandelier();
                //Debug.Log("Chains hit!");
            }

        }
        else
        {
            laser.SetActive(false);
        }
    }

    private GameObject LaserDoubleCheck(Vector3 direction)
    {
        // Adjust scale based on raycast
        if (Physics.Raycast(cameraPos.position, direction, out RaycastHit hit, maxDistance, hitObjects))
        {
            if (hit.transform.CompareTag("Mirrror"))
            {
                laserBounceDir = Vector3.Reflect(direction, hit.transform.forward);
                laser.SetActive(true);
                laser.transform.position = hit.point;
                laser.transform.forward = -laserBounceDir;
                if (Physics.Raycast(hit.point, -laserBounceDir, out RaycastHit bounceHit, maxDistance, hitObjects))
                {
                    if (bounceHit.transform.CompareTag("Art"))
                    {
                        /*fireParticle.transform.position = hit.point;
                        smokeParticle.transform.position = hit.point;
                        fireParticle.Play();
                        smokeParticle.Play();*/
                        StartCoroutine(uiManager.ShowHitmarker());
                        Destroy(bounceHit.collider.gameObject);
                    }
                    if (bounceHit.transform.CompareTag("Player"))
                    {
                        uiManager.GameOver();
                    }
                    if (bounceHit.transform.CompareTag("Enemy"))
                    {
                        if (GameManager.Instance.GetEnemyCount() > 0)
                        {
                            GameManager.Instance.ReduceEnemyCount(1);

                            StartCoroutine(uiManager.ShowHitmarker());
                        }
                        if (GameManager.Instance.GetEnemyCount() <= 0)
                        {
                            playerData.LevelComplete();
                        }
                        //Debug.Log("Enemy Hit");
                    }
                    //Debug.Log("Object hit: " + bounceHit.collider.tag);
                }

            }
            else if (hit.transform.CompareTag("Enemy"))
            {
                if (GameManager.Instance.GetEnemyCount() > 0)
                {
                    GameManager.Instance.ReduceEnemyCount(1);

                    StartCoroutine(uiManager.ShowHitmarker());
                }
                if (GameManager.Instance.GetEnemyCount() <= 0)
                {
                    playerData.LevelComplete();
                }
                return hit.collider.gameObject;
                //Debug.Log("Enemy Hit");
            }
            else if (hit.transform.CompareTag("Art"))
            {
                GameManager.Instance.AddArtDestroyed(1);
                StartCoroutine(uiManager.ShowHitmarker());
                return hit.collider.gameObject;
            }
            else if (hit.transform.CompareTag("ChandelierChain"))
            {
                hit.transform.parent.GetComponent<ChandelierBehavior>().DestroyChandelier();
                return hit.collider.gameObject;
                //Debug.Log("Chains hit!");
            }

        }
        else
        {
            laser.SetActive(false);
            return null;
        }
        return null;
    }

    private void FixedUpdate()
    {
        Laser();
    }

}
