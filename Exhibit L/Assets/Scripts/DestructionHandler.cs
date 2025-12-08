using UnityEngine;

public class DestructionHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] gibs;
    [SerializeField] private GameObject mainGib;
    public float GibLifetime = 3f;

    public MeshRenderer mRenderer;
    public Collider bCollider;
    public Collider extraCollider;
    private float timer = 0;
    

    private bool destructionStarted = false;

    public bool Grunt = false;
    public GameObject GruntBody;

    public Enemy_Roamer enemyRoamer;

    private void Awake()
    {
        if (!Grunt)
        {
            mRenderer = GetComponent<MeshRenderer>();
        }
        
        if (bCollider == null)
        {
            bCollider = GetComponent<Collider>();
        }
    }

    public void StartDestruction()
    {
        if (!destructionStarted)
        {
            Debug.Log("DestructionStarted");
            destructionStarted = true;
            bCollider.enabled = false;
            if (enemyRoamer != null)
            {
                enemyRoamer.StopAllCoroutines();
            }
            if (extraCollider != null)
            {
                extraCollider.enabled = false;
            }
            if (mainGib != null)
            {
                mainGib.SetActive(true);
            }
            foreach (GameObject gib in gibs)
            {
                gib.SetActive(true);

                Rigidbody rb = gib.GetComponent<Rigidbody>();

                rb.angularVelocity = Vector3.zero;

                Vector3 randomDir = Random.onUnitSphere;
                float forceStrength = Random.Range(3f, 8f);
                rb.AddForce(randomDir * forceStrength, ForceMode.Impulse);

                Vector3 randomTorque = Random.insideUnitSphere * Random.Range(1f, 5f);
                rb.AddTorque(randomTorque, ForceMode.Impulse);
            }
            if (Grunt)
            {
                GruntBody.SetActive(false);
            }
            if (mRenderer != null)
            {
                mRenderer.enabled = false;
            } 
        }
    }

    private void Update()
    {
        if (destructionStarted)
        {
            timer += Time.deltaTime;
            if (timer > GibLifetime)
            {
                Destroy(gameObject);
            }
        }
    }





}
