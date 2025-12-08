using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //[SerializeField] private int health = 3;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject levelCompleteScreen;

    private UIManager uiManager;


    private void Awake()
    {
        endScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Somthing is inside me O.o");
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("ShieldEnemy"))
        {
            uiManager.GameOver();
        }
    }

    public void LevelComplete()
    {
        levelCompleteScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}

    
