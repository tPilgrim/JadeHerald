using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    private Animator Anim;
    public Animator SpellAnimation;
    private Transform Player;
    private Rigidbody2D EnemyRb;
    private int DefaultLayer;
    private Vector3 Scaler;
    public ParticleSystem DeathParticles;
    public ParticleSystem LandParticles;
    public ParticleSystem BurstParticles;
    public ParticleSystem StruckParticles;

    private bool IsMelee;
    private bool Cooldown;
    private float CooldownTime;
    private bool CanTurn;
    private bool IsDead;

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
    public float JumpSpeed;
    public float JumpHeight;
    private bool JumpDirection;
    private int WhichAttack;
    //private int PrevAttack = -1;
    public GameObject[] AttackHitbox;
    public GameObject AttackPoint;

    public Transform SpellPoint;
    public Transform SpawnPoint;
    public float SpawnRate;
    private bool CanSpawn;
    public GameObject Bubble;
    public GameObject SpikeRight;
    public GameObject SpikeLeft;
    public GameObject BurstObject;
    public Transform BurstPoint;

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

    private bool StartFight = false;
    public Animator Ring1Anim;
    public Animator Ring2Anim;
    public Animator Ring3Anim;
    public Animator Ring4Anim;
    public AudioSource RingAudio;

    void Start()
    {
        MaterialRenderer.sharedMaterial = Glows[8];
        Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRb = GetComponent<Rigidbody2D>();
        DefaultLayer = LayerMask.NameToLayer("Dead");
        AudioManager = GetComponent<AudioSource>();
        CooldownTime = 10f;
    }

    void Update()
    {
        if (IsDead == false)
        {
            if (StartFight)
            {
                SpellCooldown();
                Follow();
                Turn();
                if (IsMelee)
                {
                    ChooseAttack(WhichAttack);
                }
            }
        }
        else
        {
            EnemyRb.velocity = new Vector2(0f, 0f);
            AttackPoint.SetActive(false);
        }
    }

    void Turn()
    {
        Scaler = transform.localScale;

        if (IsAttacking == false && IsDashing == false || CanTurn == true)
        {
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
    }

    void AttackTurn()
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            LandParticles.Play();
        }
    }

    void Burst()
    {
        BurstParticles.Play();
    }

    void Struck()
    {
        StruckParticles.Play();
    }

    public void Initiate()
    {
        Anim.SetTrigger("Initiate");
        SpellAnimation.SetTrigger("Initiate");
        StartCoroutine(StartInitiate());
    }

    public void Combat(bool EnterCombat)
    {
        IsMelee = EnterCombat;
    }

    public void StartJump(bool ShouldJump)
    {
        if(ShouldJump == true)
        {
            JumpRand = Random.Range(1, 11);

            if(1 <= JumpRand && JumpRand <= 4)
            {
                WhichAttack = 2;
                ExecuteAttack();
            }
        }
    }

    void CastBubble()
    {
        Instantiate(Bubble, SpellPoint.position, SpellPoint.rotation);
    }

    void SpellCooldown()
    {
        if(CooldownTime > 0) 
        {
            Cooldown = true;
            CooldownTime -= Time.deltaTime;
        }
        else
        {
            Cooldown = false;
            ChooseAttack(WhichAttack);
        }
    }

    void ActivateCameraShake()
    {
        CameraShake.Instance.ShakeCamera(2f, 0.2f);
    }

    void Follow()
    {
        if (IsAttacking == false && IsDashing == false && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
        {
            if (!AudioRun.isPlaying)
                AudioRun.Play();
            MaterialRenderer.sharedMaterial = Glows[0];
            Anim.SetBool("IsRunning", true);
            SpellAnimation.SetBool("Spell Run", true);

            if (Player.position.x + 0.1 > transform.position.x && Player.position.x - 0.1 < transform.position.x && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
            {
                AudioRun.Stop();
                EnemyRb.velocity = new Vector2(0f, 0f);
                Anim.SetBool("IsRunning", false);
                SpellAnimation.SetBool("Spell Run", false);
            }
            else if (Player.transform.position.x < transform.position.x)
            {
                EnemyRb.velocity = new Vector2(-Speed, 0f);
            }
            else if (Player.transform.position.x > transform.position.x)
            {
                EnemyRb.velocity = new Vector2(Speed, 0f);
            }
        }
        else
        {
            AudioRun.Stop();
            Anim.SetBool("IsRunning", false);
            SpellAnimation.SetBool("Spell Run", false);
        }

        if (IsAttacking == true && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            EnemyRb.velocity = new Vector2(0f, 0f);
        }

        Scaler = transform.localScale;

        if (IsDashing == true)
        {
            if (Scaler.x > 0)
            {
                EnemyRb.velocity = new Vector2(DashSpeed, 0f);
            }
            else if (Scaler.x < 0)
            {
                EnemyRb.velocity = new Vector2(-DashSpeed, 0f);
            }
        }

        if (IsJumping == true)
        {
            if (JumpDirection == true)
            {
                EnemyRb.velocity = new Vector2(-JumpSpeed, JumpHeight);
            }
            else
            {
                EnemyRb.velocity = new Vector2(JumpSpeed, JumpHeight);
            }
        }

        if (FloatUp == true)
        {
            EnemyRb.velocity = new Vector2(EnemyRb.velocity.x, FloatSpeed);
        }
    }

    void ChooseAttack(int PrevAttack)
    {
        if(!IsAttacking)
        {
            do
            {
                Randomizer = Random.Range(1, 101);

                if (IsMelee == true)
                {
                    if (Randomizer >= 1 && Randomizer <= 35)
                    {
                        WhichAttack = 1;
                    }
                    else if (Randomizer >= 36 && Randomizer <= 60)
                    {
                        WhichAttack = 3;
                    }
                    else if (Randomizer >= 61 && Randomizer <= 80)
                    {
                        WhichAttack = 5;
                    }
                    else if (Randomizer >= 81 && Randomizer <= 100)
                    {
                        WhichAttack = 6;
                    }
                }
                else
                {
                    if (Randomizer >= 1 && Randomizer <= 60)
                    {
                        WhichAttack = 4;
                    }
                    else if (Randomizer >= 61 && Randomizer <= 100)
                    {
                        WhichAttack = 7;
                    }
                }
            } while (WhichAttack == PrevAttack);
        }

        ExecuteAttack();
    }

    void ExecuteAttack()
    {
        if(!IsAttacking)
        {
            switch (WhichAttack)
            {
                case 1: StartCoroutine(Attack1()); break;
                case 2: StartCoroutine(Attack2()); break;
                case 3: StartCoroutine(Attack3()); break;
                case 4: if (Cooldown == false) StartCoroutine(Attack4()); break;
                case 5: StartCoroutine(Attack5()); break;
                case 6: StartCoroutine(Attack6()); break;
                case 7: if (Cooldown == false) StartCoroutine(Attack7()); break;
            }
        }
    }

    private IEnumerator Attack1()
    {
        IsAttacking = true;
        AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[1];
        Anim.SetBool("Attack 1", true);
        SpellAnimation.SetBool("Spell 1", true);

        CanTurn = true;
        yield return new WaitForSeconds(0.5f);
        CanTurn = false;
        yield return new WaitForSeconds(0.333f);
        if (IsDead == false)
        {
            AttackHitbox[0].SetActive(true);
        }
        yield return new WaitForSeconds(0.2f);
        AttackHitbox[0].SetActive(false);
        yield return new WaitForSeconds(0.633f);
        if (IsDead == false)
        {
            AttackHitbox[1].SetActive(true);
        }
        yield return new WaitForSeconds(0.2f);
        AttackHitbox[1].SetActive(false);
        yield return new WaitForSeconds(0.436f);

        SpellAnimation.SetBool("Spell 1", false);
        Anim.SetBool("Attack 1", false);
        IsAttacking = false;
    }

    private IEnumerator Attack2()
    {
        IsAttacking = true;
        AudioManager.PlayOneShot(Attack2Sound);
        MaterialRenderer.sharedMaterial = Glows[2];
        Anim.SetBool("Attack 2", true);
        SpellAnimation.SetBool("Spell 2", true);

        yield return new WaitForSeconds(0.333f);
        if (Player.transform.position.x < transform.position.x)
        {
            JumpDirection = true;
        }
        else if (Player.transform.position.x > transform.position.x)
        {
            JumpDirection = false;
        }
        IsJumping = true;
        yield return new WaitForSeconds(0.25f);
        IsJumping = false;
        yield return new WaitForSeconds(0.166f);
        if (IsDead == false)
        {
            AttackHitbox[0].SetActive(true);
        }
        yield return new WaitForSeconds(0.2f);
        AttackHitbox[0].SetActive(false);
        yield return new WaitForSeconds(0.882f);
        if (IsDead == false)
        {
            AttackHitbox[2].SetActive(true);
        }
        yield return new WaitForSeconds(0.2f);
        AttackHitbox[2].SetActive(false);
        yield return new WaitForSeconds(0.206f);

        SpellAnimation.SetBool("Spell 2", false);
        Anim.SetBool("Attack 2", false);
        IsAttacking = false;
    }

    private IEnumerator Attack3()
    {
        IsAttacking = true;
        AudioManager.PlayOneShot(Attack3Sound);
        MaterialRenderer.sharedMaterial = Glows[3];
        Anim.SetBool("Attack 3", true);
        SpellAnimation.SetBool("Spell 3", true);

        CanTurn = true;
        yield return new WaitForSeconds(0.748f);
        IsDashing = true;
        FloatUp = true;
        CanTurn = false;
        if (IsDead == false)
        {
            AttackHitbox[3].SetActive(true);
        }
        yield return new WaitForSeconds(0.15f);
        FloatUp = false;
        AttackHitbox[3].SetActive(false);
        yield return new WaitForSeconds(0.0833f);
        IsDashing = false;
        yield return new WaitForSeconds(0.326f);
        CanTurn = true;
        yield return new WaitForSeconds(0.573f);
        CanTurn = false;

        SpellAnimation.SetBool("Spell 3", false);
        Anim.SetBool("Attack 3", false);
        IsAttacking = false;
    }

    private IEnumerator Attack4()
    {
        IsAttacking = true;
        AudioManager.PlayOneShot(Attack4Sound);
        MaterialRenderer.sharedMaterial = Glows[4];
        Anim.SetBool("Attack 4", true);
        SpellAnimation.SetBool("Spell 4", true);

        EnemyRb.gravityScale = 2;
        FloatUp = true;
        yield return new WaitForSeconds(0.916f);
        CanSpawn = true;
        StartCoroutine(SpawnSpikes());
        FloatUp = false;
        EnemyRb.gravityScale = 0;
        yield return new WaitForSeconds(2.246f);
        CanSpawn = false;
        StopCoroutine(SpawnSpikes());
        EnemyRb.gravityScale = 20;
        yield return new WaitForSeconds(1.083f);
        EnemyRb.gravityScale = 5;

        SpellAnimation.SetBool("Spell 4", false);
        Anim.SetBool("Attack 4", false);
        IsAttacking = false;
        Cooldown = true;
        CooldownTime = 10f;
    }

    private IEnumerator Attack5()
    {
        IsAttacking = true;
        AudioManager.PlayOneShot(Attack5Sound);
        MaterialRenderer.sharedMaterial = Glows[5];
        Anim.SetBool("Attack 5", true);
        SpellAnimation.SetBool("Spell 5", true);

        IsDashing = true;
        FloatUp = true;
        yield return new WaitForSeconds(0.1f);
        FloatUp = false;
        yield return new WaitForSeconds(0.316f);
        IsDashing = false;
        yield return new WaitForSeconds(0.082f);
        Cooldown = false;

        SpellAnimation.SetBool("Spell 5", false);
        Anim.SetBool("Attack 5", false);
        IsAttacking = false;
    }

    private IEnumerator Attack6()
    {
        IsAttacking = true;
        AudioManager.PlayOneShot(Attack6Sound);
        MaterialRenderer.sharedMaterial = Glows[6];
        Anim.SetBool("Attack 6", true);
        SpellAnimation.SetBool("Spell 6", true);

        yield return new WaitForSeconds(1.66f);
        if (IsDead == false)
        {
            AttackHitbox[4].SetActive(true);
        }
        yield return new WaitForSeconds(0.166f);
        AttackHitbox[5].SetActive(true);
        AttackHitbox[4].SetActive(false);
        yield return new WaitForSeconds(0.166f);
        AttackHitbox[6].SetActive(true);
        AttackHitbox[5].SetActive(false);
        yield return new WaitForSeconds(0.166f);
        AttackHitbox[7].SetActive(true);
        AttackHitbox[6].SetActive(false);
        yield return new WaitForSeconds(0.166f);
        AttackHitbox[8].SetActive(true);
        AttackHitbox[7].SetActive(false);
        yield return new WaitForSeconds(0.166f);
        AttackHitbox[8].SetActive(false);
        yield return new WaitForSeconds(0.45f);

        SpellAnimation.SetBool("Spell 6", false);
        Anim.SetBool("Attack 6", false);
        IsAttacking = false;
    }

    private IEnumerator Attack7()
    {
        IsAttacking = true;
        AudioManager.PlayOneShot(Attack7Sound);
        MaterialRenderer.sharedMaterial = Glows[7];
        Anim.SetBool("Attack 7", true);
        SpellAnimation.SetBool("Spell 7", true);
        yield return new WaitForSeconds(1.666f);
        StartCoroutine(BurstAttack());
        yield return new WaitForSeconds(0.49f);
        SpellAnimation.SetBool("Spell 7", false);
        Anim.SetBool("Attack 7", false);
        IsAttacking = false;
        Cooldown = true;
        CooldownTime = 10f;
    }

    private IEnumerator SpawnSpikes()
    {
        Instantiate(SpikeRight, SpawnPoint.position, SpawnPoint.rotation);
        AudioManager.PlayOneShot(SpikeSound);
        yield return new WaitForSeconds(SpawnRate);
        Instantiate(SpikeLeft, SpawnPoint.position, SpawnPoint.rotation);
        yield return new WaitForSeconds(SpawnRate);
        if (CanSpawn == true)
        {
            StartCoroutine(SpawnSpikes());
        }
    }

    private IEnumerator BurstAttack()
    {
        Vector3 BPos = new Vector3(Player.position.x,-2.53f,0f);
        BurstPoint.transform.position = BPos;
        Instantiate(BurstObject, BurstPoint.position, SpellPoint.rotation);
        yield return new WaitForSeconds(0.8f);
        BPos = new Vector3(Player.position.x, -2.53f, 0f);
        BurstPoint.transform.position = BPos;
        Instantiate(BurstObject, BurstPoint.position, SpellPoint.rotation);
        yield return new WaitForSeconds(0.8f);
        BPos = new Vector3(Player.position.x, -2.53f, 0f);
        BurstPoint.transform.position = BPos;
        Instantiate(BurstObject, BurstPoint.position, SpellPoint.rotation);
        yield return new WaitForSeconds(0.8f);
    }

    private IEnumerator StartInitiate()
    {
        yield return new WaitForSeconds(0.83f);
        RingAudio.Play();
        Ring1Anim.SetTrigger("Dissolve");
        Ring2Anim.SetTrigger("Dissolve");
        Ring3Anim.SetTrigger("Dissolve");
        Ring4Anim.SetTrigger("Dissolve");
        yield return new WaitForSeconds(0.5f);
        EnemyRb.gravityScale = 3;
        yield return new WaitForSeconds(2.66f);
        EnemyRb.gravityScale = 5;
        StartFight = true;
    }

    void Death()
    {
        IsDead = true;
        DeathParticles.Play();
        gameObject.layer = DefaultLayer;
    }
}
