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
    [SerializeField] private GameObject explosionParticlePrefab;
    [SerializeField] private float degreesDoubleCheckSteps = 2f;

    // audio stuff here
    [SerializeField] private AudioOneShot audioPrefab;
    [SerializeField] private AudioClip[] artClips;
    [SerializeField] private AudioClip[] statueClips;
    [SerializeField] private AudioClip[] enemyClips;
    [SerializeField] private AudioClip[] chainClips;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;
    [SerializeField] private float volumeSensitivity = 0.02f;
    [SerializeField] private float minAngularSpeed = 5f;
    [SerializeField] private float volumeLerpSpeed = 5f;
    [SerializeField] private float maxLaserMovingVolume = 1f;
    private AudioSource musicSource;
    private AudioSource laserSfxSource;
    private AudioSource laserMovingSfxSource;
    private AudioSource laserImpactSfxSource;

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

        // other sound setup
        var audioManager = LaserAudioManager.Instance;
        if (audioManager != null)
        {
            musicSource = audioManager.music;
            laserSfxSource = audioManager.laserSfx;
            laserMovingSfxSource = audioManager.laserMovingSfx;
            laserImpactSfxSource = audioManager.laserImpactSfx;
        }
    }

    private void LateUpdate()
    {
        cameraRoation = cameraPos.rotation;
        camForward = cameraPos.forward;

        float angularSpeed = cameraMagnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        UpdateCameraMoveSound(angularSpeed);

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
                    if (hitObj.GetComponent<DestructionHandler>() != null)
                    {
                        hitObj.GetComponent<DestructionHandler>().StartDestruction();
                    }
                    else
                    {
                        Debug.Log("No gibs to spawn");
                        Destroy(hitObj);
                    }
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
            if (laserImpactSfxSource != null) // have the laser impact audio play at hit point
            {
                laserImpactSfxSource.transform.position = hit.point;
            }

            Vector3 hitNormal = hit.normal;
            Quaternion lookRotation = Quaternion.LookRotation(-hitNormal);
            lookRotation *= Quaternion.Euler(-90, 0, 0);

            Vector3 spawnPos = hit.point + (-hitNormal * 0.01f);
            float yOffset = Random.Range(-0.02f, 0.02f);
            spawnPos.y += yOffset;

            if (hit.transform.CompareTag("Untagged"))
            {
                Instantiate(burnDecal, spawnPos, lookRotation);
            }

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
                GameManager.Instance.PlayImpactAudio(hit.point, enemyClips, minPitch, maxPitch);
                Instantiate(explosionParticlePrefab, hit.point, Quaternion.identity);
                hit.transform.GetComponent<DestructionHandler>().StartDestruction();
                //Destroy(hit.collider.gameObject);

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
                //create audio prefab
                bool statue = hit.transform.GetComponent<StatueMarker>() != null;

                if (statue)
                    GameManager.Instance.PlayImpactAudio(hit.point, statueClips, minPitch, maxPitch);
                else
                    GameManager.Instance.PlayImpactAudio(hit.point, artClips, minPitch, maxPitch);

                //then destroy
                //Destroy(hit.collider.gameObject);
                hit.transform.GetComponent<DestructionHandler>().StartDestruction();
                dustParticle.transform.position = hit.point;
                dustParticle.Play();

                GameManager.Instance.AddArtDestroyed(1);
                uiManager.UpdateCircles();
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
                    GameManager.Instance.PlayImpactAudio(hit.point, enemyClips, minPitch, maxPitch);
                    Instantiate(explosionParticlePrefab, hit.point, Quaternion.identity);
                    hit.transform.GetComponent<DestructionHandler>().StartDestruction();
                    //Destroy(hit.collider.gameObject);

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
                    //create audio prefab
                    bool statue = hit.transform.GetComponent<StatueMarker>() != null;

                    if (statue)
                        GameManager.Instance.PlayImpactAudio(hit.point, statueClips, minPitch, maxPitch);
                    else
                        GameManager.Instance.PlayImpactAudio(hit.point, artClips, minPitch, maxPitch);
                    GameManager.Instance.AddArtDestroyed(1);
                    StartCoroutine(uiManager.ShowHitmarker());
                    dustParticle.transform.position = hit.transform.position;
                    dustParticle.Play();
                    uiManager.UpdateCircles();
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
                bool statue = bounceHit.transform.GetComponent<StatueMarker>() != null;

                if (statue)
                    GameManager.Instance.PlayImpactAudio(bounceHit.point, statueClips, minPitch, maxPitch);
                else
                    GameManager.Instance.PlayImpactAudio(bounceHit.point, artClips, minPitch, maxPitch);
                GameManager.Instance.AddArtDestroyed(1);
                StartCoroutine(uiManager.ShowHitmarker());
                Destroy(bounceHit.collider.gameObject);
                dustParticle.transform.position = bounceHit.point;
                dustParticle.Play();
                uiManager.UpdateCircles(); 
            }
            if (bounceHit.transform.CompareTag("Player"))
            {
                uiManager.GameOver();
            }
            if (bounceHit.transform.CompareTag("Enemy"))
            {
                GameManager.Instance.PlayImpactAudio(bounceHit.point, enemyClips, minPitch, maxPitch);
                Instantiate(explosionParticlePrefab, bounceHit.point, Quaternion.identity);
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

    private void UpdateCameraMoveSound(float angularSpeed)
    {
        if (!laserMovingSfxSource) return;

        float targetVolume = 0f;

        if (angularSpeed >= minAngularSpeed)
        {
            targetVolume = Mathf.Clamp01(angularSpeed * volumeSensitivity);
            targetVolume *= maxLaserMovingVolume;
        }

        laserMovingSfxSource.volume = Mathf.Lerp(laserMovingSfxSource.volume, targetVolume, Time.deltaTime * volumeLerpSpeed);
    }
}
