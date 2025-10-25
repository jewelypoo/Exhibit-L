using System.Collections;
using UnityEngine;
/*
Ian Iversen-Krampitz
10/24/2025
controls the main menu camera turn
*/
public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private bool turnAround;
    [SerializeField] private float maxRotation;
    [SerializeField] private float cameraPauseTime;
    [SerializeField] private float defaultRotation;
    public float rotation;
    public float counter;
    public bool firstFlip = true;
    public bool canFlip = true;

    void OnEnable()
    {
        //resets to center camera when setting active
        Vector3 tempRotation = this.transform.eulerAngles;
        tempRotation.y = defaultRotation;
        this.transform.eulerAngles = tempRotation;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        //print(this.transform.rotation.y);
        //checks if max rotation is hit
        if (counter < maxRotation)
        {
            //rotates the camera
            this.transform.Rotate(Vector3.up * cameraSpeed * Time.deltaTime);
        }
        else
        {
            if (canFlip)
            {
                StartCoroutine(FlipCamera());
            }    
        }       
    }

    /// <summary>
    /// flips camera direction when hitting max rotation
    /// </summary>
    /// <returns></returns>
    public IEnumerator FlipCamera()
    {
        //double rotation length so the camera goes all the way
        if (firstFlip)
        {
            maxRotation *= 2;
        }
        firstFlip = false;
        canFlip = false;
        float tempCameraSpeed = cameraSpeed * -1;
        cameraSpeed = 0;
        yield return new WaitForSeconds(cameraPauseTime);
        Debug.Log("flipped cam");
        counter = 0f;
        canFlip = true;
        cameraSpeed = tempCameraSpeed;
    }
}
