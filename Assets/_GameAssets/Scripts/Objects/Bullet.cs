using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet Instance;
    [SerializeField] private float _bulletDamage;
    

    private void Awake() 
    {
        Instance = this;    
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
         if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.TryGetComponent<IDamagables>(out IDamagables component);
            component.Damage(_bulletDamage);
            Destroy(gameObject);
        }
    }

    public void SetBulletDamage(float amount)
    {
        _bulletDamage += amount;
    }

    
}
