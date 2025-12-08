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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
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
