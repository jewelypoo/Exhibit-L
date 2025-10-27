using System.Collections;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    [SerializeField] private GameObject mainMenuBackground;
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private Image hitmarker;
    [SerializeField] private AudioSource hitmarkerSound;
    [SerializeField] private Image areaScanBackground;
    [SerializeField] private TMP_Text areaScanCD;

    [SerializeField] private TMP_Text fovSliderNumber;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private TMP_Text sensNumber;
    [SerializeField] private Slider sensSlider;

    [SerializeField] private TMP_Text masterSliderNumber;
    [SerializeField] private TMP_Text sfxSliderNumber;
    [SerializeField] private TMP_Text musicSliderNumber;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button levelSelectButton;

    [SerializeField] private AudioMixer mixer;

    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private CinemachineBrain camBrain;
    [SerializeField] private CinemachineInputAxisController cineAxisController;
    [SerializeField] private Button[] levelButtons;

    private bool showHitmarker = false;
    private bool gradeCalculated = false;
    private bool areaScanCDStarted = false;
    private float areaScanCDSeconds = 0f;

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

        areaScanBackground.color = Color.white;
        areaScanBackground.gameObject.SetActive(true);
        areaScanCD.text = "";

        masterSlider.value = GameManager.Instance.GetMasterVolume();
        sfxSlider.value = GameManager.Instance.GetSFXVolume();
        musicSlider.value = GameManager.Instance.GetMusicVolume();
        SetMasterVolume();
        SetSFXVolume();
        SetMusicVolume();

        fovSlider.value = GameManager.Instance.GetFOV();
        sensSlider.value = GameManager.Instance.GetSensitivity();

        

        if (!GameManager.Instance.launched)
        {
            crosshair.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            OpenMainMenu();
            GameManager.Instance.launched = true;
            camBrain.enabled = false;
            GameManager.Instance.paused = true;

            areaScanBackground.color = Color.white;
            areaScanBackground.gameObject.SetActive(false);
            areaScanCD.text = "";

            fovSlider.value = 90;
            SetFOV();

            sensSlider.value = 1;
            SetSensitivity();

            masterSlider.value = 80f;
            sfxSlider.value = 80f;
            musicSlider.value = 80f;

            SetMasterVolume();
            SetSFXVolume();
            SetMusicVolume();
        }
        fovSliderNumber.text = GameManager.Instance.GetFOV().ToString();
        fovSlider.value = GameManager.Instance.GetFOV();
        if (cam.Lens.FieldOfView != GameManager.Instance.GetFOV())
        {
            cam.Lens.FieldOfView = GameManager.Instance.GetFOV();
        }
        SetSensitivity();
        if (levelSelect.activeSelf)
        {
            levelSelect.SetActive(false);
            GameManager.Instance.levelSelectActive = false;
        }

        GameManager.Instance.ResetArtDestroyed();
    }

    public void SetSensitivity()
    {
        {
            foreach (var axis in cineAxisController.Controllers)
            {
                if (axis.Name == "Look X (Pan)")
                {
                    axis.Input.Gain = sensSlider.value;
                    //print("sensSlider.value is" + axis.Input.Gain);
                }
                   
                else if (axis.Name == "Look Y (Tilt)")
                {
                    axis.Input.Gain = -sensSlider.value;
                   //print("sensSlider.value is" + axis.Input.Gain);
                }
                    
            }
            GameManager.Instance.SetSensitivity(sensSlider.value);

            sensNumber.text = (Mathf.Round(sensSlider.value * 100f) / 100f).ToString();
        } 
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

        if (areaScanCDStarted)
        {
            areaScanCD.text = ((int)areaScanCDSeconds).ToString();
            areaScanCDSeconds -= Time.deltaTime;
            if (areaScanCDSeconds <= 1)
            {
                areaScanCDStarted = false;
                areaScanBackground.color = Color.white;
                areaScanCD.text = "";
            }
        }

    }

    public void EndLevel()
    {
        Debug.Log("Level end reached");
        artDestroyedCounter.text = " ";
        enemyCount.text = " ";
        timer.text = " ";
        if (!gradeCalculated)
        {
            Debug.Log("Scoring Player");
            endScreenGrade.text = "Grade: " + CalculateGrade();
            CalculateGrade();
            Debug.Log("Grade calculated");
            gradeCalculated = true;
        }
        endScreenTimer.text = "Final time: " + roundedTimer + " seconds";
        artDestroyed.text = "Art destroyed: " + GameManager.Instance.GetArtDestroyed();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshair.SetActive(false);
        areaScanBackground.gameObject.SetActive(false);
        areaScanCD.text = "";
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        artDestroyedCounter.text = " ";
        enemyCount.text = " ";
        timer.text = " ";

        endScreen.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Cursor Visible");

        crosshair.SetActive(false);
        areaScanBackground.gameObject.SetActive(false);
        areaScanCD.text = "";
        Time.timeScale = 0f;

        Debug.Log("GameOver() is done running");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Retry()
    {
        //print("retrying");
        if (Time.timeScale <= 0f)
        {
            //print("timescale reset");
            Time.timeScale = 1f;
        }
        GameManager.Instance.levelSelectActive = false;
        GameManager.Instance.mainMenuActive = false;
        GameManager.Instance.ResetArtDestroyed();
        SceneManager.LoadScene(GameManager.Instance.GetLevelNumber() - 1);
        
    }

    public void PauseScreen(bool activation)
    {
        if (!endScreen.activeSelf && !endScreen.activeSelf && !levelSelect.activeSelf)
        {
            pauseScreen.SetActive(activation);
            resumeButton.enabled = activation;
            resumeButton.interactable = activation;

            if (activation) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = activation;
        }
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
            score += 10;
        }

        if (roundedTimer < GameManager.Instance.GetTimeGoals(1))
        {
            Debug.Log("Earned Gold");
        }
        else if (roundedTimer < GameManager.Instance.GetTimeGoals(2))
        {
            Debug.Log("Earned Silver");
            score++;
        }
        else
        {
            Debug.Log("Earned Bronze");
            score += 2;
        }

        score += GameManager.Instance.GetArtDestroyed();

        Debug.Log(score + " running scoring handler");

        switch (score)
        {
            case 0:
                levelSelectButton.interactable = true;
                GameManager.Instance.SetLevelComplete(GameManager.Instance.GetLevelNumber(), true);
                Debug.Log("Level passed");
                return "S";
            case 1:
                levelSelectButton.interactable = true;
                GameManager.Instance.SetLevelComplete(GameManager.Instance.GetLevelNumber(), true);
                Debug.Log("Level passed");
                return "A";
            case 2:
                levelSelectButton.interactable = true;
                GameManager.Instance.SetLevelComplete(GameManager.Instance.GetLevelNumber(), true);
                Debug.Log("Level passed");
                return "B";
            case 3:
                levelSelectButton.interactable = true;
                GameManager.Instance.SetLevelComplete(GameManager.Instance.GetLevelNumber(), true);
                Debug.Log("Level passed");
                return "C";
            case 4:
                if (!GameManager.Instance.GetLevelsComplete(GameManager.Instance.GetLevelNumber()))
                {
                    levelSelectButton.interactable = false;
                    GameManager.Instance.SetLevelComplete(GameManager.Instance.GetLevelNumber(), false);
                    Debug.Log("Did not pass level");
                }
                else
                {
                    levelSelectButton.interactable = true;
                    Debug.Log("Level already passed");
                }
                    return "D";
            case 5:
                if (!GameManager.Instance.GetLevelsComplete(GameManager.Instance.GetLevelNumber()))
                {
                    levelSelectButton.interactable = false;
                    GameManager.Instance.SetLevelComplete(GameManager.Instance.GetLevelNumber(), false);
                    Debug.Log("Did not pass level");
                }
                else
                {
                    levelSelectButton.interactable = true;
                    Debug.Log("Level already passed");
                }
                return "F";
            default:
                if (!GameManager.Instance.GetLevelsComplete(GameManager.Instance.GetLevelNumber()))
                {
                    levelSelectButton.interactable = false;
                    GameManager.Instance.SetLevelComplete(GameManager.Instance.GetLevelNumber(), false);
                    Debug.Log("Did not pass level");
                }
                else
                {
                    levelSelectButton.interactable = true;
                    Debug.Log("Level already passed");
                }
                return "F";
        }
    }

    /// <summary>
    /// Probably temporary, just need something for now until we have a level select
    /// </summary>
    public void NextLevel()
    {
        GameManager.Instance.mainMenuActive = false;
        GameManager.Instance.levelSelectActive = false;
        SceneManager.LoadScene(GameManager.Instance.GetLevelNumber());
        Debug.Log("loading scene" + GameManager.Instance.GetLevelNumber());
    }

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        Time.timeScale = 1f;
        camBrain.enabled = false;
        areaScanCD.gameObject.SetActive(false);
        GameManager.Instance.mainMenuActive = true;
        if (settingMenu.activeSelf)
        {
            settingMenu.SetActive(false);
        }
        else if (levelSelect.activeSelf)
        {
            levelSelect.SetActive(false);
            GameManager.Instance.levelSelectActive = false;
        }
    }

    public void OpenSettings()
    {
        areaScanCD.gameObject.SetActive(false);
        settingMenu.SetActive(true);
        if (mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
        }
    }

    public void OpenLevelSelect()
    {
        areaScanCD.gameObject.SetActive(false);
        levelSelect.SetActive(true);
        GameManager.Instance.levelSelectActive = true;
        if (mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
            GameManager.Instance.mainMenuActive = false;
        }
        else if (levelCompleteScreen.activeSelf)
        {
            levelCompleteScreen.SetActive(false);
            GameManager.Instance.SetLevelComplete(GameManager.Instance.GetLevelNumber(), true);
        }

        bool[] turnButtonsOn = GameManager.Instance.GetLevelsComplete();
        for (int index = 0; index < levelButtons.Length; ++index)
        {
            levelButtons[index].interactable = turnButtonsOn[index];
        }
    }

    public void LoadScene(int levelNumber)
    {
        //print("loading scene");
        
        Time.timeScale = 1f;
        GameManager.Instance.mainMenuActive = false;
        GameManager.Instance.levelSelectActive = false;
        if (levelNumber == GameManager.Instance.GetLevelNumber())
        {
            //print("loading this scene");
            levelSelect.SetActive(false);
            GameManager.Instance.levelSelectActive = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameManager.Instance.paused = false;
            crosshair.SetActive(true);
            camBrain.enabled = true;
            areaScanBackground.gameObject.SetActive(true);
            areaScanCD.gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(levelNumber - 1);
        } 
            
    }

    public void BeginAreaScanCD(float seconds)
    {
        areaScanCDSeconds = seconds;
        areaScanCDStarted = true;
        areaScanBackground.color = Color.gray;
    }

    public void SetMasterVolume()
    {
        masterSliderNumber.text = masterSlider.value.ToString();
        mixer.SetFloat("MasterVol", masterSlider.value - 80);
        GameManager.Instance.SetMasterVolume((int)masterSlider.value);
    }

    public void SetSFXVolume()
    {
        sfxSliderNumber.text = sfxSlider.value.ToString();
        mixer.SetFloat("SFXVol", sfxSlider.value - 80);
        GameManager.Instance.SetSFXVolume((int)sfxSlider.value);
    }

    public void SetMusicVolume()
    {
        musicSliderNumber.text = musicSlider.value.ToString();
        mixer.SetFloat("MusicVol", musicSlider.value - 80);
        
        GameManager.Instance.SetMusicVolume((int)musicSlider.value);
    }

    public void SetFOV()
    {
        fovSliderNumber.text = fovSlider.value.ToString();
        cam.Lens.FieldOfView = fovSlider.value;
        GameManager.Instance.SetFOV((int)fovSlider.value);
    }


}
