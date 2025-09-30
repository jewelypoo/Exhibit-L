using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private int enemies;

    private void Awake()
    {
        GameManager.Instance.SetLevelData(level, enemies);
    }

}
