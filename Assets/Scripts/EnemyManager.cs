using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameManager Manager;
    public SpriteRenderer SpriteHandle;
    public Rigidbody2D rgbd;
    public BoxCollider2D Hitbox;
    public string EnemyType;
    public int EnemySpeed;
    public int MaxHealth;
    public int CurHealth;
    public Sprite Idle;
    public Sprite Walking;
    public Sprite Dead;
    public Sprite Hit;
    public int ScoreUp;
    public AudioSource WasHit;
    public AudioSource Death;
    public AudioSource LostLife;
    public GameObject WasHitExplo;


    private void Start()
    {
        Manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        CurHealth = MaxHealth;
        rgbd.linearVelocity = new Vector2(-1.5f * EnemySpeed, 0);
        StartCoroutine(Walk());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && this.gameObject.CompareTag("Enemy"))
        {
            triggerHit(collision.gameObject);
        }
    }

    public void triggerHit(GameObject hitBy)
    {
        StopAllCoroutines();
        WasHitExplo.SetActive(true);
        WasHit.Play();
        Destroy(hitBy);
        SpriteHandle.sprite = Hit;
        rgbd.linearVelocity = Vector2.zero;
        CurHealth--;
        isDead();
    }

    public bool checkDead()
    {
        if (CurHealth <= 0 && MaxHealth >= 0)
        {
            return true;
        }
        else
        {
            StartCoroutine(HitRoutine());
            return false;
        }
    }

    public void isDead()
    {
        if (checkDead())
        {
            rgbd.linearVelocity = Vector2.zero;
            SpriteHandle.sprite = Dead;
            Hitbox.enabled = false;
            Manager.IncreaseScore(ScoreUp);
            Death.Play();
            Destroy(this.gameObject, 1);
        }
    }

    public IEnumerator HitRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Walk());
        rgbd.linearVelocity = new Vector2(-1.5f * EnemySpeed, 0);
    }

    public IEnumerator Walk()
    {
        while (true)
        {
            WasHitExplo.SetActive(false);
            SpriteHandle.sprite = Idle;
            yield return new WaitForSeconds(0.15f);
            SpriteHandle.sprite = Walking;
            yield return new WaitForSeconds(0.15f);
        }


    }
}
