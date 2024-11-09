using System;
using PrimeTween;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField][Range(60, 144)] private int targetFps = 60;
    private void Start()
    {
        PrimeTweenConfig.SetTweensCapacity(400);
        Application.targetFrameRate = targetFps;
    }
}
