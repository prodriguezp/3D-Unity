using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public AudioClip sonido;

    private void Start()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(sonido, other.transform.position,0.3f);
            GameManager.sharedInstance.CheckCoinsNumber();
            Timer.sharedInstance.countdown += 10;//incrementa el tiempo de juego cada vez que cogues un item
            GameManager.sharedInstance.generarItemHeal();
            Destroy(gameObject);
        }
    }
}
