using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

#if XR_MAGIC_LEAP
    [RequireComponent(typeof(MLImageTrackerBehavior))]
#endif
public class MagicLeapDetectedImagePlacer : MonoBehaviour
{
    private MLImageTrackerBehavior _trackerBehavior;
    private bool _targetFound = false;

    public GameObject objectToFollow;
    public GameObject CubeStage;
    /// <summary>
    /// Initializes variables and register callbacks
    /// </summary>
    void Start()
    {
        _trackerBehavior = GetComponent<MLImageTrackerBehavior>();
        _trackerBehavior.OnTargetFound += OnTargetFound;
        _trackerBehavior.OnTargetLost += OnTargetLost;
    }

    /// <summary>
    /// Unregister calbacks
    /// </summary>
    void OnDestroy()
    {
        _trackerBehavior.OnTargetFound -= OnTargetFound;
        _trackerBehavior.OnTargetLost -= OnTargetLost;
    }

    private void Update()
    {
        if (_targetFound)
        {
            // Use the current transform
            objectToFollow.transform.position = CubeStage.transform.position;
            Vector3 forward = CubeStage.transform.forward;
            forward.y = 0;
            objectToFollow.transform.forward = forward;
            objectToFollow.SetActive(true);
        }
    }

    #region Event Handlers
    /// <summary>
    /// Callback for when tracked image is found
    /// </summary>
    /// <param name="isReliable"> Contains if image found is reliable </param>
    private void OnTargetFound(bool isReliable)
    {
        _targetFound = true;
    }

    /// <summary>
    /// Callback for when image tracked is lost
    /// </summary>
    private void OnTargetLost()
    {
        _targetFound = false;

        Invoke("HidePrevious", 10);

    }

    private void HidePrevious() {
        objectToFollow.SetActive(false);
    }
#endregion
    }