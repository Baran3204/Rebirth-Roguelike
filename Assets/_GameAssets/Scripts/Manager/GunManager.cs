using System;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _gunTransform;
    [SerializeField] private GameObject _bullet;

    [Header("Settings")]

    [SerializeField] private float _bulletSpeed;

    private float _Direction;


    private void Update() 
    {
        SetInputs();
        CreateBullet();   
    }

    private void CreateBullet()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(_bullet, _gunTransform.position, Quaternion.identity);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            rb.linearVelocity = _gunTransform.transform.right * _bulletSpeed;

            Destroy(bullet, 2f);
        }
    }
    private void SetInputs()
    {
        var mouseDir = Input.mousePosition - Camera.main.WorldToScreenPoint(_gunTransform.position);
         
        _Direction = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

        _gunTransform.rotation = Quaternion.AngleAxis(_Direction, Vector3.forward);
    }
}
