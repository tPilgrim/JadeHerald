using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : MonoBehaviour
{
    private Animator Anim;
    private Transform Player;
    private Rigidbody2D EnemyRb;
    private int DefaultLayer;
    public ParticleSystem DeathParticles;

    private float Speed;
    public float DashSpeed;
    public float FollowSpeed;
    private bool CanMove = true;

    public float JumpForce;
    public float JumpSpeed;
    private bool IsJumping;
    private bool JumpAgain = true;
    private bool CanJump;
    public GameObject JumpCheck;

    private float SleepTimeCounter;
    public float SleepTime;
    private bool StartCounting;

    private bool CanCombat;
    private int WhichAttack;
    private bool NextAttack = true;
    public GameObject AttackHitbox;
    public GameObject DashHitbox;
    private bool IsDashing;
    private bool IsChargeing;
    private bool DashDirection;
    private int AttackCount;
    private int DashCount;

    private AudioSource AudioManager;
    public AudioSource Footsteps;
    public AudioClip AttackSound;
    public AudioClip AwakeSound;
    public AudioClip DashSound;
    public AudioClip JumpSound;
    public AudioClip LandSound;
    public AudioClip DeathSound;
    public ParticleSystem AwakeParticles;

    void Start()
    {
        Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRb = GetComponent<Rigidbody2D>();
        DefaultLayer = LayerMask.NameToLayer("Dead");
        AudioManager = GetComponent<AudioSource>();
    }

    void Update()
    {
        Sleep();
        Jump();

        if (this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Death"))
        {
            DashSpeed = 0;
        }

        if (!this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Sleep") && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Death") && IsDashing == false && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Awake") && IsJumping == false)
        {
            Turn();
        }

        if (IsDashing == true)
        {
            if(DashDirection == true)
            {
                EnemyRb.linearVelocity = new Vector2(-DashSpeed, 0f);
            }
            else
            {
                EnemyRb.linearVelocity = new Vector2(DashSpeed, 0f);
            }
        }

        if(this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Awake") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Attack") || IsChargeing == true || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Sleep") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Death"))
        {
            CanMove = false;
        }
        else
        {
            CanMove = true;
        }

        if (EnemyRb.linearVelocity.y < 0)
        {
            Anim.SetBool("IsRunning", false);
        }
    }

    void FixedUpdate()
    {
         Follow();
    }

    public void Combat(bool EnterCombat)
    {
        if(EnterCombat == true)
        {
            CanCombat = EnterCombat;
            Anim.SetTrigger("IsAwake");
            Anim.SetBool("IsSleeping", false);
            SleepTimeCounter = SleepTime;
            StartCounting = false;
        }
        else
        {
            StartCounting = true;
        }
    }

    private bool FirstPlay = true;

    void PlayAwakeSound()
    {
        if (!FirstPlay)
        {
            AwakeParticles.Play();
            AudioManager.PlayOneShot(AwakeSound, 0.6f);
        }
        FirstPlay = false;
    }

    void Turn()
    {
        Vector3 Scaler = transform.localScale;

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

    void Follow()
    {
        if (this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Run"))
        {
            if (!Footsteps.isPlaying)
                Footsteps.Play();
        }
        else
        {
            Footsteps.Stop();
        }

        if (CanCombat == true && IsDashing == false && EnemyRb.linearVelocity.y >= 0 && IsJumping == false && JumpAgain == true)
        {
            Speed = FollowSpeed;
            Anim.SetBool("IsRunning", true);

            if((Player.position.x + 0.1 > transform.position.x && Player.position.x - 0.1 < transform.position.x) || CanMove == false)
            {
                Speed = 0;
                Anim.SetBool("IsRunning", false);
            }
            else if(Player.transform.position.x < transform.position.x)
            {
                Speed = -FollowSpeed;
            }
            else if(Player.transform.position.x > transform.position.x)
            {
                Speed = FollowSpeed;
            }

            EnemyRb.linearVelocity = new Vector2(Speed, 0f);
        }
    }

    public void StartJump(bool ShouldJump)
    {
        CanJump = ShouldJump;
    }

    void Jump()
    {
        if (Player.transform.position.x < transform.position.x)
        {
            JumpSpeed = Mathf.Abs(JumpSpeed) * -1f;
        }
        else
        {
            JumpSpeed = Mathf.Abs(JumpSpeed);
        }

        if (CanJump == true && JumpAgain == true && IsJumping == false && CanMove == true && EnemyRb.linearVelocity.y >= 0 && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Dash"))
        {
            StartCoroutine(JumpStart());
        }

        if (CanJump == false)
        {
            JumpAgain = true;
        }
        else
        {
            Anim.SetBool("IsRunning", false);
        }
    }

    IEnumerator JumpStart()
    {
        JumpAgain = false;
        IsJumping = true;
        Anim.SetBool("IsJumping", true);
        EnemyRb.linearVelocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(0.4f);
        AudioManager.PlayOneShot(JumpSound, 0.6f);
        EnemyRb.linearVelocity = new Vector2(JumpSpeed, JumpForce);
        yield return new WaitForSeconds(0.3f);
        EnemyRb.linearVelocity = new Vector2(JumpSpeed, EnemyRb.linearVelocity.y);
        yield return new WaitForSeconds(0.3f);
        EnemyRb.linearVelocity = new Vector2(JumpSpeed, EnemyRb.linearVelocity.y);
        yield return new WaitForSeconds(0.25f);
        EnemyRb.linearVelocity = new Vector2(0f, EnemyRb.linearVelocity.y);
        IsJumping = false;
        Anim.SetBool("IsJumping", false);
    }

    void Sleep()
    {
        if(StartCounting == true)
        {
            SleepTimeCounter -= Time.deltaTime;
        }

        if(SleepTimeCounter <= 0)
        {
            Anim.SetBool("IsSleeping", true);
        }
    }

    public void StartAttack(bool CanAttack)
    {
        if(CanAttack == true && NextAttack == true && IsJumping == false)
        {
            if (WhichAttack < 5)
            {
                NextAttack = false;
                
                if(DashCount < 2)
                {
                    DashCount++;
                    Anim.SetTrigger("IsDashing");
                }
                else
                {
                    DashCount = 0;
                    Anim.SetTrigger("IsAttacking");
                }
            }
            else
            {
                NextAttack = false;

                if (AttackCount < 2)
                {
                    AttackCount++;
                    Anim.SetTrigger("IsAttacking");
                }
                else
                {
                    AttackCount = 0;
                    Anim.SetTrigger("IsDashing");
                }
            }
        }
    }

    IEnumerator Slash()
    {
        AttackHitbox.SetActive(true);
        AudioManager.PlayOneShot(AttackSound, 0.4f);
        yield return new WaitForSeconds(0.1f);
        AttackHitbox.SetActive(false);
    }

    void Charge()
    {
        IsChargeing = true;
    }

    void StartDash()
    {
        if(Player.transform.position.x < transform.position.x)
        {
            DashDirection = true;
        }

        if(Player.transform.position.x > transform.position.x)
        {
            DashDirection = false;
        }

        IsChargeing = false;
        JumpCheck.SetActive(false);
        AudioManager.PlayOneShot(DashSound, 0.4f);
        DashHitbox.SetActive(true);
        IsDashing = true;
    }

    void CanAttackAgain()
    {
        IsDashing = false;
        DashHitbox.SetActive(false);
        JumpCheck.SetActive(true);
        WhichAttack = Random.Range(1, 10);
        NextAttack = true;
    }

    private bool FirstCollision = true;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            if (!FirstCollision)
            {
                AudioManager.PlayOneShot(LandSound, 0.4f);
            }
            FirstCollision = false;
        }
    }

    void Death()
    {
        AudioManager.PlayOneShot(DeathSound, 0.4f);
        DeathParticles.Play();
        gameObject.layer = DefaultLayer;
    }
}
