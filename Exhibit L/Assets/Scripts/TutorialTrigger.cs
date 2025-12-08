using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject tutorialPanel;

    private PlayerController playerController;

    private void Awake()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (tutorialPanel != null && tutorialPanel.activeSelf)
            {
                tutorialPanel.SetActive(false);
                playerController.Pause();
                playerController.tutorialPause = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialPanel.SetActive(true);
            playerController = other.GetComponent<PlayerController>();
            playerController.Pause();
            playerController.tutorialPause = true;
        }
    }
}

