using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class Beatle : MonoBehaviour, IEnemy
{
    private Animator Anim;
    public Animator SpellAnimation;
    private Transform Player;
    private Rigidbody2D EnemyRb;
    private int DefaultLayer;
    private Vector3 Scaler;
    public ParticleSystem DeathParticles;
    public ParticleSystem LandParticles;
    public ParticleSystem GallopParticles;

    private bool IsMelee;
    private bool Cooldown;
    private float CooldownTime;
    private bool IsDead;

    public float GallopSpeed;
    private bool IsGalloping;

    public float Speed;
    public float DashSpeed;
    public float FloatSpeed;

    private bool IsAttacking;
    private bool IsDashing;
    private bool FloatUp;
    private int AttackCount;
    private int Randomizer;
    private int JumpRand;
    private bool IsJumping;

    private float CurJumpSpeed;
    private float CurJumpHeight;

    public float JumpSpeed;
    public float JumpHeight;

    public float GrandJumpSpeed;
    public float GrandJumpHeight;

    private bool JumpDirection;
    private int WhichAttack;
    public GameObject[] AttackHitbox;
    public GameObject AttackPoint;

    public Transform SpellPoint;
    public Transform SpawnPoint;
    public float SpawnRate;
    private bool CanSpawn;
    public GameObject Bubble;
    public GameObject SpikeRight;
    public GameObject FastSpikeRight;
    public GameObject SpikeLeft;
    public GameObject FastSpikeLeft;
    public GameObject BurstObject;
    public Transform BurstPoint;

    private bool Direction;

    public Renderer MaterialRenderer;
    public Material[] Glows;

    private AudioSource AudioManager;
    public AudioSource AudioRun;
    public AudioClip Attack1Sound;
    public AudioClip Attack2Sound;
    public AudioClip Attack3Sound;
    public AudioClip Attack4Sound;
    public AudioClip Attack5Sound;
    public AudioClip Attack6Sound;
    public AudioClip Attack7Sound;
    public AudioClip SpikeSound;
    public AudioClip RunSound;

    private int Area;
    private bool FirstTurn;
    void Start()
    {
        MaterialRenderer.sharedMaterial = Glows[13];
        Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRb = GetComponent<Rigidbody2D>();
        DefaultLayer = LayerMask.NameToLayer("Dead");
        AudioManager = GetComponent<AudioSource>();
        CooldownTime = 8f;
    }
    void Update()
    {
        if (IsDead == false && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Stance") && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
        {
            GallopCooldown();

            if (IsAttacking == false && IsGalloping == false && !(Area == 2 && IsMelee))
            {
                Turn();
            }

            if(IsMelee && Area != 3)
            {
                ChooseAttack(WhichAttack);
            }

            Follow();
        }
        else
        {
            //EnemyRb.velocity = new Vector2(0f, 0f);
        }

        if(this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Stance"))
        {
            MaterialRenderer.sharedMaterial = Glows[12];
        }
    }

    public void Turn()
    {
        Scaler = transform.localScale;

        if (Player.transform.position.x < transform.position.x && Scaler.x > 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        if (Player.transform.position.x > transform.position.x && Scaler.x < 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Anim.SetTrigger("Stance");
            SpellAnimation.SetTrigger("Stance");
            LandParticles.Play();
            EnemyRb.gravityScale = 5;
        }

        if (other.gameObject.tag == "Finish")
        {
            int TurnRand = Random.Range(1, 11);

            if (1 <= TurnRand && TurnRand <= 5 && FirstTurn == false)
            {
                Turn();
                FirstTurn = true;
            }
            else
            {
                FirstTurn = false;
                CanSpawn = false;
                AttackHitbox[10].SetActive(false);
                Anim.SetBool("Attack 4", false);
                SpellAnimation.SetBool("Spell 4", false);
                IsGalloping = false;
                Anim.SetBool("IsGalloping", false);
                SpellAnimation.SetBool("IsGalloping", false);

                if (this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 4") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Gallop"))
                {
                    Anim.SetBool("Attack 5", false);
                    SpellAnimation.SetBool("Spell 5", false);
                    IsAttacking = false;
                }
            }
        }
    }

    public void Combat(bool EnterCombat, int AreaId)
    {
        IsMelee = EnterCombat;
        Area = AreaId;

        if(AreaId == 3 && this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Gallop"))
        {
            CurJumpSpeed = GrandJumpSpeed;
            CurJumpHeight = GrandJumpHeight;

            AttackHitbox[10].SetActive(false);
            Anim.SetBool("IsGalloping", false);
            SpellAnimation.SetBool("IsGalloping", false);
            Anim.SetBool("JumpAttack", true);
            SpellAnimation.SetBool("JumpAttack", true);
            MaterialRenderer.sharedMaterial = Glows[9];
        }
    }

    public void JumpCheck(bool ShouldJump)
    {
        if (ShouldJump == true)
        {
            JumpRand = Random.Range(1, 11);

            if (1 <= JumpRand && JumpRand <= 4)
            {
                CurJumpSpeed = JumpSpeed;
                CurJumpHeight = JumpHeight;

                WhichAttack = 2;
                ExecuteAttack();
            }
        }
    }

    void GallopCooldown()
    {
        if (CooldownTime > 0)
        {
            CooldownTime -= Time.deltaTime;
        }
        else
        {
            WhichAttack = Random.Range(4, 6);
            ExecuteAttack();
            CooldownTime = 12f;
        }
    }

    void LongCameraShake()
    {
        CameraShake.Instance.ShakeCamera(1f, 1f);
    }

    void ShortCameraShake()
    {
        CameraShake.Instance.ShakeCamera(1.5f, 0.2f);
    }

    void Follow()
    {
        if (IsAttacking == false && IsGalloping == false)
        {
            if (!AudioRun.isPlaying)
                AudioRun.Play();
            MaterialRenderer.sharedMaterial = Glows[0];
            Anim.SetBool("IsRunning", true);
            SpellAnimation.SetBool("IsRunning", true);

            if (Player.position.x + 0.1 > transform.position.x && Player.position.x - 0.1 < transform.position.x)
            {
                AudioRun.Stop();
                EnemyRb.linearVelocity = new Vector2(0f, 0f);
                Anim.SetBool("IsRunning", false);
                SpellAnimation.SetBool("IsRunning", false);
                MaterialRenderer.sharedMaterial = Glows[8];
            }
            else if (Player.transform.position.x < transform.position.x)
            {
                EnemyRb.linearVelocity = new Vector2(-Speed, 0f);
            }
            else if (Player.transform.position.x > transform.position.x)
            {
                EnemyRb.linearVelocity = new Vector2(Speed, 0f);
            }
        }
        else
        {
            AudioRun.Stop();
            Anim.SetBool("IsRunning", false);
            SpellAnimation.SetBool("IsRunning", false);
        }

        //if (IsAttacking == true && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        //{
        //    EnemyRb.velocity = new Vector2(0f, 0f);
        //}

        Scaler = transform.localScale;

        if (Scaler.x > 0)
        {
            Direction = true;
        }
        else
        {
            Direction = false;
        }

        if (IsDashing == true)
        {
            if (Scaler.x > 0)
            {
                EnemyRb.linearVelocity = new Vector2(DashSpeed, 0f);
            }
            else if (Scaler.x < 0)
            {
                EnemyRb.linearVelocity = new Vector2(-DashSpeed, 0f);
            }
        }

        if (IsJumping == true)
        {
            if (Scaler.x > 0)
            {
                EnemyRb.linearVelocity = new Vector2(CurJumpSpeed, CurJumpHeight);
            }
            else if (Scaler.x < 0)
            {
                EnemyRb.linearVelocity = new Vector2(-CurJumpSpeed, CurJumpHeight);
            }
        }

        if (IsGalloping == true)
        {
            if (Scaler.x > 0)
            {
                EnemyRb.linearVelocity = new Vector2(GallopSpeed, 0f);
            }
            else if (Scaler.x < 0)
            {
                EnemyRb.linearVelocity = new Vector2(-GallopSpeed, 0f);
            }
        }
    }

    void ChooseAttack(int PrevAttack)
    {
        if (!IsAttacking)
        {
            do
            {
                Randomizer = Random.Range(1, 101);

                if (Randomizer >= 1 && Randomizer <= 70 && IsMelee)
                {
                    if (Area == 0)
                    {
                        if (Randomizer >= 1 && Randomizer <= 50)
                        {
                            WhichAttack = 1;
                        }
                        else if (Randomizer >= 51 && Randomizer <= 70)
                        {
                            WhichAttack = 6;
                        }
                    }
                    else if (Area == 1)
                    {
                        if (Randomizer >= 1 && Randomizer <= 50)
                        {
                            WhichAttack = 3;
                        }
                        else if (Randomizer >= 51 && Randomizer <= 70)
                        {
                            WhichAttack = 6;
                        }
                    }
                    else if (Area == 2)
                    {
                        if (Randomizer >= 1 && Randomizer <= 15)
                        {
                            WhichAttack = 3;
                        }
                        else if (Randomizer >= 16 && Randomizer <= 30)
                        {
                            WhichAttack = 6;
                        }
                        else if (Randomizer >= 31 && Randomizer <= 70)
                        {
                            WhichAttack = 7;
                        }
                    }
                }
                else
                {
                    if (Randomizer >= 71 && Randomizer <= 85)
                    {
                        WhichAttack = 4;
                    }
                    else if (Randomizer >= 86 && Randomizer <= 100)
                    {
                        WhichAttack = 5;
                    }
                }

            } while (WhichAttack == PrevAttack);
        }

        ExecuteAttack();
    }

    void ExecuteAttack()
    {
        if (!IsAttacking)
        {
            switch (WhichAttack)
            {
                case 1: Attack1(); break;
                case 2: Attack2(); break;
                case 3: Attack3(); break;
                case 4: Attack4(); break;
                case 5: Attack5(); break;
                case 6: Attack6(); break;
                case 7: Attack7(); break;
            }
        }
    }

    void Attack1()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[1];
        Anim.SetBool("Attack 1", true);
        SpellAnimation.SetBool("Spell 1", true);
    }

    void Attack2()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[2];
        Anim.SetBool("Attack 2", true);
        SpellAnimation.SetBool("Spell 2", true);
    }

    void Attack3()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[3];
        Anim.SetBool("Attack 3", true);
        SpellAnimation.SetBool("Spell 3", true);
    }

    void Attack4()
    {
        IsAttacking = true;

        Scaler = transform.localScale;

        if (transform.position.x > 0 && Scaler.x > 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
        else if (transform.position.x < 0 && Scaler.x < 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[4];
        Anim.SetBool("Attack 4", true);
        SpellAnimation.SetBool("Spell 4", true);
    }

    void Attack5()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[5];
        Anim.SetBool("Attack 5", true);
        SpellAnimation.SetBool("Spell 5", true);
    }

    void Attack6()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[6];
        Anim.SetBool("Attack 6", true);
        SpellAnimation.SetBool("Spell 6", true);
    }

    void Attack7()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[7];
        Anim.SetBool("Attack 7", true);
        SpellAnimation.SetBool("Spell 7", true);
    }

    private IEnumerator SpawnSpikes()
    {
        if (CanSpawn == true)
            if(Direction)
            {
                Instantiate(FastSpikeRight, SpawnPoint.position, SpawnPoint.rotation);
            }
            else
            {
                Instantiate(SpikeRight, SpawnPoint.position, SpawnPoint.rotation);
            }
            AudioManager.PlayOneShot(SpikeSound);
        yield return new WaitForSeconds(SpawnRate);
        if (CanSpawn == true)
            if(Direction)
            {
                Instantiate(SpikeLeft, SpawnPoint.position, SpawnPoint.rotation);
            }
            else
            {
                Instantiate(FastSpikeLeft, SpawnPoint.position, SpawnPoint.rotation);
            }
        yield return new WaitForSeconds(SpawnRate);
        if (CanSpawn == true)
        {
            StartCoroutine(SpawnSpikes());
        }
    }

    void SpawnHitbox(int AttackId)
    {
        AttackHitbox[AttackId].SetActive(true);
    }

    void DespawnHitbox(int AttackId)
    {
        AttackHitbox[AttackId].SetActive(false);
    }
    void StopAttack(int AttackId)
    {
        IsAttacking = false;
        Anim.SetBool("Attack " + AttackId, false);
        SpellAnimation.SetBool("Spell " + AttackId, false);
        Anim.SetBool("JumpAttack", false);
        SpellAnimation.SetBool("JumpAttack", false);
    }

    void StartJump()
    {
        IsJumping = true;
        GallopParticles.Play();
    }

    void StopJump()
    {
        IsJumping = false;
        if(this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 5"))
        {
            EnemyRb.gravityScale = 5.75f;
        }
        CurJumpSpeed = JumpSpeed;
        CurJumpHeight = JumpHeight;
    }

    void StartGallop(int Id)
    {
        Anim.SetBool("IsGalloping", true);
        SpellAnimation.SetBool("IsGalloping", true);
        if (Id == 1)
        {
            MaterialRenderer.sharedMaterial = Glows[8];
            StartCoroutine(SpawnSpikes());
        }
        else
        {
            MaterialRenderer.sharedMaterial = Glows[11];
        }
        CanSpawn = true;
    }

    void StopGallop()
    {
        IsGalloping = false;
    }

    void AdvanceGallop()
    {
        AttackHitbox[10].SetActive(true);
        IsGalloping = true;
    }

    void GallopPart()
    {
        GallopParticles.Play();
    }

    void Death()
    {
        IsDead = true;
        DeathParticles.Play();
        gameObject.layer = DefaultLayer;
    }
}
