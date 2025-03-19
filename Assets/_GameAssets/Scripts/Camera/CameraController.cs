using UnityEngine;
public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _targetTransform;
    private Vector3 _cameraTransfrom;
    private void Update() 
    {   
        _cameraTransfrom = transform.position += new Vector3(0f, 0f, 10f);
    }
    private void FixedUpdate() 
    {
         _targetTransform.position = _cameraTransfrom;         
    }
}
