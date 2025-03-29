using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform TargetUp;
    public Transform TargetDown;
    private Transform Target;
    private bool Move;
    public float Speed;
    private float Direction;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(StartMoving());
            other.transform.parent = this.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

    void FixedUpdate()
    {
        if(Move == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
        }

        if(transform.position == TargetUp.position)
        {
            Move = false;
            Target = TargetDown;
        }
        else if(transform.position == TargetDown.position)
        {
            Move = false;
            Target = TargetUp;
        }
    }

    IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(1f);
        Move = true;
    }

    /*
    private Rigidbody2D ObjectRb;
    public float Speed;
    private float Direction = -1f;
    private bool IsMoving;

    void Start()
    {
        ObjectRb = GetComponent<Rigidbody2D>();
        //ObjectRb.isKinematic = true;
        //ObjectRb.velocity = constantVelocity;
    }

    void FixedUpdate()
    {
        Move();
        Debug.Log(ObjectRb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player" && IsMoving == false)
        {
            StartCoroutine(StartMoving());
        }
    }

    void Move()
    {
        //ObjectRb.velocity = new Vector2(0f, Speed * Direction);
    }

    private Vector2 a;

    IEnumerator StartMoving()
    {
        IsMoving = true;
        ObjectRb.isKinematic = true;
        yield return new WaitForSeconds(2f);
        ObjectRb.isKinematic = false;
        a = new Vector2(0f, Speed * Direction);
        ObjectRb.MovePosition(ObjectRb.position + a * Time.fixedDeltaTime);
        yield return new WaitForSeconds(10f);
        IsMoving = false;
        /*
        IsMoving = true;
        ObjectRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(2f);
        ObjectRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        ObjectRb.constraints = RigidbodyConstraints2D.FreezePositionX;
        Direction = -1f;
        */
}
