using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class FloatingTextFeedback : MonoBehaviour
{
    [Header("Animation Settings")]
    public float floatSpeed = 1f;
    public float lifetime = 2f;
    public Vector3 floatDirection = Vector3.up;

    private TextMeshPro textMesh;
    private Color originalColor;
    private float timer;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh != null)
        {
            originalColor = textMesh.color;
        }
        
        // Ensure it always faces the camera roughly
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        // Move the text upwards
        transform.position += floatDirection * floatSpeed * Time.deltaTime;

        // Fade out
        if (textMesh != null)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0f, timer / lifetime);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }

        // Destroy when lifetime is reached
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
