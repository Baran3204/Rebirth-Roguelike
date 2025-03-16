using UnityEngine;

public class GoblinController : MonoBehaviour, IDamagables
{
    [Header("References")]
    [SerializeField] private Animator _goblinAnimator;
    private Transform _playerTransform;
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Settings")]
    [SerializeField] private float _goblinSpeed;
    [SerializeField] private float _damageCoolDown;
    [SerializeField] private float _maxHealGoblin;
    [SerializeField] private float _destroyCooldown;
    [SerializeField] private float _damageAmount;


    private Rigidbody2D _goblinRB;
    private GoblinState _currentState = GoblinState.Move;
    private Vector3  _movementDirection;
    private float _currentDamageCooldown, _currentGoblinHeal;
    private bool _isDead;
    private void Awake() 
    {
       _goblinRB = GetComponent<Rigidbody2D>();
       ChangeState(GoblinState.Move);
       _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
       _currentDamageCooldown = _damageCoolDown;
       _currentGoblinHeal = _maxHealGoblin;
    }

    private void Update() 
    {

        Debug.Log("SKELETON CURRENT HEAL: " +  _currentGoblinHeal.ToString());
        SetGoblinFlip();
        SetGoblinState();
        SetGoblinAnim();
        SetGoblinSpeed();
        SetGoblinDamage();
    }

    private void FixedUpdate() 
    {
        SetGoblinDirection();  
        StateWorking();  
    }


    private void SetGoblinDirection()
    {

        _movementDirection =  _playerTransform.position - transform.position;

        _goblinRB.linearVelocity = _movementDirection * _goblinSpeed * Time.deltaTime;
    } 

    private void SetGoblinFlip()
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
    private void SetGoblinSpeed()
    {
        var currentState = GetGoblinState();

        var newSpeed = currentState switch
        {
            _ when currentState == GoblinState.Move => 15f,
            _ when currentState == GoblinState.Attack => 0f,
            _ when currentState == GoblinState.Dead => 0f,
            _ => _goblinSpeed
        };

        _goblinSpeed = newSpeed;
    }

    private bool IsAttack()
    {
        if(Vector3.Distance(transform.position, _playerTransform.position)<= 2f)
        {
            return true;
        }
        else return false;     
    }

    private void SetGoblinDamage()
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
    private void SetGoblinState()
    {
        var movementDirection = _movementDirection.normalized;
        var currentState = GetGoblinState();
        var isAttack = IsAttack();
        var IsDead = GetIsDead();

        var newState = currentState switch
        {
            _ when movementDirection != Vector3.zero && !isAttack && IsDead => GoblinState.Dead,
            _ when movementDirection != Vector3.zero && !isAttack && !IsDead => GoblinState.Move,
            _ when movementDirection != Vector3.zero && isAttack && !IsDead => GoblinState.Attack,
            _ => GoblinState.Move
        };

        if(newState == currentState) { return; }
        else ChangeState(newState);
    }
    public enum GoblinState
    {
        Move, Attack, Dead
    }

    private void ChangeState(GoblinState newState)
    {
        if(_currentState == newState) { return; }

        _currentState = newState;
    }

    public GoblinState GetGoblinState()
    {
        return _currentState;
    }
    private void SetGoblinAnim()
    {
        var currentState = GetGoblinState();

        switch(currentState)
        {
            case GoblinState.Move:
            _goblinAnimator.SetBool("IsMove", true);
            _goblinAnimator.SetBool("IsDead", false);
            _goblinAnimator.SetBool("IsAttack", false);
            break;
            case GoblinState.Dead:
            _goblinAnimator.SetBool("IsMove", false);
            _goblinAnimator.SetBool("IsDead", true);
            _goblinAnimator.SetBool("IsAttack", false);
            break;
            case GoblinState.Attack:
            _goblinAnimator.SetBool("IsMove", false);
            _goblinAnimator.SetBool("IsDead", false);
            _goblinAnimator.SetBool("IsAttack", true);
            break;
        }
    }
    private void StateWorking()
    {
        var currentState = GetGoblinState();

        switch (currentState)
        {
            case GoblinState.Move:
            Debug.Log("GOBLİN MOVE");
            break;
            case GoblinState.Attack:
            Debug.Log("GOBLİN ATTACK");
            break;
            case GoblinState.Dead:
            Debug.Log("GOBLİN DEAD");
            break;
        }
    }

    public void Damage(float damageAmount)
    {
        _currentGoblinHeal -= damageAmount;

        if(_currentGoblinHeal <= 0f)
        {
            _isDead = true;
        }
        if(_isDead)
        {
            Destroy(gameObject, _destroyCooldown);
        }
    }

    private bool GetIsDead()
    {
        return _isDead;
    }
}
