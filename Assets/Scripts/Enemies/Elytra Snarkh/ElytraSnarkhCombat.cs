using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElytraSnarkhCombat : MonoBehaviour
{
    public GameObject EnemyComponent;


    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            EnemyComponent.GetComponent<ElytraSnarkh>().Combat(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            EnemyComponent.GetComponent<ElytraSnarkh>().Combat(false);
        }
    }
}
