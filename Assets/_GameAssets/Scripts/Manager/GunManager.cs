using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager Instance;

    [Header("References")]
    [SerializeField] private Transform _gunTransform;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _bulletDamage;

    [Header("Settings")]

    [SerializeField] private float _bulletSpeed;

    private float _direction;
    private float _usedBullet;

    private void Awake() 
    {
        Instance = this;   
        _usedBullet = 0f; 
    }
    private void Update() 
    {
        var currentState = GameManager.Instance.GetGameState();

         if(currentState != GameManager.GameState.Pause && currentState != GameManager.GameState.GameOver)
         {
             SetInputs();
             CreateBullet();
         }  
    }

    private void CreateBullet()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(_bullet, _gunTransform.position, Quaternion.identity);
            _usedBullet++;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            rb.linearVelocity = _gunTransform.transform.right * _bulletSpeed;

            Destroy(bullet, 2f);
        }
    }
    private void SetInputs()
    {
        var mouseDir = Input.mousePosition - Camera.main.WorldToScreenPoint(_gunTransform.position);
         
        _direction = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

        _gunTransform.rotation = Quaternion.AngleAxis(_direction, Vector3.forward);
    }

    public float GetBulletDamage()
    {
        return _bulletDamage;
    }

    public void UpdateBulletDamge(float amount)
    {
        _bulletDamage += amount;
    }

    public float GetUsedBullet()
    {   
        return _usedBullet;
    }

    public float GetMaxDamage()
    {
        return _bulletDamage;
    }
}
