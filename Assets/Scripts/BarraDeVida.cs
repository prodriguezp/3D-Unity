using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class BarraDeVida : MonoBehaviour
{

    public Image barradevida;
    private Health vidaP;

    public float vidaActual;

    public float vidaMaxima=100;
    
    private void Awake()
    {
        vidaP = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        vidaActual = vidaP.HelathPoints;
        barradevida.fillAmount = vidaActual / vidaMaxima;
    }
}