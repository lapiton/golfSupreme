using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    private int currentHoleNumber = 0; //Saber en qu� hoyo estamos
    [SerializeField] private List<Transform> startingPositions; //Lista con los puntos de partida donde se colocar� la bola inicialmente en cada hoyo
    [SerializeField] private Rigidbody ballRigidbody; //Referencia de la bola

    [SerializeField] private int currentHitNumber = 0; //Llevar la cuenta de golpes dados en el HOLE actual
    private List<int> previousHitNumbers = new List<int>(); //Ir guardando los golpes necesitados en HOLES anteriores

    [SerializeField] private TextMeshProUGUI scoreText;//Referencia al marcador de golpes del canvas
    [SerializeField] private TextMeshProUGUI totalHitsText;//Referencia al marcador de golpes totales del canvas
    [SerializeField] private TextMeshProUGUI recordHitsText;//Referencia al marcador de r�cord de golpes de las partidas anteriores

    public GameObject scoreMenu;//Referencia al men� de puntuaci�n

    private Transform lastUsedStartingPosition; // Variable para almacenar la �ltima posici�n inicial utilizada

    public int CurrentHitNumber
    {
        get => currentHitNumber;
        set => currentHitNumber = value;
    }

    private void Awake()
    {
        sharedInstance = this;
        DisplayScore();//Llamamos a DisplayScore en Awake para mostrar el marcador de golpes desde el principio
        Debug.Log("GameManager Awake() llamado.");

        //Comprobamos si el r�cord almacenado es igual a 0
        int recordHits = GetRecordHits();
        if (recordHits == 0)
        {
            PlayerPrefs.DeleteKey("RecordHits");//Borra el valor almacenado en PlayerPrefs para la clave "RecordHits"
            PlayerPrefs.Save();//Guarda los cambios en PlayerPrefs
        }
    }

    private void Start()
    {
        ballRigidbody.transform.position = startingPositions[currentHoleNumber].position;
        ballRigidbody.velocity = Vector3.zero;//Reseteamos las velocidades que lleve la bola
        ballRigidbody.angularVelocity = Vector3.zero;//Reseteamos las velocidades que lleve la bola
        Debug.Log("GameManager Start() llamado.");

        DisplayScore();//Llamamos a DisplayScore en Start para mostrar el marcador de golpes desde el principio
        ResetBall();
    }

    private void ResetBall()
    {
        ballRigidbody.transform.position = startingPositions[currentHoleNumber].position;
        ballRigidbody.velocity = Vector3.zero;//Reset de las velodidades de la bola
        ballRigidbody.angularVelocity = Vector3.zero;//Reset de las velodidades de la bola
    }

    public void DisplayScore()
    {
        string scoreString = "";//Acumular las puntuaciones
        int totalHits = CalculateTotalHits();//Calcular la suma total de golpes
        int recordHits = GetRecordHits();//Obtener el r�cord de menos golpes

        // Iterar a trav�s de los golpes anteriores y mostrarlos
        /*for (int i = 0; i < previousHitNumbers.Count; i++)
        {
            Debug.Log("HOLE " + (i + 1) + " - HITS: " + previousHitNumbers[i]);
            scoreString += "HOLE " + (i + 1) + " - HITS: " + previousHitNumbers[i] + "<br>";
        }*/

        // Iterar a trav�s de los golpes anteriores y mostrarlos
        for (int i = 0; i < previousHitNumbers.Count; i++)
        {
            string holeInfo = "HOLE " + (i + 1) + " - HITS: " + previousHitNumbers[i];
            if (i < currentHoleNumber)
            {
                holeInfo = $"<color=blue>{holeInfo}</color>";//Color azul para hoyos completados
            }
            else if (i == currentHoleNumber)
            {
                holeInfo = $"<color=red>{holeInfo}</color>";//Color rojo para hoyo actual
            }
            else
            {
                holeInfo = $"<color=white>{holeInfo}</color>";//Color blanco para hoyos futuros
            }

            scoreString += holeInfo + "<br>";
        }

        totalHitsText.text = "TOTAL HITS: " + totalHits;//Mostrar la suma total de golpes en totalHitsText
        recordHitsText.text = "RECORD HITS: " + recordHits;//Mostrar el r�cord de menos golpes en recordHitsText
        scoreText.text = scoreString;//Mostrar la puntuaci�n en scoreText
        Debug.Log("DisplayScore() llamado. Total Hits: " + totalHits + ", Record Hits: " + recordHits);
    }

    public void GotoNextHole()//Llevar la bola al siguiente punto de partida del siguiente hoyo
    {//Le llamaremos desde el collider que pongamos dentro de los hoyos
        currentHoleNumber++;
        if (currentHoleNumber >= startingPositions.Count)
        {
            Debug.Log("Enhorabuena, has completado todos los hoyos.");
            int totalHits = CalculateTotalHits();
            int recordHits = GetRecordHits();//Determinar si el jugador ha establecido un nuevo r�cord

            ShowScoreMenu();

            if (totalHits < recordHits)
            {
                UpdateRecordHits(totalHits);//Se ha establecido un nuevo r�cord, actualiza y guarda el r�cord
            }
        }
        else
        {
            ResetBall();
        }
        previousHitNumbers.Add(currentHitNumber);//A�adimos a la lista cu�ntos golpes necesitamos para pasar de HOLE
        currentHitNumber = 0;//Inicializa la variable del n�mero de golpes por HOLE (se incrementa desde el OnTriggerEnter)

        DisplayScore();//Muestra los valores de la lista de golpes
        Debug.Log("GotoNextHole() llamado. Current Hole Number: " + currentHoleNumber);
    }

    private void ShowScoreMenu()
    {
        //Activa el men� de puntuaci�n
        scoreMenu.SetActive(true);

        // Busca el panel que contiene los mensajes dentro del men� de puntuaci�n
        Transform scorePanel = scoreMenu.transform.Find("ScorePanel");
        if (scorePanel != null)
        {
            //Busca el mensaje de fin de partida dentro del panel
            Transform endGameMessage = scorePanel.Find("EndGameMessage");
            if (endGameMessage != null)
            {
                endGameMessage.gameObject.SetActive(true);
            }

            //Comprueba si se ha establecido un nuevo r�cord
            if (CurrentHitNumber < GetRecordHits())
            {
                //Actualiza el r�cord con la nueva puntuaci�n
                UpdateRecordHits(CurrentHitNumber);

                //Busca el mensaje de nuevo r�cord dentro del panel
                Transform newRecordMessage = scorePanel.Find("NewRecordMessage");
                if (newRecordMessage != null)
                {
                    newRecordMessage.gameObject.SetActive(true);
                }
            }

            //Desactiva el texto de las puntuaciones
            Transform scoreText = scorePanel.Find("ScoreText (TMP)");
            if (scoreText != null)
            {
                scoreText.gameObject.SetActive(false);
            }

            //Activa los botones del men� de puntuaci�n
            ActivateScoreMenuButtons(scorePanel);
        }
    }

    public void RestartGame()
    {
        //Reiniciar el nivel actual (escena)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        //Salir del juego
        Application.Quit();
        Debug.Log("Se cierra el juego");
    }

    private void ActivateScoreMenuButtons(Transform scorePanel)
    {
        //Activa los botones dentro del panel
        foreach (Transform child in scorePanel)
        {
            if (child.CompareTag("button"))
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        /*if (Input.GetButtonDown("LeftMenuButton"))//Detectar si se ha presionado el bot�n de men� del controlador izquierdo
        {
            scoreMenu.SetActive(!scoreMenu.activeSelf);//Alterna la visibilidad del men� de puntuaci�n
        }*/

        if (Input.GetKeyDown(KeyCode.Space))//Detecta si se ha presionado la tecla de espacio
        {
            GotoNextHole();//Vamos al siguiente hoyo al pulsar la tecla
            Debug.Log("GotoNextHole() llamado.");
        }
        CalculateTotalHits(); // Llama a CalculateTotalHits() en cada frame para actualizar el marcador de golpes
    }

    public int CalculateTotalHits()//Calculamos los golpes totales
    {
        int totalHits = 0;
        foreach (int hits in previousHitNumbers)
        {
            totalHits += hits;
        }
        Debug.Log("CalculateTotalHits() llamado. Total Hits: " + totalHits);
        return totalHits;
    }

    public void UpdateRecordHits(int newRecord)//Actualizar y guardar el r�cord de menos golpes
    {
        PlayerPrefs.SetInt("RecordHits", newRecord);
        PlayerPrefs.Save();
        Debug.Log("UpdateRecordHits() llamado. Nuevo r�cord: " + newRecord);
    }

    public int GetRecordHits()
    {
        // Obtener el r�cord de menos golpes desde PlayerPrefs
        int recordHits = PlayerPrefs.GetInt("RecordHits", int.MaxValue);
        Debug.Log("GetRecordHits() llamado. Record Hits: " + recordHits);
        return recordHits;
    }

    // M�todo para teletransportar la bola al punto de inicio del HOLE actual
    public void TeleportBallToStartPoint()
    {
        if (currentHoleNumber >= 0 && currentHoleNumber < startingPositions.Count)
        {
            ballRigidbody.transform.position = startingPositions[currentHoleNumber].position;
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("No hay una posici�n inicial asignada para el hoyo actual.");
        }
    }
}
