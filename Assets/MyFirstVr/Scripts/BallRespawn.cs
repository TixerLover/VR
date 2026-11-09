using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallRespawn : MonoBehaviour
{
    [Tooltip("El punto donde la pelota reaparecer�")]
    [SerializeField] private Transform spawnPoint;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Respawn()
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            Respawn();
        }
    }
}