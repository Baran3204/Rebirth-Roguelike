using UnityEngine;

public class SkeletonController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Animator _skeletonAnimator;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Settings")]
    [SerializeField] private float _skeletonSpeed;

    private Rigidbody2D _skeletonRB;
    private SkeletonState _currentState = SkeletonState.Move;
    private Vector3  _movementDirection;

    private void Awake() 
    {
       _skeletonRB = GetComponent<Rigidbody2D>();
       ChangeState(SkeletonState.Move);
    }

    private void Update() 
    {
        SetSkeletonFlip();
        SetSkeletonState();
        SetSkeletonAnim();
        SetSkeletonSpeed();
    }

    private void FixedUpdate() 
    {
        SetSkeletonDirection();  
        StateWorking();  
    }


    private void SetSkeletonDirection()
    {

        _movementDirection =  _playerTransform.position - transform.position;

        _skeletonRB.linearVelocity = _movementDirection * _skeletonSpeed * Time.deltaTime;
    } 

    private void SetSkeletonFlip()
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
    private void SetSkeletonSpeed()
    {
        var currentState = GetSkeletonState();

        var newSpeed = currentState switch
        {
            _ when currentState == SkeletonState.Move => 15f,
            _ when currentState == SkeletonState.Attack => 0f,
            _ when currentState == SkeletonState.Dead => 0f,
            _ => _skeletonSpeed
        };

        _skeletonSpeed = newSpeed;
    }

    private bool IsAttack()
    {
        if(Vector3.Distance(transform.position, _playerTransform.position)<= 2f)
        {
            return true;
        }
        else return false;     
    }

    private void SetSkeletonState()
    {
        var movementDirection = _movementDirection.normalized;
        var currentState = GetSkeletonState();
        var isAttack = IsAttack();

        var newState = currentState switch
        {
            _ when movementDirection == Vector3.zero && !isAttack => SkeletonState.Dead,
            _ when movementDirection != Vector3.zero && !isAttack => SkeletonState.Move,
            _ when movementDirection != Vector3.zero && isAttack=> SkeletonState.Attack,
            _ => SkeletonState.Move
        };

        if(newState == currentState) { return; }
        else ChangeState(newState);
    }
    public enum SkeletonState
    {
        Move, Attack, Dead
    }

    private void ChangeState(SkeletonState newState)
    {
        if(_currentState == newState) { return; }

        _currentState = newState;
    }

    public SkeletonState GetSkeletonState()
    {
        return _currentState;
    }
    private void SetSkeletonAnim()
    {
        var currentState = GetSkeletonState();

        switch(currentState)
        {
            case SkeletonState.Move:
            _skeletonAnimator.SetBool("IsMove", true);
            _skeletonAnimator.SetBool("IsDead", false);
            _skeletonAnimator.SetBool("IsAttack", false);
            break;
            case SkeletonState.Dead:
            _skeletonAnimator.SetBool("IsMove", false);
            _skeletonAnimator.SetBool("IsDead", true);
            _skeletonAnimator.SetBool("IsAttack", false);
            break;
            case SkeletonState.Attack:
            _skeletonAnimator.SetBool("IsMove", false);
            _skeletonAnimator.SetBool("IsDead", false);
            _skeletonAnimator.SetBool("IsAttack", true);
            break;
        }
    }
    private void StateWorking()
    {
        var currentState = GetSkeletonState();

        switch (currentState)
        {
            case SkeletonState.Move:
            Debug.Log("SKELETON MOVE");
            break;
            case SkeletonState.Attack:
            Debug.Log("SKELETON ATTACK");
            break;
            case SkeletonState.Dead:
            Debug.Log("SKELETON DEAD");
            break;
        }
    }
}
