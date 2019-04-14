using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using System.Collections.Generic;

    public class MagicLeapImageAnchorTracker : MonoBehaviour
    {
        [SerializeField]
        private GameObject anchorPlacer = null;

        private bool _hasStarted = false;

        void OnEnable()
        {
            StartTracking();
        }

        private void OnDisable()
        {
            StopTracking();
        }

        /// <summary>
        /// Cannot make the assumption that a privilege is still granted after
        /// returning from pause. Return the application to the state where it
        /// requests privileges needed and clear out the list of already granted
        /// privileges. Also, unregister callbacks.
        /// </summary>
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                _hasStarted = false;
            }
        }


        /// <summary>
        /// Once privileges have been granted, enable the camera and callbacks.
        /// </summary>
        void StartCapture()
        {
            if (!_hasStarted)
            {
                StartTracking();
                _hasStarted = true;
            }
        }

        private void StartTracking()
        {
            if (MLImageTracker.IsStarted) {
                MLImageTracker.Enable();
                anchorPlacer.SetActive(true);
            }
        }

        private void StopTracking()
        {
            if (MLImageTracker.IsStarted) {
                MLImageTracker.Disable();
                anchorPlacer.SetActive(false);
            }
        }
    }
