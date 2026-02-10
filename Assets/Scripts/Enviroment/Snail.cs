using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : MonoBehaviour
{
    private Animator Anim;

    private Rigidbody2D SnailRb;
    private Transform Player;

    private float TimeBeforeIdle = 3f;
    public float IdleTime;
    private float IdleTimeCounter;

    private bool ShouldTurn;
    private bool GeneralDirection;

    public float Speed;
    private bool Stop;
    private bool CanMove = true;

    public Transform PatrolArea;
    private bool PatrolDirection = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SnailRb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    void Update()
    {  
        Idle();
        Flip();
        Patrol();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Idle()
    {
        if(TimeBeforeIdle > 0)
        {
            TimeBeforeIdle -= Time.deltaTime;
        }
        else
        {
            IdleTimeCounter = IdleTime;
            TimeBeforeIdle = Random.Range(4, 10);
        }

        if(IdleTimeCounter < 0)
        {
            Stop = false;
        }
        else
        {
            IdleTimeCounter -= Time.deltaTime;
            Stop = true;
        }
    }

    public void Turn(bool TurnAround)
    {
        if(TurnAround == true)
        {
            ShouldTurn = true;
        }
        else
        {
            ShouldTurn = false;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Anim.SetBool("IsHiding", true);
            CanMove = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        Anim.SetBool("IsHiding", false);
        yield return new WaitForSeconds(0.4f);
        CanMove = true;
    }

    void Flip()
    {
        Vector3 Scaler = transform.localScale;

        if(GeneralDirection == false && Scaler.x < 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        if(GeneralDirection == true && Scaler.x > 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
    }

    void Move()
    {
        if(GeneralDirection == true)
        {
            if(CanMove == true && Stop == false)
            {
                SnailRb.linearVelocity = new Vector2(Speed, 0);
            }
            else
            {
                SnailRb.linearVelocity = new Vector2(0, 0);
            }
        }
        else
        {
            if(CanMove == true && Stop == false)
            {
                SnailRb.linearVelocity = new Vector2(Speed * (-1), 0);
            }
            else
            {
                SnailRb.linearVelocity = new Vector2(0, 0);
            }
        }
    }

    void Patrol()
    {
        if(PatrolArea.position.x < transform.position.x && ShouldTurn == true)
        {
            PatrolDirection = false;
        }
        
        if(PatrolArea.position.x > transform.position.x && ShouldTurn == true) 
        {
            PatrolDirection = true;
        }

        GeneralDirection = PatrolDirection;
    }
}