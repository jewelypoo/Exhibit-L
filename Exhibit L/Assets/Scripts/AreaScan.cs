using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AreaScan : MonoBehaviour
{
    public Material highlightArt;
    public Material highlightEnemies;

    private float targetAlpha = 0, currentAlpha = 0, currentAlphaTwo = 0, fadeSpeed = 2f;

    public bool toggle = false;
    //Color c;
    //Color cTwo;
    private void Awake()
    {
        //c.a = 0;
        //cTwo.a = 0;

        //highlightArt.SetColor("_BaseColor", c);
        //highlightEnemies.SetColor("_BaseColor", cTwo);
    }

    private void Update()
    {

        if (toggle)
        {
            targetAlpha = (targetAlpha < 1f) ? 1f : targetAlpha;
        }
        

        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
        currentAlphaTwo = Mathf.MoveTowards(currentAlphaTwo, targetAlpha, fadeSpeed * Time.deltaTime);
        Color c = highlightArt.GetColor("_BaseColor");
        Color cTwo = highlightEnemies.GetColor("_BaseColor");

        c.a = currentAlpha;
        cTwo.a = currentAlphaTwo;

        highlightArt.SetColor("_BaseColor", c);
        highlightEnemies.SetColor("_BaseColor", cTwo);

    }


    public void ToggleAreaScan()
    {
        toggle = (toggle) ? false : true;
    }

}
