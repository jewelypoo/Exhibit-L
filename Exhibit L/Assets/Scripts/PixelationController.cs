using UnityEngine;

public class PixelationController : MonoBehaviour
{
    public RenderTexture pixelRT;
    public int baseWidth = 320;
    public int baseHeight = 180;

    public Camera mainCam;
    public Camera pixelCam; // Secondary camera

    [Range(1, 10)]
    public int pixelationLevel = 1;
    public bool pixelationEnabled = true;

    private void Start()
    {
        ApplyState();
    }

    void LateUpdate()
    {
        if (pixelationEnabled)
        {
            // Match mainCam's transform to follow Cinemachine Brain
            pixelCam.transform.position = mainCam.transform.position;
            pixelCam.transform.rotation = mainCam.transform.rotation;
        }
    }
    public void SetPixelationLevel(int level)
    {
        pixelationLevel = Mathf.Clamp(level, 1, 10);
        Debug.Log("PIXEL ON");
        pixelationEnabled = true;
        ApplyState();
    }

    public void TogglePixelation(bool enabled)
    {
        pixelationEnabled = enabled;
        Debug.Log("PIXEL start");

        ApplyState();
    }

    void ApplyState()
    {
        if (pixelationEnabled)
        {
            pixelRT.Release();
            pixelRT.width = baseWidth/pixelationLevel;
            pixelRT.height = baseHeight/pixelationLevel;
            pixelRT.Create();

            mainCam.targetTexture = pixelRT;
        }
        else
        {
            mainCam.targetTexture = null;
        }
    }
}
