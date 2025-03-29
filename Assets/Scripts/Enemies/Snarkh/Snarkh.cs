using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snarkh : MonoBehaviour
{
    private Animator Anim;

    private Rigidbody2D EnemyRb;
    private Transform Player;
    private int DefaultLayer;
    private Vector3 Scaler;
    public ParticleSystem DeathParticles;

    public float Speed;
    public float FollowSpeed;
    public float DashSpeed;

    private bool IsInCombat;
    private bool CanAttack;
    private bool IsRestrained;
    private bool IsAttacking;
    private bool CanTurn = true;
    private bool CanFollow;
    private bool IsDead;
    public GameObject Collider;

    private AudioSource AudioManager;
    public AudioSource AudioManager2;
    public AudioClip RunSound;
    public AudioClip WalkSound;
    public AudioClip ChargeSound;
    public AudioClip DashSound;
    public AudioClip DeathSound;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        DefaultLayer = LayerMask.NameToLayer("Dead");
        Scaler = transform.localScale;
        AudioManager = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (IsDead == false)
        {
            Patrol();

            if (IsInCombat == true && IsRestrained == false && CanFollow == true)
            {
                if (CanTurn == true)
                {
                    Turn();
                }
                Follow();
            }

            if (IsRestrained == true)
            {
                StartCoroutine(Restrain());
                IsRestrained = false;
            }
        }
        else
        {
            StopCoroutine(Dash());
            StopCoroutine(Restrain());
            EnemyRb.velocity = new Vector2(0f, 0f);
            Collider.SetActive(false);
            CanTurn = true;
            IsAttacking = false;
            Anim.SetBool("IsDashing", false);
        }
    }

    void Turn()
    {
        Scaler = transform.localScale;

        if (Player.transform.position.x < transform.position.x && Scaler.x < 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        if (Player.transform.position.x > transform.position.x && Scaler.x > 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
    }

    public void Combat(bool EnterCombat)
    {
        IsInCombat = EnterCombat;
    }

    public void StartAttack(bool StartCharge)
    {
        CanAttack = StartCharge;

        if(CanAttack == true && IsAttacking == false && IsRestrained == false && IsDead == false)
        {
            StartCoroutine(Dash());
        }
    }

    void Patrol()
    {
        if(CanFollow == false && IsRestrained == false && IsAttacking == false)
        {
            AudioManager2.clip = WalkSound;
            if (!AudioManager2.isPlaying)
                AudioManager2.Play();
            Anim.SetBool("IsRuning", true);
            EnemyRb.velocity = new Vector2(-Speed * Scaler.x, 0f);
        }
    }

    public void CanPatrol(bool CanPatrol)
    {
        CanFollow = CanPatrol;
    }

    public void StartRestrain(bool Rstr)
    {
        IsRestrained = Rstr;
    }

    IEnumerator Restrain()
    {
        StopCoroutine(Dash());
        Collider.SetActive(false);
        CanTurn = true;
        IsAttacking = false;
        Anim.SetBool("IsDashing", false);
        Anim.SetBool("IsRuning", true);
        Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        AudioManager2.clip = RunSound;
        if (!AudioManager2.isPlaying)
            AudioManager2.Play();
        EnemyRb.velocity = new Vector2(-FollowSpeed * Scaler.x, 0f);
        yield return new WaitForSeconds(1f);
        IsRestrained = false;
    }

    void Follow()
    {
        if (IsInCombat == true && IsRestrained == false && IsAttacking == false)
        {
            AudioManager2.clip = RunSound;
            if (!AudioManager2.isPlaying)
                AudioManager2.Play();
            Anim.SetBool("IsRuning", true);

            if (Player.position.x + 0.1 > transform.position.x && Player.position.x - 0.1 < transform.position.x)
            {
                EnemyRb.velocity = new Vector2(0f, 0f);
                Anim.SetBool("IsRuning", false);
                AudioManager2.Stop();
            }
            else if (Player.transform.position.x < transform.position.x)
            {
                EnemyRb.velocity = new Vector2(-FollowSpeed, 0f);
            }
            else if (Player.transform.position.x > transform.position.x)
            {
                EnemyRb.velocity = new Vector2(FollowSpeed, 0f);
            }
        }
        else
        {
            AudioManager2.Stop();
            Anim.SetBool("IsRuning", false);
        }
    }

    IEnumerator Dash()
    {
        IsAttacking = true;
        EnemyRb.velocity = new Vector2(0f, 0f);
        Anim.SetBool("IsChargeing", true);
        AudioManager.PlayOneShot(ChargeSound, 0.4f);
        yield return new WaitForSeconds(0.9375f);
        Collider.SetActive(true);
        AudioManager.PlayOneShot(DashSound, 0.4f);
        if (IsRestrained == false)
        {
            Anim.SetBool("IsDashing", true);
        }
        Anim.SetBool("IsChargeing", false);
        CanTurn = false;
        EnemyRb.velocity = new Vector2(-DashSpeed*Scaler.x, 0f);
        yield return new WaitForSeconds(0.75f);
        Collider.SetActive(false);
        Anim.SetBool("IsDashing", false);
        CanTurn = true;
        IsAttacking = false;
    }

    void Death()
    {
        AudioManager.PlayOneShot(DeathSound, 0.4f);
        AudioManager2.volume = 0f;
        IsDead = true;
        DeathParticles.Play();
        gameObject.layer = DefaultLayer;
    }

    /*
    private Animator Anim;

    private Rigidbody2D EnemyRb;
    private Transform Player;
    private bool CanMove = true;
    public bool IsStatonary;
    private int DefaultLayer;

    private float TimeBeforeIdle = 3f;
    public float IdleTime;
    private float IdleTimeCounter;

    private bool ShouldTurn;
    private bool GeneralDirection;

    private float Speed;
    private bool Stop;

    public Transform PatrolArea;
    public float PatrolSpeed;
    private bool PatrolDirection = false;

    public int Damage;
    public float CombatSpeed;
    private bool CombatDirection;
    private bool CanCombat;
    private bool IsInCombat;
    public GameObject DamageCollider;

    public float DashSpeed;
    private bool IsDashing;
    private bool DashCharge;
    public float DashTime;
    private float DashTimeCounter;
    public float ChargeTime;
    private bool StartDashing;
    private float RestrainTime = 1f;
    private float RestrainTimeCounter;
    private bool RestrainSpeed;
    private bool DashDirection;
    private bool DidDamage;

    void Start()
    {
        Speed = PatrolSpeed;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        DefaultLayer = LayerMask.NameToLayer("Dead");
    }

    void Update()
    {
        if(RestrainTimeCounter > 0)
        {
            DashTimeCounter = 0;
            if(RestrainSpeed == true)
            {
                Speed = CombatSpeed;
            }
        }

        if(CanMove == true)
        {
            Idle();
            Flip();

            if((IsInCombat == false || RestrainTimeCounter > 0) && DashTimeCounter <= 0)
            {
                Anim.speed = 0.7f;
                Patrol();
            }   
            else
            {
                Anim.speed = 1f;

                if(IsDashing == false)
                {
                    Follow();
                }
            }
        
            Dash();
        }
    }

    void FixedUpdate()
    {
        if(IsStatonary == false && CanMove == true)
        {
            Move();
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
        if((transform.position.x > Player.transform.position.x - 0.1f && transform.position.x < Player.transform.position.x + 0.1f) || IsDashing == true)
        {
            Anim.SetBool("IsRuning", false);
        }
        else
        {
             Anim.SetBool("IsRuning", true);
        }

        if(Stop == false)
        {
            if(GeneralDirection == true)
            {
                EnemyRb.velocity = new Vector2(Speed, 0f);
            }
            else
            {
                EnemyRb.velocity = new Vector2(-Speed, 0f);
            }
        }
        else
        {
            EnemyRb.velocity = new Vector2(0f, 0f);
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

    void Follow()
    {
        GeneralDirection = CombatDirection;
        Speed = CombatSpeed;
        
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

    public void CombatRestrain(bool CombatRestraind)
    {
        if(CombatRestraind == true)
        {
            CanCombat = true;
        }
        else
        {
            IdleTimeCounter = IdleTime;
            CanCombat = false;
        }
    }

    public void DashRestrain(bool DashRestraind)
    {
        if (DashRestraind == false)
        {
            RestrainTimeCounter = RestrainTime;
            IdleTimeCounter = IdleTime;
        }
    }

    public void Combat(bool EnterCombat)
    {
        RestrainSpeed = EnterCombat;

        if(EnterCombat == true && CanCombat == true && RestrainTimeCounter <= 0)
        {
            IsInCombat = true;
        }
        else if(DashTimeCounter <= 0)
        {
            Speed = PatrolSpeed;
            IsInCombat = false;
        }
    }

    public void StartAttack(bool StartCharge)
    {
        if(StartCharge == true && CanCombat == true)
        {
            if(IsStatonary == true)
            {
                DashTimeCounter = DashTime;
            }
            EnemyRb.velocity = new Vector2(0f, 0f);

            IsStatonary = false;
            IsDashing = true;
            StartDashing = true;
        }
        else
        {
            StartDashing = false;
        }
    }

    public void DashDamage(int Damage, bool HitSheild)
    {
        if(Speed == DashSpeed && CanMove == true)
        {
            if(DidDamage == false)
            {
                Player.GetComponent<PlayerController>().AccesHealth(Damage, 0, 0, HitSheild);
                DidDamage = true;
            }
        }
    }

    void Dash()
    {
        if(Speed == DashSpeed && CanMove == true)
        {
            DamageCollider.SetActive(true);
        }
        else
        {
            DamageCollider.SetActive(false);
        }

        if(RestrainTimeCounter > 0)
        {
            RestrainTimeCounter -= Time.deltaTime;
        }

        if(StartDashing == true && DashTimeCounter <= 0)
        {
            DashTimeCounter = ChargeTime;
            DidDamage = false;
        }

        if(DashTimeCounter == ChargeTime)
        {
            StartDashing = false;
        }

        if(DashTimeCounter > 0)
        {
            if(DashTimeCounter > DashTime)
            {
                if(RestrainTimeCounter <= 0)
                {
                    Anim.SetBool("IsChargeing", true);
                    Stop = true;

                    if(Player.position.x < transform.position.x)
                    {
                        GeneralDirection = false;
                        DashDirection = false;
                    }
                    else
                    {
                        GeneralDirection = true;
                        DashDirection = true;
                    }
                }
            }
            else
            {
                if(RestrainTimeCounter <= 0 && DashTimeCounter < DashTime)
                {   
                    Anim.SetBool("IsChargeing", false);
                    Anim.SetBool("IsDashing", true);
                    GeneralDirection = DashDirection;
                    Speed = DashSpeed;
                }
            }

            DashTimeCounter -= Time.deltaTime;
        }
        else
        {
            Anim.SetBool("IsDashing", false);
            IsDashing = false;
        }
    }

    void Death()
    {
        CanMove = false;
        EnemyRb.velocity = new Vector2(0f, 0f);
        gameObject.layer = DefaultLayer;
    }
    */
}