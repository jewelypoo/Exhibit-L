using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AreaScan : MonoBehaviour
{
    public Material highlightArt;
    public Material highlightEnemies;

    private float currentAlpha = 0, fadeSpeed = 2f, targetAlpha = 0f;
    public float maxAlpha;



    public bool toggle = false;

    private void Awake()
    {
        highlightArt.SetFloat("_ColorIntensity", currentAlpha);
        highlightEnemies.SetFloat("_ColorIntensity", currentAlpha);
    }

    private void Update()
    {
        targetAlpha = (toggle) ? maxAlpha : 0f;

        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
        highlightArt.SetFloat("_ColorIntensity", currentAlpha);
        highlightEnemies.SetFloat("_ColorIntensity", currentAlpha);

    }


    public void ToggleAreaScan()
    {
        toggle = !toggle;
    }

}
