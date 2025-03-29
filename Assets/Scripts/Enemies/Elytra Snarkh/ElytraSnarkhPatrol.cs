using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElytraSnarkhPatrol : MonoBehaviour
{
    public GameObject EnemyComponent;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            EnemyComponent.GetComponent<ElytraSnarkh>().PatrolPoint(true);
        }
    }
}
