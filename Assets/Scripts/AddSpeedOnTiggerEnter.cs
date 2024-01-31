using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeedOnTiggerEnter : MonoBehaviour
{
    public string targetTag;
    private Vector3 previousPosition;
    private Vector3 velocity;
    private Collider clubCollider;//Collider del palo de golf
    
    private void Awake()
    {
        clubCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        previousPosition = transform.position;//Inicializamos la previous position
    }

    private void Update()
    {
        velocity = (transform.position - previousPosition) / Time.deltaTime;
        //La velocidad es igual a la distancia dividida entre el tiempo
        previousPosition = transform.position;//Actualizamos la posición en cada frame
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("chocando");
            GameManager.sharedInstance.CurrentHitNumber++;
            GameManager.sharedInstance.DisplayScore();//Llamada para actualizar el número de golpes en tiempo real
            
            //Obtenemos el punto exacto donde colisionan los colliders del palo y de la bola
            Vector3 collisionPosition = clubCollider.ClosestPoint(other.transform.position);
            //Calculo el vector normal de colisión
            Vector3 collisionNormal = other.transform.position - collisionPosition;
            //Podemos calcular la proyección de la velocidad
            Vector3 projectedVelocity = Vector3.Project(velocity, collisionNormal);
            //Obtenemos el rigidbody de quien choca con nosotros (la pelota)
            Rigidbody rbBall = other.attachedRigidbody;
            //Le transmitimos la velocidad del palo
            //rbBall.velocity = velocity;

            //y le transmitimos la velocidad proyectada
            rbBall.velocity = projectedVelocity;
        }
    }
}
