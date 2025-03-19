using UnityEngine;

public class Medkit : MonoBehaviour, IHealables
{
    [SerializeField] private float _healAmount;

    public void Heal()
    {
        HealManager.Instance.Healİncrease(_healAmount);
    }
}
