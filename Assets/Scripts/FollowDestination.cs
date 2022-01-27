using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowDestination : MonoBehaviour
{

    private NavMeshAgent theAgent = null;
    public Transform destination;

    private void Awake()
    {
        theAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theAgent.SetDestination(destination.position);
    }
}
