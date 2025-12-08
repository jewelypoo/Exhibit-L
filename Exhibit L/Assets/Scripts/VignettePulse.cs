using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignettePulse : MonoBehaviour
{
    public float minValue = 0.3f;
    public float maxValue = 0.4f;
    public float speed = 0.5f;

    private Vignette vignette;
    private float target;

    void Start()
    {
        Volume volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);

        target = maxValue;
    }

    void Update()
    {
        if (vignette == null) return;

        vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, target, speed * Time.deltaTime);

        if (Mathf.Approximately(vignette.intensity.value, target))
        {
            if (target == maxValue)
                target = minValue;
            else
                target = maxValue;
        }
    }
}
