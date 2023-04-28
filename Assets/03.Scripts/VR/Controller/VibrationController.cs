using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationController : MonoBehaviour
{
    [SerializeField] private float vibrateFrequency = 0.1f;
    [SerializeField] private float vibrateValue = 0.1f;
    [SerializeField] private float vibrateDuration = 0.1f;
    private InputBridge input;
    private void Awake()
    {
        input = InputBridge.Instance;
    }

    public void VibrateLeftController()
    {
        input.VibrateController(vibrateFrequency, vibrateValue, vibrateFrequency, ControllerHand.Left);
    }
    public void VibrateRightController()
    {
        print("xxx");
        input.VibrateController(vibrateFrequency, vibrateValue, vibrateFrequency, ControllerHand.Right);
    }
}
