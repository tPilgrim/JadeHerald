using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject BossComponent;
    public AudioSource BossAudio;
    public AudioSource RingsAudio;

    public GameObject BossCamera;
    public GameObject PlayerCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (BossComponent.GetComponent<Warrior>() != null)
            {
                PlayerCamera.SetActive(false);
                BossCamera.SetActive(true);
                StartCoroutine(Initiate());
            }
        }
    }

    IEnumerator Initiate()
    {
        yield return new WaitForSeconds(1f);

        RingsAudio.Play();
        BossAudio.Play();
        BossComponent.GetComponent<Warrior>().Initiate();
        gameObject.GetComponent<AudioSource>().enabled = false;

        yield return new WaitForSeconds(4f);

        PlayerCamera.SetActive(true);
        BossCamera.SetActive(false);
        Destroy(gameObject);
    }
}
