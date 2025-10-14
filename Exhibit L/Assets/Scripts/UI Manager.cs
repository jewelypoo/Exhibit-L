using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text enemyCount;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text endScreenTimer;
    [SerializeField] private TMP_Text artDestroyed;
    [SerializeField] private TMP_Text artDestroyedCounter;
    [SerializeField] private TMP_Text endScreenGrade;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private Image hitmarker;
    [SerializeField] private AudioSource hitmarkerSound;

    [SerializeField] private TMP_Text fovSliderNumber;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private TMP_Text sensNumber;
    [SerializeField] private Slider sensSlider;

    [SerializeField] private Button resumeButton;

    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private CinemachineBrain camBrain;
    [SerializeField] private CinemachineInputAxisController cineAxisController;
    [SerializeField] private Button[] levelButtons;

    private bool showHitmarker = false;
    private bool gradeCalculated = false;

    private float timerTime;
    private float roundedTimer;

    public bool isPaused = false;

    private void Awake()
    {
        timerTime = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        crosshair.SetActive(true);
        pauseScreen.SetActive(false);
        hitmarker.gameObject.SetActive(false);

        endScreenGrade.text = "";
        gradeCalculated = false;
        cineAxisController = cam.GetComponent<CinemachineInputAxisController>();

        if (!GameManager.Instance.launched)
        {
            crosshair.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            OpenMainMenu();
            GameManager.Instance.launched = true;
            camBrain.enabled = false;
            GameManager.Instance.paused = true;
            if (fovSlider.value != GameManager.Instance.GetFOV())
            {
                fovSlider.value = 90;
                GameManager.Instance.SetFOV((int)fovSlider.value);
            }
        }
        fovSliderNumber.text = GameManager.Instance.GetFOV().ToString();
        fovSlider.value = GameManager.Instance.GetFOV();
        if (cam.Lens.FieldOfView != GameManager.Instance.GetFOV())
        {
            cam.Lens.FieldOfView = GameManager.Instance.GetFOV();
        }
        SetSensitivity();
    }

    public void SetSensitivity()
    {
        foreach (var axis in cineAxisController.Controllers)
        {
            if (axis.Name == "Look X (Pan)")
                axis.Input.Gain = sensSlider.value;
            else if (axis.Name == "Look Y (Tilt)")
                axis.Input.Gain = -sensSlider.value;
        }
        GameManager.Instance.SetSensitivity(sensSlider.value);

        sensNumber.text = (Mathf.Round(sensSlider.value * 100f) / 100f).ToString();
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
                EndLevel();
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
        else if (GameManager.Instance.paused == false && GameManager.Instance.GetEnemyCount() > 0)
        {
            //Debug.Log("Game is unpaused and this is running");
            isPaused = false;
            PauseScreen(GameManager.Instance.paused);
        }
        
        if (fovSlider.value != GameManager.Instance.GetFOV())
        {
            GameManager.Instance.SetFOV((int)fovSlider.value);
            fovSliderNumber.text = fovSlider.value.ToString();
        }

    }

    public void EndLevel()
    {
        artDestroyedCounter.text = " ";
        enemyCount.text = " ";
        timer.text = " ";
        if (!gradeCalculated)
        {
            endScreenGrade.text = "Grade: " + CalculateGrade();
            gradeCalculated = true;
        }
        endScreenTimer.text = "Final time: " + roundedTimer + " seconds";
        artDestroyed.text = "Art destroyed: " + GameManager.Instance.GetArtDestroyed();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshair.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Retry()
    {
        GameManager.Instance.ResetArtDestroyed();
        SceneManager.LoadScene(GameManager.Instance.GetLevelNumber() - 1);
    }

    public void PauseScreen(bool activation)
    {
        pauseScreen.SetActive(activation);
        resumeButton.enabled = activation;
        resumeButton.interactable = activation;
        
        if (activation) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = activation;
    }

    public void ToggleHitmarkers()
    {
        showHitmarker = !showHitmarker;
    }

    public IEnumerator ShowHitmarker()
    {
        if (showHitmarker)
        {
            hitmarkerSound.Play();
            hitmarker.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hitmarker.gameObject.SetActive(false);
        }
        yield return null;
    }

    private string CalculateGrade()
    {
        int score = 0;
        if (GameManager.Instance.GetArtDestroyed() >= 3)
        {
            return "F";
        }

        if (roundedTimer < GameManager.Instance.GetTimeGoals(1))
        {
            //Debug.Log("Earned Gold");
        }
        else if (roundedTimer < GameManager.Instance.GetTimeGoals(2))
        {
            //Debug.Log("Earned Silver");
            score++;
        }
        else
        {
            //Debug.Log("Earned Bronze");
            score += 2;
        }

        score += GameManager.Instance.GetArtDestroyed();

        switch (score)
        {
            case 0:
                return "S";
            case 1:
                return "A";
            case 2:
                return "B";
            case 3:
                return "C";
            case 4:
                return "D";
            case 5:
                return "F";
            default:
                return "F";
        }
    }

    /// <summary>
    /// Probably temporary, just need something for now until we have a level select
    /// </summary>
    public void NextLevel()
    {
        SceneManager.LoadScene(GameManager.Instance.GetLevelNumber());
        //Debug.Log("loading scene" + GameManager.Instance.GetLevelNumber());
    }

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        if (settingMenu.activeSelf)
        {
            settingMenu.SetActive(false);
        }
        else if (levelSelect.activeSelf)
        {
            levelSelect.SetActive(false);
        }
    }

    public void OpenSettings()
    {
        settingMenu.SetActive(true);
        if (mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
        }
    }

    public void OpenLevelSelect()
    {
        levelSelect.SetActive(true);
        if (mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
        }
        else if (levelCompleteScreen.activeSelf)
        {
            levelCompleteScreen.SetActive(false);
            GameManager.Instance.SetLevelComplete(GameManager.Instance.GetLevelNumber());
        }

            bool[] turnButtonsOn = GameManager.Instance.GetLevelsComplete();
        for (int index = 0; index < levelButtons.Length; ++index)
        {
            levelButtons[index].interactable = turnButtonsOn[index];
        }
    }

    public void LoadScene(int levelNumber)
    {
        if (levelNumber == GameManager.Instance.GetLevelNumber())
        {
            levelSelect.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameManager.Instance.paused = false;
            crosshair.SetActive(true);
            camBrain.enabled = true;
        }
        else
        {
            SceneManager.LoadScene(levelNumber - 1);
        } 
            
    }


}
