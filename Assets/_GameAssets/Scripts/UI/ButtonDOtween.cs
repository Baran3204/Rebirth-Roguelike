using UnityEngine;
using DG.Tweening;
public class ButtonDOtween : MonoBehaviour
{
    private RectTransform _transform;

    private void Awake() 
    {
        _transform = GetComponent<RectTransform>();    
    }
    private void OnMouseEnter()
    {
        _transform.DOScale(0.7f, 0.2f).SetEase(Ease.InBack);
    }

    private void OnMouseExit() 
    {
         _transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);  
    }

    
}
