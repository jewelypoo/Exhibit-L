using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BurnDecalBehavior : MonoBehaviour
{
    [Header("Animation Settings")]
    public int totalFrames = 7;    // total number of frames in the sheet
    public float fps = 3.5f;        // playback speed
    public bool destroyOnEnd = true;

    [Header("References")]
    public Renderer targetRenderer; // assign automatically or manually

    private MaterialPropertyBlock mpb;
    private float timer;
    private int currentFrame;

    void Awake()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        mpb = new MaterialPropertyBlock();
    }

    void Update()
    {
        timer += Time.deltaTime * fps;
        int newFrame = Mathf.FloorToInt(timer);

        if (newFrame != currentFrame)
        {
            currentFrame = newFrame;
            if (currentFrame >= totalFrames)
            {
                if (destroyOnEnd)
                {
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    currentFrame = 0; // loop
                }
            }

            // Apply the new frame to the shader
            targetRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Frame", currentFrame);
            targetRenderer.SetPropertyBlock(mpb);
        }
    }
}
