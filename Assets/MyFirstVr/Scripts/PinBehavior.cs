using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class PinBehavior : MonoBehaviour
{
    [Tooltip("El sonido que hace al caer o chocar")]
    public AudioClip fallSound;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;
    private AudioSource audioSource;
    private PinManager manager;

    private bool isFallen = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        manager = GetComponentInParent<PinManager>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void Update()
    {
        if (!isFallen)
        {
            if (Vector3.Angle(transform.up, Vector3.up) > 45f)
            {
                isFallen = true;
                if (fallSound != null) audioSource.PlayOneShot(fallSound);
                manager.ReportFallenPin();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFallen && collision.relativeVelocity.magnitude > 2f)
        {
            if (fallSound != null) audioSource.PlayOneShot(fallSound);
        }
    }

    public void ResetPin()
    {
        isFallen = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}