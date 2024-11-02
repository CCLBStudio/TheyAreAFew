using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static List<PlayerFacade> Players = new();

    private void Awake()
    {
        Players.Clear();
    }
}
