using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeedOnTriggerEnter : MonoBehaviour
{
    public string targetTag;
    private Vector3 previousPosition;
    private Vector3 velocity;
    private Collider clubCollider;

    public GameManager gameManager;

    private void Awake()
    {
        clubCollider = GetComponent<Collider> ();
    }

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void Update()
    {
        velocity = (transform.position - previousPosition) / Time.deltaTime;

        previousPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Chocando...");
            gameManager.currentHitNumber++;
        }
        Vector3 collisionPosition = clubCollider.ClosestPoint(other.transform.position);

        Vector3 collisionNormal = other.transform.position - collisionPosition;

        Vector3 projectedVelocity = Vector3.Project(velocity, collisionNormal);

        Rigidbody rbBall = other.attachedRigidbody;

        //rbBall.velocity = velocity;
        rbBall.velocity = projectedVelocity;
    }
}
