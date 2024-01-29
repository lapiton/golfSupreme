using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private int currentHoleNumber = 0;
    public List<Transform> startingPositions;
    public Rigidbody ballRigidbody;

    public int currentHitNumber = 0;
    private List<int> previousHitNumbers = new List<int>();

    public TMPro.TextMeshPro scoreText;

    public void DisplayScore()
    {
        string scoreString = "";
        for(int i = 0; i < previousHitNumbers.Count; i++)
        {
            Debug.Log("HOLE " + (i + 1) + " - HITS: " + previousHitNumbers[i]);
            scoreString += "HOLE " + (i + 1) + " - HITS: " + previousHitNumbers[i] + "<br>";
        }

        scoreText.text = scoreString;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GotoNextHole();
        }
    }
    
    public void GotoNextHole()
    {
        currentHoleNumber = currentHoleNumber + 1;
        if (currentHoleNumber >= startingPositions.Count)
        {
            Debug.Log("Enhorabuena, has completado todos los hoyos.");
        }
        else
        {
            ballRigidbody.transform.position = startingPositions[currentHoleNumber].position;
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
        }
        previousHitNumbers.Add(currentHitNumber);
        currentHitNumber = 0;
        DisplayScore();
    }

    private void Start()
    {
        ballRigidbody.transform.position = startingPositions[currentHoleNumber].position;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
    }
}
