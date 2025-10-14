using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int enemyCount;
    [SerializeField] private int artDestroyed = 0;
    [SerializeField] private int goldTime, silverTimer, bronzeTime;
    [SerializeField] private int fov;
    [SerializeField] private int sens;
    [SerializeField] private bool[] levelsComplete;

    private int currentLevel = 0;

    public bool paused = false;
    public bool launched = false;

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

    public int GetEnemyCount()
    {
        return enemyCount;
    }

    public void ReduceEnemyCount(int amount)
    {
        enemyCount -= amount;
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
        if (input > 1 ||  input < 0.01)
        {
            Debug.LogError("Sensitivity is set out of bounds");
        }
    }

    public void SetLevelComplete(int levelIndex)
    {
        levelsComplete[levelIndex] = true;
    }

    public bool[] GetLevelsComplete()
    {
        return levelsComplete;
    }
}
