using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class OrientationHandler : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cineCamera;

    // Update is called once per frame
    void Update()
    {
        float cameraY = cineCamera.GetComponent<CinemachinePanTilt>().PanAxis.Value;
        transform.rotation = Quaternion.Euler(0, cameraY, 0);
    }
}
