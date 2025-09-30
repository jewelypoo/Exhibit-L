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
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject pauseScreen;

    private float timerTime;
    private float roundedTimer;

    private bool isPaused = false;

    private void Awake()
    {
        timerTime = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.SetActive(true);
        pauseScreen.SetActive(false);
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
                crosshair.SetActive(false);
            }
            else
            {
                artDestroyedCounter.text = "Art destroyed: " + GameManager.Instance.GetArtDestroyed();
            }
        }

        if (GameManager.Instance.paused == true)
        {
            if (!isPaused)
            {
                isPaused = true;
                PauseScreen(GameManager.Instance.paused);
            }
        }
        else
        {
            isPaused = false;
            PauseScreen(GameManager.Instance.paused);
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

    public void PauseScreen(bool activation)
    {
        pauseScreen.SetActive(activation);
        if (activation) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = activation;
    }
}
