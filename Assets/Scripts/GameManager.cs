using System;
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

    [SerializeField] private TMPro.TextMeshPro scoreText;
    private void Start()
    {
        ResetBall();
    }

    private void ResetBall()
    {
        ballRigidbody.transform.position = startingPositions[currentHoleNumber].position;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
    }

    public void DisplayScore()
    {
        string scoreString = "";
        for (int i = 0; i < previousHitNumbers.Count; i++)
        {
            Debug.Log("Hole" + (i +1) + "Hits:" + previousHitNumbers[i]);
            scoreString += "HOLE " + (i + 1) + " - HITS: " + previousHitNumbers[i] + "<br>";
        }
        scoreText.text = scoreString;
    }

    public void GoToNextHole()
    {
        currentHoleNumber++;
        if (currentHoleNumber >= startingPositions.Count)
        {
            Debug.Log("Completaste todo");
        }
        else
        {
            ResetBall();
        }
        previousHitNumbers.Add(currentHitNumber);
        currentHitNumber = 0;
        DisplayScore();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GoToNextHole();
        }
    }
}
