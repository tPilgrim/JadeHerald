using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElytraSnarkh : MonoBehaviour
{   
    public bool IsStationary;
    public bool IsRestraind;
    private Rigidbody2D EnemyRb;

    public Transform OldPatrolPoint;
    public Transform NewPatrolPoint;
    private float Speed;
    private bool NextPoint;
    private bool NextPoint2;
    private float PositionX;
    private float PositionY;

    private bool Stop;
    public float IdleTime;
    private float IdleTimeCounter;

    private bool IsInCombat;
    public float PatrolSpeed;
    public float CombatSpeed;
    private Transform Player;
    private bool CanCombat = true;
    private bool ShouldStop;

    private Vector2 Movement;
    private Vector2 Direction;

    void Start()
    {
        Speed = PatrolSpeed;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(IsInCombat == false)
        {
            ShouldStop = true;
        }
        else if(ShouldStop == false)
        {
            Stop = false;
        }
        Idle();
    }

    void FixedUpdate()
    {
        if(IsInCombat == false && IsRestraind == false)
        {
            Patrol();
        }

        if(IsRestraind == true && IsInCombat == false)
        {
            Patrol();
        }

        if(IsInCombat == true && CanCombat == true)
        {
            Follow();
        }
    }

    public void PatrolPoint(bool PointReached)
    {
        if(PointReached == true)
        {
            NextPoint = true;
        }
    }
    
    void Patrol()
    {
        if(NextPoint == true)
        {
            if(IsRestraind == false)
            {
                IdleTimeCounter = IdleTime;
            }

            Vector3 NewPoint = NewPatrolPoint.position;

            do
            {
                PositionX = Random.Range(-2, 2);
            }while(PositionX > -1 && PositionX < 1);

            do
            {
                PositionY = Random.Range(-2, 2);
            }while(PositionY > -1 && PositionY < 1);

            NewPoint.x = OldPatrolPoint.position.x + PositionX;
            NewPoint.y = OldPatrolPoint.position.y + PositionY;

            if(IsStationary == false)
            {
                NewPatrolPoint.position = NewPoint;
            }

            NextPoint2 = true;
        }
        else
        {
            if(Stop == false)
            {
                transform.position = Vector2.MoveTowards(transform.position, NewPatrolPoint.position, Speed * Time.deltaTime);
            }
        }

        if(NextPoint2 == true)
        {
            NextPoint = false;
        }
    }

    void Idle()
    {
        if(IdleTimeCounter < 0)
        {
            Stop = false;
            ShouldStop = false;
        }
        else
        {
            IdleTimeCounter -= Time.deltaTime;

            if(ShouldStop == true)
            {
                Stop = true;
            }
        }
    }

    public void Restrain(bool Restraind)
    {
        if(IsRestraind == true)
        {
            CanCombat = Restraind;
        }
        else
        {
            CanCombat = true;
        }
    }

    public void Combat(bool EnterCombat)
    {
        if(EnterCombat == true && CanCombat == true)
        {
            ShouldStop = false;
            IsInCombat = true;
            Speed = CombatSpeed;
        }
        else
        {
            Speed = PatrolSpeed;
            IsInCombat = false;
        }
    }

    void Follow()
    {
        if(Stop == false)
        {
            EnemyRb.MovePosition((Vector2)transform.position + (Direction * Speed * Time.deltaTime));
            Direction = Player.position - transform.position;
            Direction.Normalize();
            Movement = Direction;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            ShouldStop = true;
            IdleTimeCounter = IdleTime;
        }
    }

    /*
    private Rigidbody2D EnemyRb;
    public Transform PatrolArea;

    public int CurrentHealth;

    public float PatrolSpeed;
    public float CombatSpeed;
    public float DashSpeed;
    private float Speed;
    private bool GeneralDirection = true;
    private bool PatrolDirection = true;
    private bool CombatDirection;
    private bool Stop;
    
    public float IdleTime;
    private float IdleTimeCounter;
    private bool CantCombat = true;

    private bool IsInCombat;
    private Transform Player;
    public float ChargeTime;
    private float ChargeTimeCounter;
    public float DashTime;
    private float DashTimeCounter;
    private bool DashCharge = false;
    private bool IsDashing;
    private bool ShouldTurn;

    public bool IsStationary;
    public Transform StationaryPoint;

    void Start()
    {
        Speed = PatrolSpeed;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(IsStationary == false)
        {
            Move();
        }
        else if(IsInCombat == true)
        {
            Speed = CombatSpeed;
            Dash();
            Move();
            Follow();
            Patrol();
        }

        if(IsDashing == false)
        {
            Idle();
        }

        Flip();

        if(IsDashing == false && IsStationary == false)
        {
            if(IsInCombat == true && CantCombat == false)
            {
                Speed = CombatSpeed;
                Follow();
            }
            else
            {
                Speed = PatrolSpeed;
                if(IsStationary == false)
                {
                    Patrol();
                }
            }
        }

        Health();

        if(ShouldTurn == false && IsStationary == false)
        {
            Dash();
        }

        Stationary();
    }

    public void CanCombat(bool Restraind)
    {
        if(Restraind == true)
        {
            CantCombat = false;
        }
        else
        {
            IdleTimeCounter = IdleTime;
            CantCombat = true;
        }
    }

    public void Combat(bool EnterCombat)
    {
        if(EnterCombat == true)
        {
            IsInCombat = true;
        }
        else if(IsDashing == false)
        {
            Speed = PatrolSpeed;
            IsInCombat = false;
        }
    }
    
    void Follow()
    {
        GeneralDirection = CombatDirection;
        
        if(IsDashing == false)
        {
            if(Player.position.x + 0.1 > transform.position.x && Player.position.x - 0.1 < transform.position.x)
            {
                Stop = true;
            }
            else if(Player.position.x > transform.position.x)
            {
                Stop = false;
                CombatDirection = true;
            }
            else if(Player.position.x < transform.position.x)
            {
                Stop = false;
                CombatDirection = false;
            }
        }
    }

    void Patrol()
    {
        if(PatrolArea.position.x < transform.position.x && ShouldTurn == true && GeneralDirection == true)
        {
            GeneralDirection = false;
        }
        
        if(PatrolArea.position.x > transform.position.x && ShouldTurn == true && GeneralDirection == false) 
        {
            GeneralDirection = true;
        }

        if(ShouldTurn == false && IsStationary == false)
        {
            GeneralDirection = PatrolDirection;
        }
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
            TimeBeforeIdle = Random.Range(6, 12);
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
            PatrolDirection = !PatrolDirection;
            IsDashing = false;
            ShouldTurn = true;
        }
        else
        {
            ShouldTurn = false;
        }
    }

    void Flip()
    {
        Vector3 Scaler = transform.localScale;
        if(GeneralDirection == true && Scaler.x < 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        if(GeneralDirection == false && Scaler.x > 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
    }

    void Move()
    {
        if(Stop == false)
        {
            if(GeneralDirection == true)
            {
                transform.Translate(new Vector2(1f, 0f) * Speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector2(1f, 0f) * Speed * (-1) * Time.deltaTime);
            }
        }
    }
    */
}
