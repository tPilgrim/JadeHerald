using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    private Animator Anim;
    private Animator BowAnim;

    private SpriteRenderer SpriteRenderer;
    public Sprite ArcherSprite;
    private Rigidbody2D EnemyRb;
    private int DefaultLayer;

    public Transform BowPosition;
    private Transform Player;
    private Vector3 Scaler;
    private float RotationZ;
    private Vector3 Direction;

    public GameObject Bow;
    public GameObject Arrow;
    public Transform ProjectilePoint;
    private bool IsInRangedCombat;
    private bool IsInCombat;
    private bool IsShooting;

    public float Speed;
    public Transform ReturnPoint;
    private bool IsReturning;
    public GameObject AttackHitbox;
    public int Damage;
    private bool IsAttacking;
    private bool CanUnsheathe;
    private bool NextAttack;

    void Start()
    {
        Anim = GetComponent<Animator>();
        BowAnim = Bow.GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Scaler = transform.localScale;
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        EnemyRb = GetComponent<Rigidbody2D>();
        DefaultLayer = LayerMask.NameToLayer("Dead");
    }

    void Update()
    {
        if(!this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Archer Death"))
        {
            Turn();
        }
        else
        {
            Bow.SetActive(false);
            gameObject.layer = DefaultLayer;
        }

        BowRotation();
        Follow();

        if(IsInCombat == false && IsInRangedCombat == true && IsShooting == false && IsReturning == false && IsAttacking == false)
        {
            IsShooting = true;
            StartCoroutine(Ranged());
        }

        if(IsAttacking == true)
        {
            EnemyRb.velocity = new Vector2(0f, 0f);
        }
    }

    void Turn()
    {
        if(IsReturning == false)
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
        else if(IsAttacking == false)
        {
            if (ReturnPoint.transform.position.x < transform.position.x && Scaler.x < 0)
            {
                Scaler.x *= -1;
                transform.localScale = Scaler;
            }

            if (ReturnPoint.transform.position.x > transform.position.x && Scaler.x > 0)
            {
                Scaler.x *= -1;
                transform.localScale = Scaler;

            }
        }

    }

    public void RangedCombat(bool EnterRangedCombat)
    {
        IsInRangedCombat = EnterRangedCombat;
    }

    IEnumerator Ranged()
    {
        Bow.SetActive(false);
        Anim.SetBool("IsDrawing", true);
        yield return new WaitForSeconds(1.085f);
        Bow.SetActive(true);
        Anim.SetBool("IsDrawing", false);

        yield return new WaitForSeconds(1f);

        if (Scaler.x > 0 && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Archer Death"))
        {
            ProjectilePoint.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ + 180);
            Instantiate(Arrow, ProjectilePoint.position, ProjectilePoint.rotation);
        }

        if (Scaler.x < 0 && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Archer Death"))
        {
            ProjectilePoint.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ + 180);
            Instantiate(Arrow, ProjectilePoint.position, ProjectilePoint.rotation);
        }

        IsShooting = false;
    }

    void BowRotation()
    {
        Direction = Player.transform.position - BowPosition.transform.position;
        Direction.Normalize();
        RotationZ = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;

        if (Scaler.x > 0)
        {
            BowPosition.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ + 180);
        }

        if (Scaler.x < 0)
        {
            BowPosition.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ);
        }
    }

    public void Combat(bool EnterCombat)
    {
        IsInCombat = EnterCombat;

        if(IsInCombat == true)
        {
            IsReturning = false;
            CanUnsheathe = true;
            Anim.SetBool("Idle", true);
        }
        else
        {
            IsReturning = true;
            Anim.SetBool("Idle", false);
        }
    }

    void Follow()
    {
        if(IsInCombat == true && IsShooting == false && CanUnsheathe == true)
        {
            Bow.SetActive(false);
            StartCoroutine(Unsheathe());
        }

        if(IsAttacking == false && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Archer Death"))
        {
            if (IsInCombat == true)
            {
                if (this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Archer Run"))
                {
                    if (Player.position.x + 0.1 > transform.position.x && Player.position.x - 0.1 < transform.position.x)
                    {
                        EnemyRb.velocity = new Vector2(0f, 0f);
                        Anim.SetBool("IsRunning", false);
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
            }
            else
            {
                IsReturning = true;

                if (ReturnPoint.position.x + 0.1 > transform.position.x && ReturnPoint.position.x - 0.1 < transform.position.x)
                {
                    if(IsInRangedCombat == false && IsShooting == false)
                    {
                        Bow.SetActive(true);
                    }
                    IsReturning = false;
                    EnemyRb.velocity = new Vector2(0f, 0f);
                    Anim.SetBool("IsRunning", false);
                }
                else if (ReturnPoint.transform.position.x < transform.position.x)
                {
                    EnemyRb.velocity = new Vector2(-Speed, 0f);
                }
                else if (ReturnPoint.transform.position.x > transform.position.x)
                {
                    EnemyRb.velocity = new Vector2(Speed, 0f);
                }
            }
        }
    }

    IEnumerator Unsheathe()
    {
        Anim.SetBool("Unsheathe", true);
        yield return new WaitForSeconds(0.5f);
        Anim.SetBool("Unsheathe", false);
        CanUnsheathe = false;
        Anim.SetBool("IsRunning", true);
    }

    public void StartAttack(bool CanAttack)
    {
        NextAttack = CanAttack;

        if (IsAttacking == false && CanAttack == true)
        {
            StartCoroutine(Attack());
        }
    }

    public IEnumerator Attack()
    {
        IsAttacking = true;
        Anim.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(2f);
        Anim.SetBool("IsAttacking", false);
        if(NextAttack == true)
        {
            StartCoroutine(Attack());
        }
        IsAttacking = false;
    }

    IEnumerator Slash()
    {
        AttackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        AttackHitbox.SetActive(false);
    }
}

