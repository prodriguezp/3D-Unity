using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIEnemy : MonoBehaviour
{
    //Declaramos los 3 estados por los que puede pasar el enemigo dentro de un enumerado
    public enum EnemyState
    {
        PATROL,
        CHASE,
        ATTACK
    }

    [SerializeField] //Para que se muestre en el inspector de la variable privada currentState
    private EnemyState _currentState = EnemyState.PATROL;

    public float pointsDamage=10.0f;
    private Health targetHealth;
    private LineSigth theLineSigth; //objeto linea de vision de la clase que hicimos anteriormente. La inicializamos en el Awake
    private NavMeshAgent theAgent; // La inicializaremos en el Awake
    private Transform target; // la inciializaremos en el Awake;
    public Transform destination; //El destinoo por donde patrullara el enemigo. La asignaremos en el Start
    private Animator _animator;
    public NavMeshAgent agente;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        targetHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        theLineSigth = GetComponent<LineSigth>();
        theAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        List<GameObject> randomDestinations = new List<GameObject>(GameObject.FindGameObjectsWithTag("Destination"));
        int valor = Random.Range(0, randomDestinations.Count);
        destination = randomDestinations[valor].GetComponent<Transform>();
        randomDestinations.RemoveAt(valor);
    }

    public EnemyState CurrentState //propiedades para acceder _currentState mediante get y set
    {
        get { return _currentState; }//para obtener el calor de la variable, o tambien: get =>  _currentState;
        set
        {
            _currentState = value; //el valor que elijamos para modificar _currentState
            StopAllCoroutines(); //paramos todas las corrutinas para luego decidir cual arrancar

            switch (value)
            {
                case EnemyState.PATROL:
                {
                    _animator.SetBool("Attacking", false);
                    StartCoroutine(AIPatrol());
                    break;
                }
                case EnemyState.CHASE:
                {
                    _animator.SetBool("Attacking", false);
                    StartCoroutine(AIChase());
                    break;
                }
                case EnemyState.ATTACK:
                {
                    _animator.SetBool("Attacking", true);
                    StartCoroutine(AIAttack());
                    break;
                }
            }
        }
    }
    
    public IEnumerator AIPatrol()
    {
        while (CurrentState==EnemyState.PATROL)
        {
            if (pointsDamage<=10)
            {
                agente.speed = 5;
            }
            else if (pointsDamage>=15)
            {
                agente.speed = 4;
            }

           
            theLineSigth.senssivity = LineSigth.SigthtSensitivity.SCRICT;
            theAgent.isStopped = false; //Para que siga patrullando con sensibilidad estricta
            theAgent.SetDestination(destination.position); //hacia el destino que tenga asiganada
        

        while (theAgent.pathPending==true) //mientras esperamos a que se calcule la ruta a seguir en patrulla
        {
            yield return null; //pausamos la corrutina y esperamos al siguiente frame a ver si ya esta calculando la ruta a seguir
        }

        if (theLineSigth.canSeeTarget)// si vemos al objetivo
        {
            theAgent.isStopped = true;//dejamos de patrulalr
            CurrentState = EnemyState.CHASE;// y cambiamos al estado de perseguir
            yield break;//finalizamos la corrutina AIPatrol
        }
        
        yield return null;// llego aqui cuando la ruta esta calculada pero no he visto al objetivo por lo que pauso la corrutina hasta el siguiente frame
        }
    }
    
    public IEnumerator AIChase()
    {
        while (CurrentState==EnemyState.CHASE)
        {
            if (pointsDamage<=10)
            {
                agente.speed = 9;
            }
            else if (pointsDamage>=15)
            {
                agente.speed = 8;
            }
            
            theLineSigth.senssivity = LineSigth.SigthtSensitivity.LOOSE; //para que sea mas dificil que pierda de vista al objetivo
            theAgent.isStopped = false; //Para que siga patrullando con sensibilidad imprecisa
            theAgent.SetDestination(theLineSigth.lastKnowSigth); //hacia el ultimo sitio que vio al objetivo
        

        while (theAgent.pathPending==true) //mientras esperamos a que se calcule la ruta a seguir el objetivo
        {
            yield return null; //pausamos la corrutina y esperamos al siguiente frame a ver si ya esta calculando la ruta a seguir
        }

        if (theAgent.remainingDistance <= theAgent.stoppingDistance)// si la distancia que falta es <= que la de parada
        {
            theAgent.isStopped = true; //Dejamos de perseguir

            if (theLineSigth.canSeeTarget == false)// si hemos perdido de vista al objetivo
            {
                CurrentState = EnemyState.PATROL; //volvemos al estado de patrulla
            }else
            {//no la hemos perdido asique ataco
                CurrentState = EnemyState.ATTACK;
            }
            yield break;
        }
        
        
        yield return null;// llego aqui cuando la ruta esta calculada pero no he visto al objetivo por lo que pauso la corrutina hasta el siguiente frame
        }
    }
    
    public IEnumerator AIAttack()
    {
        while (CurrentState== EnemyState.ATTACK)
        {

            theAgent.isStopped = false;
            theAgent.SetDestination(target.position);
            while (theAgent.pathPending==true)
            {
                yield return null;
            }

            if (theAgent.remainingDistance>theAgent.stoppingDistance)
            {
                theAgent.isStopped = true;
                CurrentState = EnemyState.CHASE;
                yield break;
            }
            else
            {
                targetHealth.HelathPoints -= pointsDamage * Time.deltaTime;
                Debug.Log(targetHealth.HelathPoints);
            }

            yield return null;
        }
    }
        
    // Start is called before the first frame update
    void Start()
    {
        CurrentState = EnemyState.PATROL; // por defecto cuando arrance el script el enemigo estara patrullando
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
