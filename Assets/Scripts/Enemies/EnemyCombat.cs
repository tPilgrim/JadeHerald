using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public GameObject EnemyComponent;
    public int Area;

    private IEnemy Enemy;

    private void Awake()
    {
        if (EnemyComponent.GetComponent<IEnemy>() != null)
            Enemy = EnemyComponent.GetComponent<IEnemy>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Enemy != null)
                Enemy.Combat(true, Area);
        }

        if (other.gameObject.tag == "Player")
        {
            if (EnemyComponent.GetComponent<Snarkh>() != null)
            {
                EnemyComponent.GetComponent<Snarkh>().Combat(true);
            }

            if (EnemyComponent.GetComponent<Scout>() != null)
            {
                EnemyComponent.GetComponent<Scout>().Combat(true);
            }

            if(EnemyComponent.GetComponent<Archer>() != null)
            {
                EnemyComponent.GetComponent<Archer>().Combat(true);
            }

            if (EnemyComponent.GetComponent<Sentinel>() != null)
            {
                EnemyComponent.GetComponent<Sentinel>().Combat(true);
            }

            if (EnemyComponent.GetComponent<Bogle>() != null)
            {
                EnemyComponent.GetComponent<Bogle>().Combat(true);
            }

            if (EnemyComponent.GetComponent<Phosp>() != null)
            {
                EnemyComponent.GetComponent<Phosp>().Combat(true);
            }

            if (EnemyComponent.GetComponent<Shrum>() != null)
            {
                EnemyComponent.GetComponent<Shrum>().Combat(true);
            }

            if (EnemyComponent.GetComponent<Bud>() != null)
            {
                EnemyComponent.GetComponent<Bud>().Combat(true);
            }

            if (EnemyComponent.GetComponent<Sprout>() != null)
            {
                EnemyComponent.GetComponent<Sprout>().Combat(true);
            }

            if (EnemyComponent.GetComponent<Warrior>() != null)
            {
                EnemyComponent.GetComponent<Warrior>().Combat(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Enemy != null)
                Enemy.Combat(false, Area);
        }

        if (other.gameObject.tag == "Player")
        {
            if (EnemyComponent.GetComponent<Snarkh>() != null)
            {
                EnemyComponent.GetComponent<Snarkh>().Combat(false);
            }

            if (EnemyComponent.GetComponent<Scout>() != null)
            {
                EnemyComponent.GetComponent<Scout>().Combat(false);
            }

            if(EnemyComponent.GetComponent<Archer>() != null)
            {
                EnemyComponent.GetComponent<Archer>().Combat(false);
            }

            if (EnemyComponent.GetComponent<Sentinel>() != null)
            {
                EnemyComponent.GetComponent<Sentinel>().Combat(false);
            }

            if (EnemyComponent.GetComponent<Bogle>() != null)
            {
                EnemyComponent.GetComponent<Bogle>().Combat(false);
            }

            if (EnemyComponent.GetComponent<Phosp>() != null)
            {
                EnemyComponent.GetComponent<Phosp>().Combat(false);
            }

            if (EnemyComponent.GetComponent<Shrum>() != null)
            {
                EnemyComponent.GetComponent<Shrum>().Combat(false);
            }

            if (EnemyComponent.GetComponent<Bud>() != null)
            {
                EnemyComponent.GetComponent<Bud>().Combat(false);
            }

            if (EnemyComponent.GetComponent<Sprout>() != null)
            {
                EnemyComponent.GetComponent<Sprout>().Combat(false);
            }

            if (EnemyComponent.GetComponent<Warrior>() != null)
            {
                EnemyComponent.GetComponent<Warrior>().Combat(false);
            }
        }
    }
}
