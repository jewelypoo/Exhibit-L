using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //[SerializeField] private int health = 3;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject levelCompleteScreen;


    private void Awake()
    {
        endScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Somthing is inside me O.o");
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("ShieldEnemy"))
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        endScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LevelComplete()
    {
        levelCompleteScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
