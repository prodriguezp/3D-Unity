using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public enum GameState
{
    menu,
    inTheGame,
    gameOver
}
public class GameManager : MonoBehaviour
{
    public Transform CoinsInGame;

    public static GameManager sharedInstance;

    public GameState currentGameState = GameState.menu;

    public void CheckCoinsNumber()
    {
        Debug.Log("Bloques: " +CoinsInGame.childCount);
        if (CoinsInGame.childCount==1)
        {
            GameOver();
        }
    }

    private void Awake()
    {
        sharedInstance = this;
    }

    public void StartGame()
    {
        Timer.sharedInstance.StartTimer();
        ChangeStateGame(GameState.inTheGame);
    }

    public void GameOver()
    {
        ChangeStateGame(GameState.gameOver);
    }

    public void BackToMainMenu()
    {
        ChangeStateGame(GameState.menu);
    }

    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
        currentGameState = GameState.menu;
    }

    void ChangeStateGame(GameState newGameState)
    {
        if (newGameState==GameState.menu)
        {
            //se muestra el menu principal
        }
        else
        {
            if (newGameState==GameState.inTheGame)
            {
                //se muestra la pantalla del juego
                //GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = true;
                currentGameState = GameState.inTheGame;
                Timer.sharedInstance.StartTimer();
            }
            else
            {
                if (newGameState==GameState.gameOver)
                {
                    //se muestra la pantalla de game over
                    currentGameState = GameState.gameOver;
                    //GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
                    if (Timer.sharedInstance.startCountdown)
                    {
                        Timer.sharedInstance.startCountdown = false; //stop countdown
                        Debug.Log("Congratulations, You're collect all the coins.");
                        GameObject[] fireworks = GameObject.FindGameObjectsWithTag("Fire");
                        foreach (var fire in fireworks)
                        {
                            fire.GetComponent<ParticleSystem>().Play();
                        }
                    }
                    else
                    {
                        Debug.Log("You Lose, the time is over.");
                    }
                }
            }
        }
    }
    
    // Update is called once per frame
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
}
