using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static Timer sharedInstance;
    public float limitTime = 60f;
    public float countdown = 0f;
    public bool startCountdown = false;

    private void Awake()
    {
        sharedInstance = this;
    }

    public void StartTimer()
    {
        countdown = limitTime;
        startCountdown = true;
    }

    // Start is called before the first frame update
    void Start()    
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (startCountdown)
        {
            countdown -= Time.deltaTime;
            Debug.Log("Cuentra atras: " +countdown);
            if (countdown <=0)
            {
                startCountdown = false;
                GameManager.sharedInstance.GameOver();
            }
        }
    }
}
