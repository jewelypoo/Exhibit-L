using NUnit.Framework;
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
    [SerializeField] private ParticleSystem dustParticle;
    [SerializeField] private ParticleSystem smokeParticle;
    [SerializeField] private float degreesDoubleCheckSteps = 2f;

    private float cameraMagnitude;
    private Quaternion lastCameraRotation;
    private Quaternion cameraRoation;

    [SerializeField] private float minCamDoubleCheckDistance;

    //private Vector3 laserBounceDir;
    private PlayerData playerData;
    private UIManager uiManager;

    [SerializeField] private int maxMirrorBounces = 5;
    private int currentMirrorBounces = 0;
    public GameObject burnDecal;

    public Vector3 camForward;


    private void Awake()
    {
        laser.SetActive(false);
        playerData = GetComponent<PlayerData>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void LateUpdate()
    {
        cameraRoation = cameraPos.rotation;
        camForward = cameraPos.forward;
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
        
        if (Physics.Raycast(cameraPos.position, cameraPos.forward, out RaycastHit hit, maxDistance, hitObjects))
        {
            smokeParticle.transform.position = hit.point;

            Vector3 hitNormal = hit.normal;
            Quaternion lookRotation = Quaternion.LookRotation(-hitNormal);
            lookRotation *= Quaternion.Euler(-90, 0, 0);

            Vector3 spawnPos = hit.point + (-hitNormal * 0.01f); 
            float yOffset = Random.Range(-0.02f, 0.02f);
            spawnPos.y += yOffset;

            Instantiate(burnDecal, spawnPos, lookRotation);

            if (hit.transform.CompareTag("Mirrror"))
            {
                laser.SetActive(true);
                laser.transform.position = hit.point;
                Vector3 laserBounceDirTemp = Vector3.Reflect(cameraPos.forward, hit.normal);
                laser.transform.forward = laserBounceDirTemp;
                MirrorBounce(hit, laserBounceDirTemp);

            }
            else if (laser.activeSelf)
            {
                laser.SetActive(false);
                currentMirrorBounces = 0;
            }
            if (hit.transform.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);

                if (GameManager.Instance.GetEnemyCount() > 0)
                {
                    GameManager.Instance.ReduceEnemyCount(1);

                    StartCoroutine(uiManager.ShowHitmarker());
                }
                if (GameManager.Instance.GetEnemyCount() <= 0)
                {
                    //playerData.LevelComplete();
                }
                //Debug.Log("Enemy Hit");
            }
            else if (hit.transform.CompareTag("Art"))
            {
                Destroy(hit.collider.gameObject);
                dustParticle.transform.position = hit.point;
                dustParticle.Play();

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
            if (!hit.transform.CompareTag("Player"))
            {
                Vector3 hitNormal = hit.normal;
                Quaternion lookRotation = Quaternion.LookRotation(-hitNormal);
                lookRotation *= Quaternion.Euler(-90, 0, 0);

                Vector3 spawnPos = hit.point + (-hitNormal * 0.01f);
                float yOffset = Random.Range(-0.02f, 0.02f);
                spawnPos.y += yOffset;

                Instantiate(burnDecal, spawnPos, lookRotation);

                if (hit.transform.CompareTag("Mirrror"))
                {
                    laser.SetActive(true);
                    laser.transform.position = hit.point;
                    Vector3 laserBounceDirTemp = Vector3.Reflect(direction, hit.normal);
                    laser.transform.forward = laserBounceDirTemp;
                    MirrorBounce(hit, laserBounceDirTemp);

                }
                else if (laser.activeSelf)
                {
                    laser.SetActive(false);
                }
                if (hit.transform.CompareTag("Enemy"))
                {
                    Destroy(hit.collider.gameObject);

                    if (GameManager.Instance.GetEnemyCount() > 0)
                    {
                        GameManager.Instance.ReduceEnemyCount(1);

                        StartCoroutine(uiManager.ShowHitmarker());
                    }
                    if (GameManager.Instance.GetEnemyCount() <= 0)
                    {
                        //playerData.LevelComplete();
                    }
                    return hit.collider.gameObject;
                    //Debug.Log("Enemy Hit");
                }
                else if (hit.transform.CompareTag("Art"))
                {
                    GameManager.Instance.AddArtDestroyed(1);
                    StartCoroutine(uiManager.ShowHitmarker());
                    dustParticle.transform.position = hit.transform.position;
                    dustParticle.Play();
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
        }
            
        return null;
    }

    private void FixedUpdate()
    {
        Laser();
    }

    private void MirrorBounce(RaycastHit hit, Vector3 laserBounceDir)
    {
        
        if (Physics.Raycast(hit.point + (laserBounceDir * 0.01f), laserBounceDir, out RaycastHit bounceHit, maxDistance, hitObjects))
        {
            if (bounceHit.transform.CompareTag("Art"))
            {
                /*fireParticle.transform.position = hit.point;
                smokeParticle.transform.position = hit.point;
                fireParticle.Play();
                smokeParticle.Play();*/
                StartCoroutine(uiManager.ShowHitmarker());
                Destroy(bounceHit.collider.gameObject);
                dustParticle.transform.position = bounceHit.point;
                dustParticle.Play();
            }
            if (bounceHit.transform.CompareTag("Player"))
            {
                uiManager.GameOver();
            }
            if (bounceHit.transform.CompareTag("Enemy"))
            {
                Destroy(bounceHit.collider.gameObject);

                if (GameManager.Instance.GetEnemyCount() > 0)
                {
                    GameManager.Instance.ReduceEnemyCount(1);

                    StartCoroutine(uiManager.ShowHitmarker());
                }
                if (GameManager.Instance.GetEnemyCount() <= 0)
                {
                    //playerData.LevelComplete();
                }
                //Debug.Log("Enemy Hit");
            }
            if (bounceHit.transform.CompareTag("Mirrror"))
            {
                /*Vector3 laserBounceDirTemp = Vector3.Reflect(laserBounceDir, hit.normal);
                MirrorBounce(bounceHit, laserBounceDir);*/
            }
            //Debug.Log("Object hit: " + bounceHit.collider.tag);
        }
    }
}
