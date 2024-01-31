using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryExitDetector : MonoBehaviour
{
    public GameManager gameManager; // Referencia al GameManager que maneja la lógica del juego

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball")) // Verifica si el objeto que sale de los límites es la bola
        {
            gameManager.TeleportBallToStartPoint(); // Llama al método del GameManager para teletransportar la bola al punto de inicio del HOLE actual
            Debug.Log("BOLA FUERA");
        }
    }
}
