using UnityEngine;

public class MirrorReflection : MonoBehaviour
{
    private Vector3 lookDir;

    private void Update()
    {
        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
        }
        else
        {
            Debug.LogWarning("LookDir is bad input: " +  lookDir);
        }
    }

    public void LookDirection(Vector3 dir)
    {
        lookDir = dir;
        //Debug.Log("Mirror Reflection Direction changed to: " + dir);
    }
}
