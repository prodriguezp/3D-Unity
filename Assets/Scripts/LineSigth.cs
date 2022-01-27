using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSigth : MonoBehaviour
{

    public enum SigthtSensitivity
    {
        SCRICT, //Escripta: Tiene al jugador dentro de su campo de vision (angulo y cercania) y no hay obstaculos entre medias
        LOOSE //Imprecisa: Tiene al jugador dentro de su angulo de vision pero hay obstaculos entre medias o estas demasiado lejos
    }  //dos tipos de visibilidad de este enemigo;

    //Variable para guar el estado de la visibilidad en cada momento
    public SigthtSensitivity senssivity = SigthtSensitivity.SCRICT;

    //varaible para guardar si podemos ver a nuestro objetivo
    public bool canSeeTarget = false;

    //Campo de vision del enemigo: cuantos grados vamos a poder ver
    public float fieldOfView=45f;
    
    //para saber donde estamos posicionados
    private Transform theTransform = null;
    
    //para saber donde esta posicionado el jugador
    private Transform target = null;
    
    //para guargar la colocaion de los ojos
    public Transform eyePoint = null;
    
    //rango de vision: Collider que marca el limite de mi campo de vision
    private SphereCollider theCollider = null;
    
    //la ultima localizacion donde fue visto el jugador
    public Vector3 lastKnowSigth = Vector3.zero;

    private void Awake()
    {
        theTransform = GetComponent<Transform>();
        theCollider = GetComponent<SphereCollider>();//Tengo que crear este componente en el onspector
        lastKnowSigth = theTransform.position;//lo inicializamos a mi posicion para que tenga un valor inicial
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private bool InFieldOfView()//devolvera true o false en funcion de que el jugador se encuentre dentro del angulo de vision del enemigo
    {
        //Calculo el vecto que empieza en el ojoj del enemigo y acaba en la posicion del jugador
        Vector3 directionToTheTarget = target.position - eyePoint.position;

        //Calculo el angulo entre la direccion donde miran los ojos y el veector directiontothetarget
        //si es igual o inferior a 45º entonces el jugador esta dentro de mi angulo de vision
        float angle = Vector3.Angle(eyePoint.forward, directionToTheTarget);
        if (angle<=fieldOfView)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool ClearLineOfSigth()
    {
        Vector3 directionToTheTargetNormalized = (target.position - eyePoint.position).normalized;
        RaycastHit rayCastInfo;

        if (Physics.Raycast(eyePoint.position, directionToTheTargetNormalized, out rayCastInfo, theCollider.radius))
        {
            if (rayCastInfo.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateSigth()
    {
        switch (senssivity)
        {
            case SigthtSensitivity.SCRICT:
            {
                canSeeTarget = InFieldOfView() && ClearLineOfSigth();
                break;
            }
            case SigthtSensitivity.LOOSE:
            {
                canSeeTarget = InFieldOfView() || ClearLineOfSigth();
                break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpdateSigth();
            if (canSeeTarget)
            {
                lastKnowSigth = target.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSeeTarget = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
