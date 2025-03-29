using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject EnemyComponent;
    public GameObject AttackRange;

    void OnTriggerStay2D(Collider2D AttackRange)
    {
        Debug.Log("asd");
        if (AttackRange.gameObject.tag == "Player")
        {
            if (EnemyComponent.GetComponent<Sentinel>() != null)
            {
                EnemyComponent.GetComponent<Sentinel>().StartAttack(true);
            }
        }
    }
}
