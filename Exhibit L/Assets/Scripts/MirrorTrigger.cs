using UnityEngine;

public class MirrorTrigger : MonoBehaviour
{
    public GameObject mirror;
    public MirrorReflection mirrorReflection;
    public LasserBehavior lasserBehavior;

    private Vector3 reflectedDir;
    private Vector3 mirrorNormal;

    private void Awake()
    {
        mirrorNormal = transform.forward;
        mirror.SetActive(false);
    }

    private void Update()
    {
        if (mirrorReflection != null)
        {
            reflectedDir = Vector3.Reflect(lasserBehavior.camForward, mirrorNormal);
            mirrorReflection.LookDirection(reflectedDir);
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Found");
            mirror.SetActive(true);
            if (mirrorReflection == null)
            {
                mirrorReflection = mirror.GetComponentInChildren<MirrorReflection>();
                //Debug.Log("MirrorReflection Found!");
            }
            //else
            //{
                //Debug.LogWarning("MirrorRefelction not found!");
            //}
            if (lasserBehavior == null)
            {
                lasserBehavior = other.GetComponent<LasserBehavior>();
                //Debug.Log("LassserBehavior Found!");
            }
            //else
            //{
                //Debug.LogWarning("LassserBehavior not found!");
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mirror.SetActive(false);
            mirrorReflection = null;
        }
    }

}
