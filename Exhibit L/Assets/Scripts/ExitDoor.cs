using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private UIManager uiManager;

    private void Awake()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.GetEnemyCount() <= 0)
            {
                other.GetComponent<PlayerData>().LevelComplete();
                uiManager.EndLevel();
            }
        }
    }


}
