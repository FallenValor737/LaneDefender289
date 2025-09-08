using UnityEngine;

public class HealthWall : MonoBehaviour
{
    public GameManager Manager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Manager.TakeDamage();
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<EnemyManager>().Hitbox.enabled = false;
            collision.gameObject.GetComponent<EnemyManager>().LostLife.Play();
            Destroy(collision.gameObject,2f);
        }
    }
}
