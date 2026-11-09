using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class SlingshotMechanic : MonoBehaviour
{
    [Header("Prefabs & References")]
    public GameObject throwPrefab;
    public OVRHand leftHand;
    public OVRHand rightHand;

    [Header("Slingshot Settings")]
    [Tooltip("Multiplier applied to the pull distance to calculate the final throw force.")]
    public float forceMultiplier = 10f;

    private LineRenderer lineRenderer;

    // State Tracking
    private bool isSlingshotActive = false;
    private Vector3 startPosition;
    
    // Track which hand initiated the slingshot
    private bool usingLeftHand = false;
    private bool usingRightHand = false;
    private bool usingLeftController = false;
    private bool usingRightController = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!isSlingshotActive)
        {
            CheckForPinchStart();
        }
        else
        {
            UpdateSlingshot();
        }
    }

    private void CheckForPinchStart()
    {
        // Check Right Controller
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            StartSlingshot(GetControllerPosition(OVRInput.Controller.RTouch));
            usingRightController = true;
            return;
        }

        // Check Left Controller
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            StartSlingshot(GetControllerPosition(OVRInput.Controller.LTouch));
            usingLeftController = true;
            return;
        }

        // Check Right Hand Tracking
        if (rightHand != null && rightHand.IsTracked && rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            // OVRHand.GetFingerIsPinching can sometimes be true continuously, but we only catch it here when !isSlingshotActive.
            StartSlingshot(rightHand.PointerPose.position);
            usingRightHand = true;
            return;
        }

        // Check Left Hand Tracking
        if (leftHand != null && leftHand.IsTracked && leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            StartSlingshot(leftHand.PointerPose.position);
            usingLeftHand = true;
            return;
        }
    }

    private void StartSlingshot(Vector3 position)
    {
        isSlingshotActive = true;
        startPosition = position;
        
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
    }

    private void UpdateSlingshot()
    {
        Vector3 currentPosition = Vector3.zero;
        bool isReleased = false;

        // Check release based on what initiated it
        if (usingRightController)
        {
            currentPosition = GetControllerPosition(OVRInput.Controller.RTouch);
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)) isReleased = true;
        }
        else if (usingLeftController)
        {
            currentPosition = GetControllerPosition(OVRInput.Controller.LTouch);
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch)) isReleased = true;
        }
        else if (usingRightHand)
        {
            currentPosition = rightHand.PointerPose.position;
            if (!rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index)) isReleased = true;
        }
        else if (usingLeftHand)
        {
            currentPosition = leftHand.PointerPose.position;
            if (!leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index)) isReleased = true;
        }

        if (isReleased)
        {
            ReleaseSlingshot(currentPosition);
        }
        else
        {
            // Update line renderer visual
            lineRenderer.SetPosition(1, currentPosition);
        }
    }

    private void ReleaseSlingshot(Vector3 endPosition)
    {
        isSlingshotActive = false;
        lineRenderer.enabled = false;
        ResetTrackingState();

        if (throwPrefab == null)
        {
            Debug.LogWarning("SlingshotMechanic: throwPrefab is not assigned!");
            return;
        }

        // The pull vector is start - end, multiplying its magnitude (distance) with our custom multiplier
        Vector3 pullVector = startPosition - endPosition;
        Vector3 force = pullVector * forceMultiplier;

        // Instantiate at the start position (where they first pinched)
        GameObject thrownObject = Instantiate(throwPrefab, startPosition, Quaternion.identity);
        
        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Turn off kinematic so physics takes over immediately
            rb.isKinematic = false;
            rb.AddForce(force, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("SlingshotMechanic: The throwPrefab does not have a Rigidbody attached!");
        }

        // Destroy the ball after a timeout to keep the scene clean
        Destroy(thrownObject, 8f);
    }

    private void ResetTrackingState()
    {
        usingLeftHand = false;
        usingRightHand = false;
        usingLeftController = false;
        usingRightController = false;
    }

    private Vector3 GetControllerPosition(OVRInput.Controller controller)
    {
        OVRCameraRig rig = FindObjectOfType<OVRCameraRig>();
        if (rig != null)
        {
            Vector3 localPos = OVRInput.GetLocalControllerPosition(controller);
            return rig.trackingSpace.TransformPoint(localPos);
        }
        
        return OVRInput.GetLocalControllerPosition(controller);
    }
}
