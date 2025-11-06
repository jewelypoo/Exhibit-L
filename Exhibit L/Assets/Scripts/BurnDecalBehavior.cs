using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BurnDecalBehavior : MonoBehaviour
{
    public DecalProjector projector;
    public Texture2D[] frames;
    public float frameRate = 10f;

    int currentFrame;
    float timer;
    Material mat;

    void Start()
    {
        projector = GetComponent<DecalProjector>();
        mat = projector.material;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % frames.Length;
            mat.SetTexture("_BaseMap", frames[currentFrame]);
        }
    }
}
