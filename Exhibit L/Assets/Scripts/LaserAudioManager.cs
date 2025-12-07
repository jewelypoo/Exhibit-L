using UnityEngine;

public class LaserAudioManager : MonoBehaviour
{
    public static LaserAudioManager Instance { get; private set; }

    public AudioSource music;
    public AudioSource laserSfx;
    public AudioSource laserMovingSfx;
    public AudioSource laserImpactSfx;

    private void Awake()
    {
        Debug.Log("LaserAudioManager Awake on " + gameObject.name + " in scene " + gameObject.scene.name);
        if (Instance != null && Instance != this)
        {
            Debug.Log("LaserAudioManager Awake on " + gameObject.name + " in scene " + gameObject.scene.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("Audio will begin now!");
        double startTime = AudioSettings.dspTime + 0.1f;

        if (music != null)
            music.PlayScheduled(startTime);

        if (laserSfx != null)
            laserSfx.PlayScheduled(startTime);

        if (laserMovingSfx != null)
            laserMovingSfx.PlayScheduled(startTime);

        if (laserImpactSfx != null)
            laserImpactSfx.PlayScheduled(startTime);
    }
}
