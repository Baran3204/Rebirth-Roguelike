using System;
using UnityEngine;
using UnityEngine.AI;

public class MushroomController : MonoBehaviour, IDamagables
{   
    [Header("References")]
    [SerializeField] private Animator _agentAnimator;
    private Transform _playerTransform;
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Settings")]
    [SerializeField] private float _agentSpeed;
    [SerializeField] private float _damageCoolDown;
    [SerializeField] private float _maxHealAgent;
    [SerializeField] private float _destroyCooldown;
    [SerializeField] private float _damageAmount;

    private NavMeshAgent _agent;
    private AgentState _currentState = AgentState.Move;
    private float _currentDamageCooldown, _currentAgentHeal;
    private bool _isDead;
    private void Awake() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

       ChangeState(AgentState.Move);
       _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
       _currentDamageCooldown = _damageCoolDown;
       _currentAgentHeal = _maxHealAgent;
    }

    private void Update() 
    {
        SetAgentStopping();
        SetAgentFlip();
        SetAgentState();
        SetAgentAnim();
        SetAgentSpeed();
        SetAgentDamage();
    }

    private void FixedUpdate() 
    {
        SetAgentDirection();  
        StateWorking();  
    }


    private void SetAgentDirection()
    {
            _agent.SetDestination(_playerTransform.position);
    } 

    private void SetAgentFlip()
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
    private void SetAgentSpeed()
    {
        var currentState = GetAgentState();

        var newSpeed = currentState switch
        {
            _ when currentState == AgentState.Move => _agentSpeed,
            _ when currentState == AgentState.Attack => 0f,
            _ when currentState == AgentState.Dead => 0f,
            _ => _agentSpeed
        };

        _agent.speed = newSpeed;
    }

    private bool IsAttack()
    {
        if(_agent.remainingDistance <= _agent.stoppingDistance)
        {
            return true;
        }
        else 
        {
            return false;
        }     
    }

    private void SetAgentStopping()
    {
        var currentState = GetAgentState();

        bool newStopping = currentState switch
        {
            _ when currentState == AgentState.Attack => true,
            _ when currentState == AgentState.Dead => true,
            _ when currentState == AgentState.Move => false,
            _ => false
        };

        _agent.isStopped = newStopping;
    }

    private void SetAgentDamage()
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
    private void SetAgentState()
    {
        var currentState = GetAgentState();
        var isAttack = IsAttack();
        var IsDead = GetIsDead();

        var newState = currentState switch
        {
            _ when  !isAttack && IsDead => AgentState.Dead,
            _ when  !isAttack && !IsDead => AgentState.Move,
            _ when  isAttack && !IsDead => AgentState.Attack,
            _ => AgentState.Move
        };

        if(newState == currentState) { return; }
        else ChangeState(newState);
    }
    public enum AgentState
    {
        Move, Attack, Dead
    }

    private void ChangeState(AgentState newState)
    {
        if(_currentState == newState) { return; }

        _currentState = newState;
    }

    public AgentState GetAgentState()
    {
        return _currentState;
    }
    private void SetAgentAnim()
    {
        var currentState = GetAgentState();

        switch(currentState)
        {
            case AgentState.Move:
            _agentAnimator.SetBool("IsMove", true);
            _agentAnimator.SetBool("IsDead", false);
            _agentAnimator.SetBool("IsAttack", false);
            break;
            case AgentState.Dead:
            _agentAnimator.SetBool("IsMove", false);
            _agentAnimator.SetBool("IsDead", true);
            _agentAnimator.SetBool("IsAttack", false);
            break;
            case AgentState.Attack:
            _agentAnimator.SetBool("IsMove", false);
            _agentAnimator.SetBool("IsDead", false);
            _agentAnimator.SetBool("IsAttack", true);
            break;
        }
    }
    private void StateWorking()
    {
        var currentState = GetAgentState();

        switch (currentState)
        {
            case AgentState.Move:
            Debug.Log("Agent MOVE");
            break;
            case AgentState.Attack:
            Debug.Log("Agent ATTACK");
            break;
            case AgentState.Dead:
            Debug.Log("Agent DEAD");
            break;
        }
    }

    public void Damage(float damageAmount)
    {
        _currentAgentHeal -= damageAmount;

        if(_currentAgentHeal <= 0f)
        {
            _isDead = true;
            if(_isDead)
            {   
                Destroy(gameObject, _destroyCooldown);
            }
        }
    }

    private bool GetIsDead()
    {
        return _isDead;
    }   
}
