using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [SerializeField] private int enemyCount;
    [SerializeField] private int artDestroyed = 0;
    [SerializeField] private int goldTime, silverTimer, bronzeTime;
    [SerializeField] private int fov;
    [SerializeField] private float sens;
    [SerializeField] private int masterVolume;
    [SerializeField] private int musicVolume;
    [SerializeField] private int sfxVolume;
    [SerializeField] private bool[] levelsComplete;
    [SerializeField] public AudioOneShot audioPrefab;

    private Animator Door;

    private int currentLevel = 0;

    public bool paused = false;
    public bool launched = false;
    public bool mainMenuActive = true;
    public bool levelSelectActive = false;
    public bool settingsActive = false;
    public bool creditsActive = false;
    public bool startLevelOne = false;

 
    private void Start()
    {
        Door = FindFirstObjectByType<Animator>();
        if (Door == null )
        {
            Debug.Log("error: door not found");
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        for (int index = 0; index < levelsComplete.Length; ++index)
        {
            if (index == 0 || index == GetLevelNumber() - 1)
            {
                levelsComplete[index] = true;
            }
            else
            {
                levelsComplete[index] = false;
            }
        }

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }

    public void ReduceEnemyCount(int amount)
    {
        enemyCount -= amount;
        if (Door == null)
            Door = FindFirstObjectByType<Animator>();

        if (enemyCount <= 0)
        {
            if (Door != null)
            {
                Door.SetBool("Open", true);
            }
            else
            {
                Debug.LogError("Door Animator not found in scene!");
            }
        }
    }

    public int GetArtDestroyed()
    {
        return artDestroyed;
    }

    public void AddArtDestroyed(int amountToAdd)
    {
        artDestroyed += amountToAdd;
    }

    public void ResetArtDestroyed()
    {
        artDestroyed = 0;
    }

    public void SetLevelData(int currentLvl, int enemyAmt)
    {
        currentLevel = currentLvl;
        enemyCount = enemyAmt;
    }

    public int GetLevelNumber()
    {
        return currentLevel;
    }

    public void Pause(bool result)
    {
        paused = result;
    }

    public void SetTimeGoals(int gold, int silver, int bronze)
    {
        goldTime = gold; silverTimer = silver; bronzeTime = bronze;
    }

    public int GetTimeGoals(int place)
    {
        switch (place)
        {
            case 1:
                return goldTime;

            case 2:
                return silverTimer;

            case 3:
                return bronzeTime;

            default:
                return 0;
        }
    }

    public int GetFOV()
    {
        return fov;
    }

    public void SetFOV(int newFOV)
    {
        fov = newFOV;
    }

    public float GetSensitivity()
    {
        return sens;
    }

    public void SetSensitivity(float input)
    {
        sens = input;
    }

    public void SetLevelComplete(int levelIndex, bool passed)
    {
        levelsComplete[levelIndex] = passed;
    }

    public bool GetLevelsComplete(int index)
    {
        return levelsComplete[index];
    }

    public bool[] GetLevelsComplete()
    {
        return levelsComplete;
    }

    public void SetMasterVolume(int value)
    {
        masterVolume = value;
    }

    public void SetSFXVolume(int value)
    {
        sfxVolume = value;
    }

    public void SetMusicVolume(int value)
    {
        musicVolume = value;
    }

    public int GetMasterVolume()
    {
        return masterVolume;
    }

    public int GetSFXVolume()
    {
        return sfxVolume;
    }

    public int GetMusicVolume()
    {
        return musicVolume;
    }

    public void PlayImpactAudio(Vector3 position, AudioClip[] list, float minPitch, float maxPitch)
    {
        if (list == null || list.Length == 0) return;

        AudioClip clip = list[Random.Range(0, list.Length)];
        float pitch = Random.Range(minPitch, maxPitch);

        AudioOneShot audio = Instantiate(audioPrefab, position, Quaternion.identity);
        audio.PlayClip(clip, pitch);
    }
}
