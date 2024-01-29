using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEndHole : MonoBehaviour
{
    public GameManager gameManager;
    private string targetTag = "Ball";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            gameManager.GoToNextHole();
        }
    }
}
