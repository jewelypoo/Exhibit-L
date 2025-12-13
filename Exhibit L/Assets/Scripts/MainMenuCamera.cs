using System.Collections;
using UnityEngine;
/*
Ian Iversen-Krampitz
10/25/2025
controls the main menu camera turn
*/
public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private bool firstFlip = true;
    [SerializeField] private bool canFlip = true;
    [SerializeField] private float cameraPauseTime;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float defaultRotation;
    [SerializeField] private float currentRotation;
    [SerializeField] private float maxRotation;
    [SerializeField] private float slowdownZone;

    public Camera renderCamera;
    public RenderTexture renderTexture;

    void OnEnable()
    {
        //resets to center camera when setting active
        Vector3 tempRotation = this.transform.eulerAngles;
        tempRotation.y = defaultRotation;
        this.transform.eulerAngles = tempRotation;
        if (renderCamera != null)
        {
            CheckAspect();
        }
        else
        {
            Debug.LogError("no camera");
        }
    }

    // Update is called once per frame
    void Update()
    {
            //a bunch of lerping nonsense to make the camera slow down near max rotation
            float distanceToMax = Mathf.Abs(maxRotation) - Mathf.Abs(currentRotation);
            float time = Mathf.InverseLerp(0f, slowdownZone, distanceToMax);
            float easedSpeed = Mathf.Lerp(0.1f, 1f, Mathf.SmoothStep(0.5f, 1f, Mathf.Pow(time, 2f)));
            float deltaY = cameraSpeed * easedSpeed * Time.deltaTime;

            //checks if max rotation is hit
            if (Mathf.Abs(currentRotation) <= maxRotation)
            {
                //rotates the camera, adds to counter
                this.transform.Rotate(0, deltaY, 0);
                currentRotation += deltaY;
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
        //double rotation length so the camera goes all the way after first turn
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
        currentRotation = 0f;
        canFlip = true;
        cameraSpeed = tempCameraSpeed;
    }

    public void CheckAspect()
    {
        renderTexture.Release();

        renderTexture.width = Screen.width / 2;
        renderTexture.height = Screen.height / 2;

        renderTexture.Create();
        renderCamera.targetTexture = renderTexture;

        renderCamera.aspect = (float)renderTexture.width / renderTexture.height;
        renderCamera.ResetProjectionMatrix();

    }
}
