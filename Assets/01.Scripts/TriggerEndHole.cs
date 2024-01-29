using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEndHole : MonoBehaviour
{
    public GameManager gameManager;
    public string targetTag = "ball";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            gameManager.GotoNextHole();
        }
    }
}
