using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text enemyCount;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text endScreenTimer;
    [SerializeField] private TMP_Text artDestroyed;
    [SerializeField] private TMP_Text artDestroyedCounter;

    private float timerTime;
    private float roundedTimer;

    private void Awake()
    {
        timerTime = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetEnemyCount() > 0)
        {
            timerTime += Time.deltaTime;
            roundedTimer = Mathf.Round(timerTime * 100f) / 100f;
            timer.text = "" + roundedTimer;
        }

        if (enemyCount != null)
        {
            enemyCount.text = "Enemies alive: " + GameManager.Instance.GetEnemyCount();
            if (GameManager.Instance.GetEnemyCount() <= 0)
            {
                artDestroyedCounter.text = " ";
                enemyCount.text = " ";
                timer.text = " ";

                endScreenTimer.text = "Final time: " + roundedTimer;
                artDestroyed.text = "Art destroyed: " + GameManager.Instance.GetArtDestroyed();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                artDestroyedCounter.text = "Art destroyed: " + GameManager.Instance.GetArtDestroyed();
            }
        }

        
        

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Retry(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
