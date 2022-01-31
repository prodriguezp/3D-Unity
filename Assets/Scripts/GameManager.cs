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
    public AudioClip chase;
    private AudioSource source;
    public Canvas menuCanvas;
    public Canvas gameCanvas;
    public Canvas gameOverCanvas;
    public Canvas gameWinCanvas;
    public Canvas gamePauseCanvas;
    private Health vidaP;
    public bool pause=false;
    public GameObject[] enemigos;
    public int valor=0;
    public bool isAttacked = false;

    
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
        vectores.Add(new Vector3(-50f,2.47f,198.84f));
        

        vectores.Add(new Vector3(-9.7f,2.5f,27.42f));
        vectores.Add(new Vector3(-6.34f,2.84f,214.33f));
        vectores.Add(new Vector3(-50f,2.47f,198.84f));
        
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
            //GameObject.FindGameObjectWithTag("Enemy").GetComponent<AIEnemy>().source.Stop();

        }

     void Update()
     {

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
        //GameObject.FindGameObjectWithTag("Enemy").GetComponent<AIEnemy>().source.volume=0;
        source.volume = 0;
        for (int i = 0; i < enemigos.Length; i++)
        {
            Destroy(enemigos[i]);
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

    public void comprobarEstado()
    {
        for (int i = 0; i < enemigos.Length; i++)
        {
            if (enemigos[i].GetComponent<AIEnemy>().CurrentState == AIEnemy.EnemyState.PATROL)
            {
                Debug.Log("enytrASD");
                valor = 1;
                isAttacked = false;
            }
        }
        for (int i = 0; i < enemigos.Length; i++)
        {
            if (enemigos[i].GetComponent<AIEnemy>().CurrentState == AIEnemy.EnemyState.CHASE || enemigos[i].GetComponent<AIEnemy>().CurrentState == AIEnemy.EnemyState.ATTACK)
            {
                isAttacked = true;
                Debug.Log("zzz");
                valor = 2;
            }
        }
    }

    /*private void FixedUpdate()
    {
        comprobarEstado();
    }*/

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
                comprobarEstado();

                //se muestra la pantalla del juego
                GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = true;
                //GameObject.FindGameObjectWithTag("Enemy").GetComponent<AIEnemy>().source.Play();

                if (!isAttacked)
                {
                    source.volume=0.1f;
                    source.clip = normal;
                    source.Play();
                    
                }else if (isAttacked)
                {
                    source.volume=0.1f;
                    source.clip = chase;
                    source.Play();
                }

                menuCanvas.enabled = false;
                gameOverCanvas.enabled = false;
                gameCanvas.enabled = true;
                gameWinCanvas.enabled = false;
                gamePauseCanvas.enabled = false;
                currentGameState = GameState.inTheGame;
                Timer.sharedInstance.StartTimer();
                menuCanvas.GetComponent<AudioSource>().Stop();
                gameOverCanvas.GetComponent<AudioSource>().Stop();
                source.volume = 0.1f;
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
        
        pause = !pause;
        if (pause)
        {
            //GameObject.FindGameObjectWithTag("Enemy").GetComponent<AIEnemy>().source.volume=0;
            Time.timeScale = 0;
            ChangeStateGame(GameState.pause);
        }
        else
        {
            ChangeStateGame(GameState.inTheGame);
            Time.timeScale = 1;
        }
        //Debug.Log(pause);
    } 
}
