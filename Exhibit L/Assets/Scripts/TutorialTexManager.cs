using TMPro;
using UnityEngine;

public class TutorialTexManager : MonoBehaviour
{
    [SerializeField] private TMP_Text tutText;
    [SerializeField] private GameObject tutPanel;
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            tutText.text = " ";
            tutPanel.SetActive(false);
        }
    }
}
