using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoughTerrain : MonoBehaviour
{
    public int Damage;
    public GameObject PlayerComponent;

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerComponent.GetComponent<PlayerController>().AccesHealth(Damage, 0, 0, false);
        }
    }
}
