using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AreaScan : MonoBehaviour
{
    public Material highlightArt;
    public Material highlightEnemies;
    public Material highlightDoors;
    public Material highlightFloors;

    [SerializeField] private AudioSource ping;

    private float currentAlpha = 0, fadeSpeed = 2f, targetAlpha = 0f;
    public float maxAlpha;

    public bool toggle = false;
    public bool canToggle = false;

    private float timeActive = 0f;
    public float areaScanActiveTime = 5f;
    public float toggleDelay = 1f;

    private UIManager uiManager;

    private void Awake()
    {
        highlightArt.SetFloat("_ColorIntensity", currentAlpha);
        highlightEnemies.SetFloat("_ColorIntensity", currentAlpha);
        highlightDoors.SetFloat("_ColorIntensity", currentAlpha);
        highlightFloors.SetFloat("_ColorIntensity", currentAlpha);

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void Update()
    {
            if (timeActive < areaScanActiveTime && toggle)
            {
                timeActive += Time.deltaTime;
                canToggle = false;
            }
            else
            {
                canToggle = false;
                toggle = false;
                StartCoroutine(ResetToggle());
                timeActive = 0f;
            }

            targetAlpha = (toggle) ? maxAlpha : 0f;

            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
            highlightArt.SetFloat("_ColorIntensity", currentAlpha);
            highlightEnemies.SetFloat("_ColorIntensity", currentAlpha);
            highlightDoors.SetFloat("_ColorIntensity", currentAlpha);
            highlightFloors.SetFloat("_ColorIntensity", currentAlpha);
    }

    private IEnumerator ResetToggle()
    {
        yield return new WaitForSeconds(toggleDelay);
        canToggle = true;
    }


    public void ToggleAreaScan()
    {
        if (!GameManager.Instance.paused && canToggle && !uiManager.mainMenu.activeSelf)
        {
            toggle = true;
            canToggle = false;
            uiManager.BeginAreaScanCD(areaScanActiveTime + toggleDelay);
            ping.Play();
        }
    }

}
