using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    private void Start()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.sharedInstance.CheckCoinsNumber();
            Timer.sharedInstance.countdown += 10;//incrementa el tiempo de juego cada vez que cogues un item
            GameManager.sharedInstance.generarItemHeal();
            Destroy(gameObject);
        }
    }
}
