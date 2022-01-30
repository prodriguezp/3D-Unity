using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    
    private Health vidaP;
    public AudioClip sonidaVida;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void Awake()
    {
        vidaP = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(sonidaVida, other.transform.position,0.3f);
            if (vidaP.HelathPoints==100)
            { 
                Debug.Log("YA TIENES LA VIDA MAXIMA");
                Destroy(gameObject);
            }
            else
            {
                vidaP.HelathPoints += 50;
                if (vidaP.HelathPoints>100)
                {
                    vidaP.HelathPoints = 100;
                }
                Debug.Log("TE HAS CURADO");
                Destroy(gameObject);
            }
            
        }
    }
    
}
