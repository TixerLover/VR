using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallRespawn : MonoBehaviour
{
    [Header("Configuración de Respawn")]
    [Tooltip("El punto donde la pelota reaparecerá")]
    [SerializeField] private Transform spawnPoint;

    [Tooltip("Tiempo máximo en segundos antes de forzar el respawn")]
    [SerializeField] private float maxLifetime = 5f;

    [Tooltip("Tag del objeto/base donde el tiempo no corre")]
    [SerializeField] private string safeBaseTag = "SafeBase";

    private Rigidbody rb;
    private float currentTimer = 0f;
    private bool isSafe = true; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isSafe)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= maxLifetime)
            {
                Respawn();
            }
        }
    }

    public void Respawn()
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        currentTimer = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(safeBaseTag))
        {
            isSafe = true;
            currentTimer = 0f; 
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(safeBaseTag))
        {
            isSafe = false; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            Respawn();
        }
    }
}