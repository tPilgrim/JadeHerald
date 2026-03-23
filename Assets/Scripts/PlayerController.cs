using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator Anim;
    public Animator SpellAnim;
    public Animator AttackAnim;
    private int ControllerConnected;
    public GameObject Buttons;

    public Material SpriteMaterial;
    public Material LightMaterial;

    private Rigidbody2D PlayerRb;
    private bool CanMove = true;
    public Transform HeadPosition;
    public float HeadSize;
    public float MaxVelocity;
    private bool IsTurning;

    private bool FacingRight = true;

    private bool RunDirection;
    public float NormalSpeed;
    private float Speed;
    private float MoveInput;
    private float MoveBy;
    public GameObject RunCollider;
    private float SlowTime;

    private bool IsGrounded;
    public float JumpForce;
    public GameObject GroundCheck;
    public LayerMask WhatIsTerrain;
    public float FallMultiplier;
    public float LowJumpMultiplier;
    private bool JumpRequest;
    public float JumpTime;
    private float JumpTimeCounter;
    private bool JumpStop;
    private bool JumpAgain;

    private bool IsFalling;
    private float FallTimeCounter;
    public float FallTime;

    private bool IsLanding;
    private int FirstCollision;
    public ParticleSystem LandParticles;
    public Transform LandParticlesPosition;

    private bool IsAttacking;
    private bool IsJumpAttacking;
    public GameObject AttackHitbox;
    public GameObject UpAttackHitbox;
    public float AttackTime;
    private int AttackNumber = 0;
    private bool NextAttack = true;
    private bool CanChargeAttack;
    private bool FirstChargeAttack;

    public int MaxHealth;
    private int CurrentHealth;
    private bool WasDamaged;
    private int PhysicalDamage;
    private int MagicDamage;
    private int PoiseDamage;
    public ParticleSystem DamageParticles;
    public SpriteRenderer PlayerRenderer;
    public Slider HealthSlider;

    public int Heal;
    public Image[] RegenerationFill;
    public Image RegenerationBubble;
    private int RegenerationLevel;
    private bool IsRegenerating;
    public ParticleSystem RegenBurstParticles;
    public Transform RegenParticlesPosition;

    public GameObject Sheild;
    private bool IsDefending;
    private bool ShieldStance;
    public float MaxStamina;
    private float Stamina;
    public Slider StaminaSlider;
    public int MaxMana;
    private int Mana;
    public Slider ManaSlider;
    private bool CanParry;
    private bool IsStaggered;

    private bool CanDash = true;
    private bool CanDashAgain = true;
    private bool IsDashing;
    private float DashSpeed;
    public float DashSlow;
    public float DashFast;
    public float DashTime;
    public float DashSlowTime;
    public float DashCooldown;
    public ParticleSystem DashParticles;
    private Sprite DashSprite;
    private SpriteRenderer FadeRenderer;

    private bool IsSitting;

    private bool StartJump;
    private bool StartAttack;
    private bool StartDash;
    private bool StartRegen;
    private bool StartDefend;

    private AudioSource AudioManager;
    public AudioSource AudioManager2;
    public AudioClip SwordSlash;
    public AudioClip ChargedSlash;
    public AudioClip ShieldHit;
    public AudioClip PlayerHit;
    public AudioClip LandSound1;
    public AudioClip LandSound2;
    public AudioClip LandSound3;
    public AudioClip JumpSound;
    public AudioClip DashSound;
    public AudioClip RegenSound;
    public AudioClip DeathSound;
    public AudioSource Footsteps;

    public GameObject Menu;
    public GameObject DashFade;

    private AudioSource[] aSources; // Store audio sources
    private bool isSlowMotion = false;

    /*
    public Transform AttackPosition;
    public float AttackRange;
    public LayerMask WhatIsEnemy;
    */

    void Start()
    {
        PlayerPrefs.SetInt("Crimson 0", 1);

        if (PlayerPrefs.GetInt("Respawn") == 1)
        {
            Vector2 Pos = new Vector2(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"));
            transform.position = Pos;
        }

        CurrentHealth = MaxHealth;

        Speed = NormalSpeed;

        PlayerRb = GetComponent<Rigidbody2D>();

        Anim = GetComponent<Animator>();

        FirstCollision = 2;

        Mana = MaxMana;

        ControllerConnected = PlayerPrefs.GetInt("ControllerConnected");

        AudioManager = GetComponent<AudioSource>();

        Load();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Toggle slow motion on key press
        {
            isSlowMotion = !isSlowMotion; // Toggle state

            if (isSlowMotion)
            {
                Time.timeScale = 0.05f; // Slow down time

            }
            else
            {
                Time.timeScale = 1f; // Reset time
            }
        }

        if((IsAttacking && !IsJumpAttacking) || IsSitting || IsDefending || IsRegenerating || IsStaggered || ShieldStance || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Parry") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Stand") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sit Idle"))
        {
            CanMove = false;
        }
        else
        {
            CanMove = true;
        }

        Jump();
        Fall();
        Land();
        Attack();
        Health();
        Regeneration();
        Defend();
        Dash();
        Sit();

        if (Input.GetButton("Reset"))
        {
            Menu.SetActive(true);
            Time.timeScale = 0f;
        }

        if (Input.touchCount > 0)
        {
            ControllerConnected = 0;
        }

        if (Input.GetButton("Attack") || Input.GetButton("Defend") || Input.GetButton("Jump") || Input.GetButton("Dash") || Input.GetButton("Regeneration") || Input.GetButton("Reset"))
        {
            ControllerConnected = 1;
        }

        if (ControllerConnected == 1)
        {
            Buttons.SetActive(false);
        }
        else
        {
            Buttons.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Run();

        if (IsDashing == true)
        {
            PlayerRb.linearVelocity = new Vector2(transform.localScale.x * DashSpeed, PlayerRb.linearVelocity.y);
        }

        if ((FacingRight == false && RunDirection == true && ControllerConnected == 0) || (FacingRight == false && MoveInput > 0))
        //if (FacingRight == false && MoveInput > 0)
        {
            if (CanMove == true && IsDashing == false || (IsDefending == true && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Parry") && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Staggered")))
            {
                Turn();
            }
        }
        //else if (FacingRight == true && MoveInput < 0)
        else if ((FacingRight == true && RunDirection == false && ControllerConnected == 0) || (FacingRight == true && MoveInput < 0))
        {
            if (CanMove == true && IsDashing == false || (IsDefending == true && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Parry") && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Staggered")))
            {
                Turn();
            }
        }

        if (JumpRequest == true)
        {
            Stamina -= 2f;
            AudioManager.pitch = 1f;
            AudioManager.volume = 1f;
            AudioManager.PlayOneShot(JumpSound);
            JumpStop = true;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            JumpRequest = false;
        }

        if (PlayerRb.linearVelocity.y > 0)
        {
            JumpTimeCounter -= Time.deltaTime;
        }

        if (IsGrounded == true)
        {
            JumpTimeCounter = JumpTime;
        }

        if (JumpTimeCounter < 0)
        {
            PlayerRb.linearVelocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
        }

        if (PlayerRb.linearVelocity.y < 0)
        {
            PlayerRb.linearVelocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
        }
        //else if (PlayerRb.velocity.y > 0 && !Input.GetButton("Jump"))
        else if (PlayerRb.linearVelocity.y > 0 && ((StartJump == false && ControllerConnected == 0) || (!Input.GetButton("Jump") && ControllerConnected == 1)))
        {
            PlayerRb.linearVelocity += Vector2.up * Physics2D.gravity.y * (LowJumpMultiplier - 1) * Time.deltaTime;
        }

        PlayerRb.linearVelocity = Vector2.ClampMagnitude(PlayerRb.linearVelocity, MaxVelocity);
    }

    void Turn()
    {
        StartCoroutine(StopDefending());
        FacingRight = !FacingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void RunLeft(bool ShouldMove)
    {
        RunDirection = false;
        if (ShouldMove == true && Input.touchCount > 0)
        {
            MoveInput = -1f;
        }
        else
        {
            MoveInput = 0f;
        }
    }

    public void RunRight(bool ShouldMove)
    {
        RunDirection = true;
        if (ShouldMove == true && Input.touchCount > 0)
        {
            MoveInput = 1f;
        }
        else
        {
            MoveInput = 0f;
        }
    }

    public void Respawn()
    {
        Menu.SetActive(true);
        Time.timeScale = 0f;
        /*
        PlayerPrefs.SetInt("Gate", 0);
        PlayerPrefs.SetInt("CurrentHealth", MaxHealth);
        PlayerPrefs.SetInt("RegenerationLevel", 0);
        PlayerPrefs.SetInt("Mana", 2);
        PlayerPrefs.SetFloat("Stamina", MaxStamina);
        PlayerPrefs.SetInt("Crimson 1", 0);
        PlayerPrefs.SetInt("Crimson 2", 0);
        PlayerPrefs.SetInt("ControllerConnected", ControllerConnected);
        SceneManager.LoadScene(0);
        */
    }

    public void AttackButton(bool ShouldAttack)
    {
        if (Input.touchCount > 0)
        {
            StartAttack = ShouldAttack;
        }
    }

    public void JumpButton(bool ShouldJump)
    {
        if (Input.touchCount > 0)
        {
            StartJump = ShouldJump;
        }
    }

    public void DashButton(bool ShouldDash)
    {
        if (Input.touchCount > 0)
        {
            StartDash = ShouldDash;
        }
    }

    public void RegenerationButton(bool ShouldRegenerate)
    {
        StartRegen = ShouldRegenerate;
    }

    public void DefendButton(bool ShouldDefend)
    {
        if (Input.touchCount > 0)
        {
            StartDefend = ShouldDefend;
        }
    }

    void Run()
    {
        if (CanMove == false)
        {
            Speed = 0f;
        }
        else
        {
            if (SlowTime >= 0)
            {
                Speed = 0f;
                SlowTime -= Time.deltaTime;
            }
            else
            {
                Speed = NormalSpeed;
            }
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            MoveInput = 1f;
            ControllerConnected = 1;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            MoveInput = -1f;
            ControllerConnected = 1;
        }
        else if (Input.GetAxis("Horizontal") == 0 && ControllerConnected == 1)
        {
            MoveInput = 0f;
        }

        MoveBy = MoveInput * Speed;
        PlayerRb.linearVelocity = new Vector2(MoveBy, PlayerRb.linearVelocity.y);
        if (MoveInput != 0 || Input.GetAxis("Horizontal") != 0)
        {
            Anim.SetBool("IsRunning", true);
            if (IsGrounded == true)
            {
                RunCollider.SetActive(true);
            }
            else
            {
                RunCollider.SetActive(false);
            }
        }
        else
        {
            Anim.SetBool("IsRunning", false);
            RunCollider.SetActive(false);
            PlayerRb.linearVelocity = new Vector2(0f, PlayerRb.linearVelocity.y);
        }

        if(this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Run") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Land"))
        {
            Footsteps.enabled = true;
        }
        else
        {
            Footsteps.enabled = false;
        }
    }

    void Jump()
    {
        if ((Input.GetButton("Jump") || StartJump == true) && IsGrounded == true && CanMove == true && JumpStop == false && IsDashing == false)
        {
            JumpRequest = true;
            JumpAgain = false;
        }

        if ((Input.GetButton("Jump") || StartJump == true) && JumpAgain == true)
        {
            JumpAgain = false;
        }

        if ((Input.GetButtonUp("Jump") || (StartJump == false && ControllerConnected == 0)))
        {
            JumpAgain = true;
        }

        if (IsFalling == true || IsGrounded == true)
        {
            Anim.SetBool("IsJumping", false);
        }

        if (PlayerRb.linearVelocity.y > 0.1 && (StartJump == true || Input.GetButton("Jump")))
        {
            Anim.SetBool("IsJumping", true);
        } 

        if (IsGrounded == true)
        {
            if (JumpAgain == true)
            {
                JumpStop = false;
            }
            Anim.SetBool("JumpAttacking", false);
        }
    }

    void Fall()
    {
        if (PlayerRb.linearVelocity.y < 0)
        {
            FallTimeCounter -= Time.deltaTime;
            IsFalling = true;

            if (IsGrounded == false)
            {
                Anim.SetBool("IsFalling", true);
            }
        }
        else
        {
            FallTimeCounter = FallTime;
            IsFalling = false;
            Anim.SetBool("IsFalling", false);
        }

        if (IsGrounded == true)
        {
            Anim.SetBool("IsFalling", false);
        }

        if (FallTimeCounter < 0)
        {
            Anim.SetBool("LongFall", true);
        }
    }

    void Land()
    {
        if (IsLanding == true && IsGrounded == false)
        {
            IsLanding = false;
        }
    }

    void OnTriggerStay2D(Collider2D GroundCheck)
    {
        if (GroundCheck.gameObject.tag == "Ground" || GroundCheck.gameObject.tag == "Object")
        {
            IsGrounded = true;

            if (FirstCollision == 0 && IsFalling == true)
            {
                IsLanding = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D GroundCheck)
    {
        if (GroundCheck.gameObject.tag == "Ground" || GroundCheck.gameObject.tag == "Object")
        {
            IsGrounded = true;

            if (FallTimeCounter < 0)
            {
                Anim.SetTrigger("IsLanding");
                Anim.SetBool("LongFall", false);

                if (PlayerRb.linearVelocity.y < -7.4f)
                {
                    LandParticles.transform.position = new Vector2(transform.position.x, transform.position.y - 0.56f);
                }
                else
                {
                    LandParticles.transform.position = new Vector2(transform.position.x, transform.position.y - 0.45f);
                }
                LandParticles.Play();
            }

            if (FirstCollision == 0 && PlayerRb.linearVelocity.y <= -0.01f)
            {
                AudioManager.pitch = 1f;
                AudioManager.volume = 0.3f;
                int num = Random.Range(1, 4);
                if(num == 1)
                {
                    AudioManager.PlayOneShot(LandSound1);
                }
                else if(num == 2)
                {
                    AudioManager.PlayOneShot(LandSound2);
                }
                else
                {
                    AudioManager.PlayOneShot(LandSound3);
                }
            }

            if(FirstCollision > 0)
            {
                FirstCollision--;
            }
        }

        if (GroundCheck.gameObject.tag == "Spikes")
        {
            StartCoroutine(QuickRespawn());
        }
    }

    void OnTriggerExit2D(Collider2D GroundCheck)
    {
        if (GroundCheck.gameObject.tag == "Ground" || GroundCheck.gameObject.tag == "Object")
        {
            IsGrounded = false;
            DashParticles.Stop();
        }
    }

    void Attack()
    {
        if ((Input.GetButton("Attack") || StartAttack == true) && IsAttacking == false && NextAttack && !IsDashing)
        {
            if (AttackNumber == 3)
            {
                AttackNumber = 0;
            }

            if (IsGrounded == true)
            {
                Anim.SetBool("JumpAttacking", false);
                Anim.SetBool("JumpUpAttacking", false);

                AttackNumber++;
                Anim.SetInteger("AttackNumber", AttackNumber);
            }
            else
            {
                IsJumpAttacking = true;
            }

            StartCoroutine(StartAttacking());
        }

        if (Input.GetButtonUp("Attack") || (StartAttack == false && ControllerConnected == 0))
        {
            NextAttack = true;
        }
    }

    private IEnumerator StartAttacking()
    {
        IsAttacking = true;
        AttackAnimation();
        NextAttack = false;
        yield return new WaitForSeconds(0.1111f);
        StartCoroutine(Slash());
        yield return new WaitForSeconds(AttackTime - 0.1111f);
        if(!IsJumpAttacking)
        {
            SlowTime = 0.125f;
        }
        Anim.SetBool("JumpAttacking", false);
        IsJumpAttacking = false;
        Anim.SetBool("IsRecovering", true);
        IsAttacking = false;
        AttackAnimation();
        yield return new WaitForSeconds(0.2f);
        if(IsAttacking == false)
        {
            AttackNumber = 0;
        }
        Anim.SetBool("IsRecovering", false);
    }

    private void AttackAnimation()
    {
        Anim.SetBool("IsUpAttacking", false);
        Anim.SetBool("IsAttacking", false);
        Anim.SetBool("JumpAttacking", false);
        Anim.SetBool("JumpUpAttacking", false);
        Anim.SetBool("ChargedAttack", false);

        if (IsJumpAttacking)
        {
            if (!IsGrounded)
            {
                if (Input.GetAxis("Vertical") > 0f)
                {
                    Anim.SetBool("JumpUpAttacking", IsAttacking);
                }
                else
                {
                    Anim.SetBool("JumpAttacking", IsAttacking);
                }
            }
            Anim.SetBool("IsAttacking", false);
        }
        else
        {
            if (Input.GetAxis("Vertical") > 0f)
            {
                Anim.SetBool("IsUpAttacking", IsAttacking);
            }
            else if (CanChargeAttack && FirstChargeAttack)
            {
                Anim.SetBool("ChargedAttack", IsAttacking);
                FirstChargeAttack = false;
                AttackNumber = 0;
            }
            else
            {
                Anim.SetBool("IsAttacking", IsAttacking);
            }
        }
    }

    private IEnumerator Slash()
    {
        AudioManager.volume = 0.3f;
        if (AttackNumber == 1)
        {
            AudioManager.pitch = 0.85f;
        }
        else if (AttackNumber == 2)
        {
            AudioManager.pitch = 0.8f;
        }
        else if (AttackNumber == 3 || IsGrounded == false)
        {
            AudioManager.pitch = 0.95f;
        }
        AudioManager.PlayOneShot(SwordSlash);

        if (CanChargeAttack == true)
        {
            AudioManager.pitch = 0.7f;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            UpAttackHitbox.SetActive(true);
        }
        else
        {
            AttackHitbox.SetActive(true);
        }
        yield return new WaitForSeconds(0.075f);
        AttackHitbox.SetActive(false);
        UpAttackHitbox.SetActive(false);
    }

    /*
    void Attack()
    {
        if ((Input.GetButton("Attack") || StartAttack == true) && AttackTimeCounter <= 0 && IsDashing == false)
        //if (Input.GetButton("Attack") && AttackTimeCounter <= 0 && IsDashing == false)
        {
            CanAttack = true;

            if (AttackNumber == 3 || AttackTimeCounter <= -0.25)
            {
                AttackNumber = 0;
            }

            if (NextAttack == true)
            {
                if (IsGrounded == true)
                {
                    if (CanChargeAttack == true && FirstChargeAttack == true)
                    {
                        Anim.SetBool("ChargedAttack", true);
                    }
                    else
                    {
                        Anim.SetBool("IsAttacking", true);
                        AttackNumber++;
                        Anim.SetInteger("AttackNumber", AttackNumber);
                    }
                }
                else
                {
                    Anim.SetBool("JumpAttacking", true);
                }

                AttackTimeCounter = AttackTime;
                NextAttack = false;
            }
        }

        if (IsGrounded == false)
        {
            Anim.SetBool("IsAttacking", false);
            Anim.SetBool("JumpAttacking", true);
        }

        if ((AttackTimeCounter >= -0.1 && IsJumping == false && IsFalling == false && IsGrounded == true) || IsRegenerating == true || IsDefending == true || ShieldStance == true || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Parry") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Staggered"))
        {
            CanMove = false;
        }
        else
        {
            CanMove = true;
        }
        if (Input.GetButtonUp("Attack") || (StartAttack == false && ControllerConnected == 0))
        //if (Input.GetButtonUp("Attack"))
        {
            NextAttack = true;
        }

        if (CanAttack == true && NextAttack == true)
        {
            Collider2D[] EnemiesToDamage = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRange, WhatIsEnemy);

            if (AttackTimeCounter >= StartAttackTime && AttackTimeCounter <= StartAttackTime + 0.05f && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Staggered") && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                if (SpawnAttack == true)
                {
                    GameObject AttackScale = Instantiate(AttackHitbox, transform.position, transform.rotation);
                    AttackScale.transform.localScale = transform.localScale;
                    SpawnAttack = false;
                }

                for (int i = 0; i < EnemiesToDamage.Length; i++)
                {
                    if (RegenerationLevel < 28)
                    {
                        if (CanChargeAttack == true && RegenerationLevel < 27)
                        {
                            RegenerationLevel += 4;
                        }
                        else
                        {
                            RegenerationLevel += 2;
                        }
                    }

                    if (CanChargeAttack == true)
                    {
                        EnemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(Damage + 1);
                        CanChargeAttack = false;
                    }
                    else
                    {
                        EnemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(Damage);
                    }

                    if (FacingRight == true)
                    {
                        AttackParticlesPosition.transform.position = new Vector2(transform.position.x + 0.7f, transform.position.y);
                    }
                    else
                    {
                        AttackParticlesPosition.transform.position = new Vector2(transform.position.x - 0.7f, transform.position.y);
                    }

                    AttackBurstParticles.Play();

                    CanAttack = false;
                }
            }
        }

        if (AttackTimeCounter > -1)
        {
            AttackTimeCounter -= Time.deltaTime;
        }

        if (AttackTimeCounter <= 0)
        {
            SpawnAttack = true;
            Anim.SetBool("IsAttacking", false);
            Anim.SetBool("JumpAttacking", false);
            Anim.SetBool("ChargedAttack", false);

            if (CanMove == false && IsJumping == false && IsFalling == false && IsGrounded == true)
            {
                AttackSlow = true;
            }
        }
    }
    */

    void JumpSlash()
    {
        AttackAnim.SetTrigger("SideSlash");
    }

    void JumpUpSlash()
    {
        AttackAnim.SetTrigger("UpSlash");
    }

    IEnumerator ChargedAttack()
    {
        CanChargeAttack = true;
        yield return new WaitForSeconds(0.8f);
        CanChargeAttack = false;
        FirstChargeAttack = false;
    }
    /*
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPosition.position, AttackRange);
        Gizmos.DrawWireSphere(HeadPosition.position, HeadSize);
    }
    */

    void Direction()
    {
        if ((FacingRight == false && RunDirection == true && ControllerConnected == 0) || (FacingRight == false && MoveInput > 0))
        {
            Turn();
        }
        else if ((FacingRight == true && RunDirection == false && ControllerConnected == 0) || (FacingRight == true && MoveInput < 0))
        {
            Turn();
        }
        /*
        if (FacingRight == false && MoveInput > 0)
        {
            Turn();
        }
        else if (FacingRight == true && MoveInput < 0)
        {
            Turn();
        }
        */
    }

    public void AccesHealth(int PhyDamage, int MagDamage, int PoiDamage, bool HitShield)
    {
        DamageParticles.Play();
        WasDamaged = true;
        PhysicalDamage = PhyDamage;
        MagicDamage = MagDamage;
        PoiseDamage = PoiDamage;
        CanParry = HitShield;
    }

    void Health()
    {
        if (HealthSlider.value < CurrentHealth)
        {
            HealthSlider.value += Time.deltaTime * 30f;
        }

        if (HealthSlider.value > CurrentHealth)
        {
            HealthSlider.value -= Time.deltaTime * 30f;
        }

        if (CurrentHealth > HealthSlider.value - 0.15f && CurrentHealth < HealthSlider.value + 0.15f)
        {
            HealthSlider.value = CurrentHealth;
        }
        
        if (WasDamaged == true)
        {
            if (CanParry == false || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Staggered"))
            {
                StartCoroutine(Flash());
                AudioManager2.pitch = 1f;
                AudioManager2.volume = 0.4f;
                AudioManager2.PlayOneShot(PlayerHit);
                CurrentHealth -= PhysicalDamage + MagicDamage;
            }
            else
            {
                FirstChargeAttack = true;
                StartCoroutine(ChargedAttack());
                CurrentHealth -= MagicDamage/2;

                if (Stamina > PhysicalDamage + MagicDamage + PoiseDamage)
                {
                    Stamina -= PhysicalDamage + MagicDamage + PoiseDamage;
                }
                else
                {
                    Stamina = 0;
                }

                AudioManager.pitch = 1f;
                AudioManager.volume = 0.4f;
                AudioManager.PlayOneShot(ShieldHit);
                //Anim.SetTrigger("Parry");
                if ((Input.GetButton("Defend") || StartDefend == true)&& !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Parry"))
                {
                    Anim.SetTrigger("Parry");
                }
            }

            CameraShake.Instance.ShakeCamera(1f, 0.2f);
            WasDamaged = false;    
        }

        if (CurrentHealth <= 0)
        {
            if(!this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                StartCoroutine(Death());
            }
            CanMove = false;

            if (IsGrounded == false)
            {
                PlayerRb.linearVelocity = new Vector2(0f, -6f);
            }
        }
    }

    IEnumerator Death()
    {
        if (FacingRight == true)
        {
            RegenParticlesPosition.transform.position = new Vector2(transform.position.x + 0.15f, transform.position.y + 0.1f);
        }
        else
        {
            RegenParticlesPosition.transform.position = new Vector2(transform.position.x - 0.15f, transform.position.y + 0.1f);
        }

        RegenBurstParticles.Play();
        Anim.SetBool("IsDying", true);
        AudioManager.pitch = 1f;
        AudioManager.volume = 0.4f;
        AudioManager.PlayOneShot(DeathSound);
        gameObject.tag = "Respawn";
        yield return new WaitForSeconds(3f);
        PlayerPrefs.SetInt("Respawn", 1);
        SceneManager.LoadScene(PlayerPrefs.GetInt("SceneReload"));
    }

    IEnumerator Flash()
    {
        GetComponent<Renderer>().material = SpriteMaterial;
        yield return new WaitForSeconds(0.15f);
        GetComponent<Renderer>().material = LightMaterial;
    }

    void Regeneration()
    {
        if ((Input.GetButton("Regeneration") || StartRegen == true) && Mana > 0 && IsRegenerating == false && IsGrounded == true && CanMove == true)
        //if (Input.GetButton("Regeneration") && Mana > 0 && IsRegenerating == false && IsGrounded == true && AttackTimeCounter <= 0 && CanMove == true)
        {
            StartCoroutine(StartRegeneration());
        }

        if (ManaSlider.value < Mana)
        {
            ManaSlider.value += Time.deltaTime * 5f;
        }

        if (ManaSlider.value > Mana)
        {
            ManaSlider.value -= Time.deltaTime * 5f;
        }

        if (Mana > ManaSlider.value - 0.15f && Mana < ManaSlider.value + 0.15f)
        {
            ManaSlider.value = Mana;
        }

        if (RegenerationLevel >= 28 && Mana < MaxMana)
        {
            Mana++;
            RegenerationLevel = 0;
        }

        RegenerationBubble = RegenerationFill[RegenerationLevel / 2];

        for (int i = 0; i < RegenerationFill.Length; i++)
        {
            if (RegenerationFill[i] != RegenerationBubble)
            {
                RegenerationFill[i].enabled = false;
            }
            else
            {
                RegenerationFill[i].enabled = true;
            }
        }
    }

    IEnumerator StartRegeneration()
    {
        AudioManager.pitch = 1f;
        AudioManager.volume = 1f;
        AudioManager.PlayOneShot(RegenSound);
        IsRegenerating = true;
        Mana--;
        Anim.SetBool("IsRegenerating", true);
        yield return new WaitForSeconds(0.45f);
        SpellAnim.SetInteger("Spell", 1);
        yield return new WaitForSeconds(0.55f);
        SpellAnim.SetInteger("Spell", 0);
        if (CurrentHealth <= MaxHealth - 12)
        {
            CurrentHealth += Heal;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
        yield return new WaitForSeconds(0.46f);
        Anim.SetBool("IsRegenerating", false);

        IsRegenerating = false;
    }

    public void Regenerate()
    {
        if (RegenerationLevel < 28)
        {
            RegenerationLevel += 2;
        }
    }

    void Defend()
    {
        if (Stamina < MaxStamina && IsDefending == false)
        {
            Stamina += 2 * Time.deltaTime;
        }

        if(Stamina > MaxStamina)
        {
            Stamina = MaxStamina;
        }

        if (StaminaSlider.value < Stamina)
        {
            StaminaSlider.value += Time.deltaTime * 30f;
        }

        if (StaminaSlider.value > Stamina)
        {
            StaminaSlider.value -= Time.deltaTime * 30f;
        }

        if (Stamina > StaminaSlider.value - 0.15f && Stamina < StaminaSlider.value + 0.15f)
        {
            StaminaSlider.value = Stamina;
        }
        if ((Input.GetButton("Defend") || StartDefend == true) && IsGrounded == true && IsRegenerating == false && !IsAttacking && IsTurning == false && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("IsDefending") && IsDashing == false)
        //if (Input.GetButton("Defend") && IsGrounded == true && IsRegenerating == false && AttackTimeCounter <= 0 && IsTurning == false && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("IsDefending") && IsDashing == false)
        {
            ShieldStance = true;
        }
        else
        {
            ShieldStance = false;
        }
        if ((Input.GetButtonUp("Defend") || (StartDefend == false && ControllerConnected == 0)) || IsAttacking || IsDashing == true)
        //if (Input.GetButtonUp("Defend") || AttackTimeCounter > 0 || IsDashing == true)
        {
            Sheild.SetActive(false);
            IsDefending = false;
        }

        Anim.SetBool("Start Defending", ShieldStance);
        Anim.SetBool("IsDefending", IsDefending);

        if (Stamina <= 0)
        {
            Anim.SetBool("Staggered", true);
        }
        else
        {
            Anim.SetBool("Staggered", false);
        }
    }

    void StartDefending()
    {
        Sheild.SetActive(true);
        IsDefending = true;
    }

    IEnumerator StopDefending()
    {
        IsTurning = true;
        IsDefending = false;
        yield return new WaitForSeconds(0.1f);
        IsTurning = false;
    }

    private IEnumerator StartDashing()
    {
        //InvokeRepeating("DashTrail", 0f, 0.04f);
        AudioManager.pitch = 1f;
        AudioManager.volume = 1f;
        AudioManager.PlayOneShot(DashSound);
        CanDash = false;
        CanDashAgain = false;
        IsDashing = true;
        Anim.SetBool("IsDashing", true);
        Stamina -= 3f;
        DashParticles.Play();
        DashSpeed = DashFast;
        yield return new WaitForSeconds(DashTime);
        CancelInvoke();
        DashSpeed = DashSlow;
        yield return new WaitForSeconds(DashSlowTime);
        IsDashing = false;
        Anim.SetBool("IsDashing", false);
        yield return new WaitForSeconds(DashCooldown);
        CanDash = true;
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(0.1f);
        StopCoroutine(StartDashing());
        IsDashing = false;
        Anim.SetBool("IsDashing", false);
    }

    void Dash()
    {
        if(IsDashing)
        {
            DashSprite = GetComponent<SpriteRenderer>().sprite;
            FadeRenderer = DashFade.GetComponent<SpriteRenderer>();
            FadeRenderer.sprite = DashSprite;
        }

        Physics2D.IgnoreLayerCollision(12, 10, IsDashing);
        Physics2D.IgnoreLayerCollision(7, 10, IsDashing);

        if ((Input.GetButtonDown("Dash") || Input.GetButton("Dash") || StartDash == true) && CanDash == true && IsGrounded == true && Stamina >= 3f && (CanMove || ShieldStance) && CanDashAgain == true)
        //if ((Input.GetButtonDown("Dash") || Input.GetButton("Dash")) && CanDash == true && IsGrounded == true && AttackTimeCounter <= 0 && Stamina >= 3f && CanMove == true && CanDashAgain == true)
        {
            StartCoroutine(StartDashing());
        }
        if (Input.GetButtonUp("Dash") || (StartDash == false && ControllerConnected == 0))
        //if (Input.GetButtonUp("Dash"))
        {
            CanDashAgain = true;
        }

        if(IsDashing == true && IsGrounded == false)
        {
            //StartCoroutine(StopDashing());
        }

        if(this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            PlayerRb.linearVelocity = new Vector2(0f, PlayerRb.linearVelocity.y);
        }
    }

    void DashTrail()
    {
        GameObject Fade = Instantiate(DashFade, transform.position, transform.rotation);
        Fade.transform.localScale = transform.localScale;
    }

    public void StartSit()
    {
        IsSitting = true;
    }

    void Sit()
    {
        Anim.SetBool("IsSitting", IsSitting);

        if (Input.GetAxis("Horizontal") != 0 && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sit"))
        {
            IsSitting = false;
        }
    }

    public void CheckPoint(float PositionX, float PositionY)
    {
        PlayerPrefs.SetInt("SceneReload", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetFloat("PosX", PositionX);
        PlayerPrefs.SetFloat("PosY", PositionY);
        CurrentHealth = MaxHealth;
        Mana = MaxMana;
        Stamina = MaxStamina;
        RegenerationLevel = 0;
        for (int i = 0; i <= 6; i++)
        {
            PlayerPrefs.SetInt("Scene" + i, 1);
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("CurrentHealth", CurrentHealth);
        PlayerPrefs.SetInt("RegenerationLevel", RegenerationLevel);
        PlayerPrefs.SetInt("Mana", Mana);
        PlayerPrefs.SetFloat("Stamina", Stamina);
        PlayerPrefs.SetInt("ControllerConnected", ControllerConnected);
    }

    void Load()
    {
        CurrentHealth = PlayerPrefs.GetInt("CurrentHealth");
        RegenerationLevel = PlayerPrefs.GetInt("RegenerationLevel");
        Mana = PlayerPrefs.GetInt("Mana");
        Stamina = PlayerPrefs.GetFloat("Stamina");
    }

    IEnumerator QuickRespawn()
    {
        StartCoroutine(Flash());
        PlayerPrefs.SetInt("CurrentHealth", CurrentHealth - 6);
        Save();
        yield return new WaitForSeconds(0.1f);
        CurrentHealth -= 999;
        yield return new WaitForSeconds(2f);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}