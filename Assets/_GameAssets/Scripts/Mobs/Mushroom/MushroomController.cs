using UnityEngine;

public class MushroomController : MonoBehaviour, IDamagables
{
    [Header("References")]
    [SerializeField] private Animator _mushroomAnimator;
    private Transform _playerTransform;
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Settings")]
    [SerializeField] private float _mushroomSpeed;
    [SerializeField] private float _damageCoolDown;
    [SerializeField] private float _maxHealMushroom;
    [SerializeField] private float _destroyCooldown;
    [SerializeField] private float _damageAmount;


    private Rigidbody2D _mushroomRB;
    private MushroomState _currentState = MushroomState.Move;
    private Vector3  _movementDirection;
    private float _currentDamageCooldown, _currentMushroomHeal;
    private bool _isDead;
    private void Awake() 
    {
       _mushroomRB = GetComponent<Rigidbody2D>();
       ChangeState(MushroomState.Move);
       _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
       _currentDamageCooldown = _damageCoolDown;
       _currentMushroomHeal = _maxHealMushroom;
    }

    private void Update() 
    {

        Debug.Log("MUSHROOM CURRENT HEAL: " +  _currentMushroomHeal.ToString());
        SetMushroomFlip();
        SetMushroomState();
        SetMushroomAnim();
        SetMushroomSpeed();
        SetMushroomDamage();
    }

    private void FixedUpdate() 
    {
        SetMushroomDirection();  
        StateWorking();  
    }


    private void SetMushroomDirection()
    {

        _movementDirection =  _playerTransform.position - transform.position;

        _mushroomRB.linearVelocity = _movementDirection * _mushroomSpeed * Time.deltaTime;
    } 

    private void SetMushroomFlip()
    {
        if(_playerTransform.position.x > transform.position.x)
        {
            _sprite.flipX = false;
        }
        else 
        {
            _sprite.flipX = true;
        }
    }
    private void SetMushroomSpeed()
    {
        var currentState = GetGoblinState();

        var newSpeed = currentState switch
        {
            _ when currentState == MushroomState.Move => 15f,
            _ when currentState == MushroomState.Attack => 0f,
            _ when currentState == MushroomState.Dead => 0f,
            _ => _mushroomSpeed
        };

        _mushroomSpeed = newSpeed;
    }

    private bool IsAttack()
    {
        if(Vector3.Distance(transform.position, _playerTransform.position)<= 2f)
        {
            return true;
        }
        else return false;     
    }

    private void SetMushroomDamage()
    {
        var isAttack = IsAttack();

        
        if(isAttack)
        {
            _currentDamageCooldown -= Time.deltaTime;

            if(_currentDamageCooldown <= 0f)
            {
                HealManager.Instance.Damage(_damageAmount);
                _currentDamageCooldown += _damageCoolDown;
            }
            
        }
    }
    private void SetMushroomState()
    {
        var movementDirection = _movementDirection.normalized;
        var currentState = GetGoblinState();
        var isAttack = IsAttack();
        var IsDead = GetIsDead();

        var newState = currentState switch
        {
            _ when movementDirection != Vector3.zero && !isAttack && IsDead => MushroomState.Dead,
            _ when movementDirection != Vector3.zero && !isAttack && !IsDead => MushroomState.Move,
            _ when movementDirection != Vector3.zero && isAttack && !IsDead => MushroomState.Attack,
            _ => MushroomState.Move
        };

        if(newState == currentState) { return; }
        else ChangeState(newState);
    }
    public enum MushroomState
    {
        Move, Attack, Dead
    }

    private void ChangeState(MushroomState newState)
    {
        if(_currentState == newState) { return; }

        _currentState = newState;
    }

    public MushroomState GetGoblinState()
    {
        return _currentState;
    }
    private void SetMushroomAnim()
    {
        var currentState = GetGoblinState();

        switch(currentState)
        {
            case MushroomState.Move:
            _mushroomAnimator.SetBool("IsMove", true);
            _mushroomAnimator.SetBool("IsDead", false);
            _mushroomAnimator.SetBool("IsAttack", false);
            break;
            case MushroomState.Dead:
            _mushroomAnimator.SetBool("IsMove", false);
            _mushroomAnimator.SetBool("IsDead", true);
            _mushroomAnimator.SetBool("IsAttack", false);
            break;
            case MushroomState.Attack:
            _mushroomAnimator.SetBool("IsMove", false);
            _mushroomAnimator.SetBool("IsDead", false);
            _mushroomAnimator.SetBool("IsAttack", true);
            break;
        }
    }
    private void StateWorking()
    {
        var currentState = GetGoblinState();

        switch (currentState)
        {
            case MushroomState.Move:
            Debug.Log("MUSHROOM MOVE");
            break;
            case MushroomState.Attack:
            Debug.Log("MUSHROOM ATTACK");
            break;
            case MushroomState.Dead:
            Debug.Log("MUSHROOM DEAD");
            break;
        }
    }

    public void Damage(float damageAmount)
    {
        _currentMushroomHeal -= damageAmount;

        if(_currentMushroomHeal <= 0f)
        {
            _isDead = true;
        }
        if(_isDead)
        {
            Destroy(gameObject, _destroyCooldown);
            HealManager.Instance.Heal(5f);
        }
    }

    private bool GetIsDead()
    {
        return _isDead;
    }
}
