using UnityEngine;
using TMPro;

public class GoalDetector : MonoBehaviour
{
    [Header("Score Settings")]
    public int currentScore = 0;
    
    [Header("UI References")]
    [Tooltip("The main TextMeshPro display above the goal.")]
    public TextMeshPro mainScoreText;
    
    [Tooltip("The floating text prefab that is spawned when a goal is scored.")]
    public GameObject floatingFeedbackPrefab;

    void Start()
    {
        // Ensure the collider is a trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }

        UpdateScoreDisplay();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered is the ball
        if (other.CompareTag("Ball"))
        {
            RegisterGoal(other.transform.position);
        }
    }

    private void RegisterGoal(Vector3 ballPosition)
    {
        currentScore++;
        UpdateScoreDisplay();

        // Spawn floating feedback
        if (floatingFeedbackPrefab != null)
        {
            Instantiate(floatingFeedbackPrefab, ballPosition, Quaternion.identity);
        }
    }

    private void UpdateScoreDisplay()
    {
        if (mainScoreText != null)
        {
            mainScoreText.text = currentScore.ToString();
        }
    }
}
