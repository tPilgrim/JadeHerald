using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedCombat : MonoBehaviour
{
    public GameObject EnemyComponent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(EnemyComponent.GetComponent<Archer>() != null)
            {
                EnemyComponent.GetComponent<Archer>().RangedCombat(true);
            }

            if (EnemyComponent.GetComponent<Phosp>() != null)
            {
                EnemyComponent.GetComponent<Phosp>().RangedCombat(true);
            }

            if (EnemyComponent.GetComponent<Shrum>() != null)
            {
                EnemyComponent.GetComponent<Shrum>().RangedCombat(true);
            }

            if (EnemyComponent.GetComponent<Sprout>() != null)
            {
                EnemyComponent.GetComponent<Sprout>().RangedCombat(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(EnemyComponent.GetComponent<Archer>() != null)
            {
                EnemyComponent.GetComponent<Archer>().RangedCombat(false);
            }

            if (EnemyComponent.GetComponent<Phosp>() != null)
            {
                EnemyComponent.GetComponent<Phosp>().RangedCombat(false);
            }

            if (EnemyComponent.GetComponent<Shrum>() != null)
            {
                EnemyComponent.GetComponent<Shrum>().RangedCombat(false);
            }

            if (EnemyComponent.GetComponent<Sprout>() != null)
            {
                EnemyComponent.GetComponent<Sprout>().RangedCombat(false);
            }
        }
    }
}
