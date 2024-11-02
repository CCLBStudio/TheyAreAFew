using System;
using UnityEngine;

public class PlayerFacade : MonoBehaviour
{
    private void Start()
    {
        PlayerManager.Players.Add(this);
    }
}
