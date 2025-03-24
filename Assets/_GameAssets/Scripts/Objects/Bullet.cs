using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D collision)
    {
         if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.TryGetComponent<IDamagables>(out IDamagables component);
            component.Damage(GunManager.Instance.GetBulletDamage());
            Destroy(gameObject);
        }
    }

    
}
