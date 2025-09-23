using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text enemyCount;
    [SerializeField] private TMP_Text timer;

    private float timerTime;

    private void Awake()
    {
        timerTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timerTime += Time.deltaTime;
        float roundedTimer = Mathf.Round(timerTime * 100f) / 100f;

        timer.text = "" + roundedTimer;
        if (enemyCount != null) 
        {
            enemyCount.text = "Enemies alive: " + GameManager.Instance.GetEnemyCount();
            if (GameManager.Instance.GetEnemyCount() <= 0)
            {
                enemyCount.gameObject.SetActive(false);
            }
        }

    }
}
