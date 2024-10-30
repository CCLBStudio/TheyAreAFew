using System;
using PrimeTween;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private void Start()
    {
        PrimeTweenConfig.SetTweensCapacity(400);
    }
}
