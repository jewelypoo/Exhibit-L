using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private int enemies;

    [SerializeField] private int goldTime, silverTimer, bronzeTime;


    private void Start()
    {
        GameManager.Instance.SetLevelData(level, enemies);

    }

}
