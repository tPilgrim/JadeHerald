using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject EnemyComponent;
    public GameObject AttackRange;

    void OnTriggerStay2D(Collider2D AttackRange)
    {
        if (AttackRange.gameObject.tag == "Player")
        {
            if (EnemyComponent.GetComponent<Snarkh>() != null)
            {
                EnemyComponent.GetComponent<Snarkh>().StartAttack(true);
            }

            if (EnemyComponent.GetComponent<Scout>() != null)
            {
                EnemyComponent.GetComponent<Scout>().StartAttack(true);
            }

            if (EnemyComponent.GetComponent<Archer>() != null)
            {
                EnemyComponent.GetComponent<Archer>().StartAttack(true);
            }

            if (EnemyComponent.GetComponent<Sentinel>() != null)
            {
                EnemyComponent.GetComponent<Sentinel>().StartAttack(true);
            }

            if (EnemyComponent.GetComponent<Bogle>() != null)
            {
                EnemyComponent.GetComponent<Bogle>().StartAttack(true);
            }

            if (EnemyComponent.GetComponent<Bud>() != null)
            {
                EnemyComponent.GetComponent<Bud>().StartAttack(true);
            }
        }

        if (AttackRange.gameObject.tag == "Respawn")
        {
            if (EnemyComponent.GetComponent<Snarkh>() != null)
            {
                EnemyComponent.GetComponent<Snarkh>().StartAttack(false);
            }

            if (EnemyComponent.GetComponent<Scout>() != null)
            {
                EnemyComponent.GetComponent<Scout>().StartAttack(false);
            }

            if (EnemyComponent.GetComponent<Archer>() != null)
            {
                EnemyComponent.GetComponent<Archer>().StartAttack(false);
            }

            if (EnemyComponent.GetComponent<Sentinel>() != null)
            {
                EnemyComponent.GetComponent<Sentinel>().StartAttack(false);
            }

            if (EnemyComponent.GetComponent<Bogle>() != null)
            {
                EnemyComponent.GetComponent<Bogle>().StartAttack(false);
            }

            if (EnemyComponent.GetComponent<Bud>() != null)
            {
                EnemyComponent.GetComponent<Bud>().StartAttack(false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D AttackRange)
    {
        if (AttackRange.gameObject.tag == "Player")
        {
            if (EnemyComponent.GetComponent<Snarkh>() != null)
            {
                EnemyComponent.GetComponent<Snarkh>().StartAttack(false);
            }

            if (EnemyComponent.GetComponent<Scout>() != null)
            {
                EnemyComponent.GetComponent<Scout>().StartAttack(false);
            }

            if (EnemyComponent.GetComponent<Archer>() != null)
            {
                EnemyComponent.GetComponent<Archer>().StartAttack(false);
            }

            if (EnemyComponent.GetComponent<Sentinel>() != null)
            {
                EnemyComponent.GetComponent<Sentinel>().StartAttack(false);
            }

            if (EnemyComponent.GetComponent<Bogle>() != null)
            {
                EnemyComponent.GetComponent<Bogle>().StartAttack(false);
            }

            if (EnemyComponent.GetComponent<Bud>() != null)
            {
                EnemyComponent.GetComponent<Bud>().StartAttack(false);
            }
        }
    }
}
