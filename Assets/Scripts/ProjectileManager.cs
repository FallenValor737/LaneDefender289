using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public int speed;
    public Rigidbody2D rgbd;
    void Start()
    {
        rgbd.linearVelocityX = speed;
        Destroy(this.gameObject, 5);
    }

}
