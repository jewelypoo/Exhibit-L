using System.Collections;
using UnityEngine;
/*
Ian Iversen-Krampitz
10/24/2025
controls the main menu camera turn
*/
public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private GameObject securityCamera;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private bool turnAround;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxRotation;
    [SerializeField] private float cameraPauseTime;
    [SerializeField] private float defaultRotation;

    void OnEnable()
    {
        
        //resets to center camera when setting active
        Vector3 tempRotation = securityCamera.transform.eulerAngles;
        tempRotation.x = defaultRotation;
        securityCamera.transform.eulerAngles = tempRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("timer at" + Time.deltaTime);
        //checks if max rotation is hit
        if (Mathf.Abs(securityCamera.transform.rotation.x) >= maxRotation)
        {
            FlipCamera();
        }
        //rotates the camera
        securityCamera.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);       
    }

    /// <summary>
    /// flips camera direction when hitting max rotation
    /// </summary>
    /// <returns></returns>
    public IEnumerator FlipCamera()
    {
        float tempCameraSpeed = cameraSpeed * -1;
        cameraSpeed = 0;
        yield return new WaitForSeconds(cameraPauseTime);
        cameraSpeed = tempCameraSpeed;
    }
}
