using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D ArrowRb;
    private GameObject PlayerComponent;

    public int Damage;
    public ParticleSystem CollisionParticles;
    private bool HitGround;

    void Start()
    {
        ArrowRb = GetComponent<Rigidbody2D>();
        PlayerComponent = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        ArrowRb.linearVelocity = transform.right * -Speed * Time.deltaTime * 100;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "Attack")
        {
            HitGround = true;
            CollisionParticles.Play();
            Speed = 0f;
            StartCoroutine(Dissapear());
        }

        if(other.gameObject.tag == "Player" && HitGround == false)
        {
            CollisionParticles.Play();
            PlayerComponent.GetComponent<PlayerController>().AccesHealth(Damage, 0, 0, false);
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "Sheild" && HitGround == false)
        {
            CollisionParticles.Play();
            PlayerComponent.GetComponent<PlayerController>().AccesHealth(Damage, 0, 0, true);
            Destroy(gameObject);
        }
    }

    IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
