using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] 
    private float _healthPoints = 100;

    public float HelathPoints //getters y setters
    {
        get { return _healthPoints; }

        set
        {
            _healthPoints = value;
            if (_healthPoints<=0)
            {
                Debug.Log("Has muerto");
                Timer.sharedInstance.startCountdown = false;
                GameManager.sharedInstance.GameOver();
            }
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
