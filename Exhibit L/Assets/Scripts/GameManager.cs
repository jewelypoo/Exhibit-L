using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int enemyCount;
    [SerializeField] private int artDestroyed = 0;

    private int currentLevel = 0;

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

    // Update is called once per frame
    void Update()
    {
        
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

    public void SetArtDestroyed(int amountToAdd)
    {
        artDestroyed += amountToAdd;
    }

    public void SetLevelData(int currentLvl, int enemyAmt)
    {
        currentLevel = currentLvl;
        enemyCount = enemyAmt;
    }
}
