using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject BossComponent;
    public AudioSource BossAudio;
    public AudioSource RingsAudio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (BossComponent.GetComponent<Warrior>() != null)
            {
                RingsAudio.Play();
                BossAudio.Play();
                BossComponent.GetComponent<Warrior>().Initiate();
                Destroy(gameObject);
            }
        }
    }
}
