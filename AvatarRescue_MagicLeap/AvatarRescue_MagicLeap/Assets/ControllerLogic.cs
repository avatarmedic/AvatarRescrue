using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class ControllerLogic : MonoBehaviour
{

    private MLInputController _ctrl;

    public GameObject Cube;
    private bool _isTriggerPressed;

    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        _ctrl = MLInput.GetController(MLInput.Hand.Left);

        MLInput.OnTriggerDown += HandleOnTriggerDown;
        MLInput.OnTriggerUp += HandleOnTriggerUp;

    }

    private void OnDestroy()
    {
        MLInput.Stop();
        MLInput.OnTriggerDown -= HandleOnTriggerDown;
        MLInput.OnTriggerUp -= HandleOnTriggerUp;

    }

    // Update is called once per frame
    void Update()
    {
        if(_isTriggerPressed) { 
        Cube.transform.position = _ctrl.Position;
        Cube.transform.rotation = _ctrl.Orientation;
        }
    }

    private void HandleOnTriggerDown(byte controllerId, float value)
    {
        print("trigger down!");
        _isTriggerPressed = true;
    }

    private void HandleOnTriggerUp(byte controllerId, float value)
    {
        print("trigger up!");
        _isTriggerPressed = false;
    }
}
