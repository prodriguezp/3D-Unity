using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
using Object = UnityEngine.Object;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public enum GameState
{
    menu,
    inTheGame,
    gameOver,
    pause
}

public class GameManager : MonoBehaviour
{
    public Transform CoinsInGame;
    public static GameManager sharedInstance;
    public GameObject healItem;
    public GameState currentGameState = GameState.menu;
    public List<Vector3> vectores = new List<Vector3>();
    public AudioClip normal;
    private AudioSource source;
    public Canvas menuCanvas;
    public Canvas gameCanvas;
    public Canvas gameOverCanvas;
    public Canvas gameWinCanvas;
    public Canvas gamePauseCanvas;
    private Health vidaP;
    public bool pause=false;
    public GameObject[] enemigos;
    private float contador;
    public GameObject personajePrincipal;

    private void FixedUpdate()
    {
        
    }

    public void CheckCoinsNumber()
    {
        Debug.Log("Bloques: " +CoinsInGame.childCount);
        if (CoinsInGame.childCount==1)
        {
            GameOver();
        }
    }

    public void generarItemHeal()
    {
        Random random = new Random();
        int valor;
        
        Debug.Log("CURA GENERADA");
        
        valor=random.Next(0,vectores.Count);
        Instantiate(healItem, vectores[valor], Quaternion.identity);
        vectores.RemoveAt(valor);
            
    }
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        sharedInstance = this;
        
        vectores.Add(new Vector3(-9.7f,2.5f,27.42f));
        vectores.Add(new Vector3(-6.34f,2.84f,214.33f));
        vectores.Add(new Vector3(-50f,2.47f,198.84f));        
        vectores.Add(new Vector3(-9.7f,2.5f,27.42f));
        vectores.Add(new Vector3(-6.34f,2.84f,214.33f));
        
    }
     void Start()
        {
            GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
            currentGameState = GameState.menu;
            menuCanvas.enabled = true;
            gameCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            gameWinCanvas.enabled = false;
            gamePauseCanvas.enabled = false;
            menuCanvas.GetComponent<AudioSource>().Play();
            gameCanvas.GetComponent<AudioSource>().Stop();
            gameOverCanvas.GetComponent<AudioSource>().Stop();    
            gameWinCanvas.GetComponent<AudioSource>().Stop();
            source.volume = 0;
        }

     void Update()
     {
         if (Input.GetButtonDown("Cancel"))
         {   
            
             if ((currentGameState == GameState.inTheGame) || (currentGameState == GameState.pause))
             {
                 PauseRestart();
             }
             Debug.Log(currentGameState);
         }
            if (Input.GetButtonDown("Submit"))
            {
                if (currentGameState == GameState.menu)
                {
                    StartGame();
                }

                if (currentGameState== GameState.gameOver)
                {
                    SceneManager.LoadScene("SampleScene");
                }

                
            }
        }
    public void StartGame()
    {
        Timer.sharedInstance.StartTimer();
        ChangeStateGame(GameState.inTheGame);
    }

    public void GameOver()
    {
        ChangeStateGame(GameState.gameOver);
        source.volume = 0;
        for (int i = 0; i < enemigos.Length; i++)
        {
            enemigos[i].GetComponent<AIEnemy>().pointsDamage=0;
        }

        
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void exit()
    {
        Application.Quit();
    }

    void ChangeStateGame(GameState newGameState)
    {
        if (newGameState==GameState.menu)
        {
            source.volume = 0;
            menuCanvas.enabled = true;
            gameCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            gameWinCanvas.enabled = false;
            gamePauseCanvas.enabled = false;
            menuCanvas.GetComponent<AudioSource>().Play();
            gameOverCanvas.GetComponent<AudioSource>().Stop();
            
        }
        else if (newGameState==GameState.inTheGame)
            {
                source.clip = normal;
                source.volume = 0.1f;
                source.Play();
                //se muestra la pantalla del juego
                GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = true;
                
                menuCanvas.enabled = false;
                gameOverCanvas.enabled = false;
                gameCanvas.enabled = true;
                gameWinCanvas.enabled = false;
                gamePauseCanvas.enabled = false;
                currentGameState = GameState.inTheGame;
                Timer.sharedInstance.StartTimer();
                menuCanvas.GetComponent<AudioSource>().Stop();
                gameOverCanvas.GetComponent<AudioSource>().Stop();

            }
        else if (newGameState==GameState.gameOver) {
            //se muestra la pantalla de game over
            currentGameState = GameState.gameOver;
            GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
                    
            if (Timer.sharedInstance.startCountdown)
            {
                menuCanvas.enabled = false;
                gameCanvas.enabled = false;
                gameOverCanvas.enabled = false;
                gameWinCanvas.enabled = true;
                gamePauseCanvas.enabled = false;
                        
                menuCanvas.GetComponent<AudioSource>().Stop();
                gameWinCanvas.GetComponent<AudioSource>().Play();
                        
                Timer.sharedInstance.startCountdown = false; //stop countdown
                personajePrincipal.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                Debug.Log("Congratulations, You're collect all the coins.");
                GameObject[] fireworks = GameObject.FindGameObjectsWithTag("Fire");
                foreach (var fire in fireworks)
                {
                    fire.GetComponent<ParticleSystem>().Play();
                }
            }
            else{
                menuCanvas.enabled = false;
                gameCanvas.enabled = false;
                gameOverCanvas.enabled = true;
                gameWinCanvas.enabled = false;
                gamePauseCanvas.enabled = false;
                menuCanvas.GetComponent<AudioSource>().Stop();
                gameOverCanvas.GetComponent<AudioSource>().Play();
                Debug.Log("You Lose, the time is over.");
            }
                }else if (newGameState == GameState.pause) {  
                    menuCanvas.enabled = false;
                    gameOverCanvas.enabled = false;
                    gameCanvas.enabled = false;
                    gamePauseCanvas.enabled = true;
                    currentGameState = GameState.pause;
                    
                    menuCanvas.GetComponent<AudioSource>().Stop();
                    gameOverCanvas.GetComponent<AudioSource>().Stop();
        }
        
        
        
        }
    public void PauseRestart()
    {
        Debug.Log("Paiusa");
        pause = !pause;
        if (pause)
        {
            contador = Timer.sharedInstance.countdown;//guardamos la variable antes de pausar la partida para saber cuantos segundos nos quedan de partida
            Time.timeScale = 0;
            GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
            source.Stop();
            for (int i = 0; i < enemigos.Length; i++)
            {
                enemigos[i].GetComponent<AIEnemy>().golpe.Stop();
            }
            ChangeStateGame(GameState.pause);
        }
        else
        {
            ChangeStateGame(GameState.inTheGame);
            Time.timeScale = 1;
            for (int i = 0; i < enemigos.Length; i++)
            {
                enemigos[i].GetComponent<AIEnemy>().golpe.Play();
            }
            Timer.sharedInstance.countdown=contador;//le asignamos la varible de tiempo la variable contador que contiene el tiempo de la partida
        }
    } 
}
