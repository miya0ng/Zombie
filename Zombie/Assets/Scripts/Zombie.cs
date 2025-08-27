using System;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public enum State
    {
        Idle,
        Trace,
        Attack,
        Die
    }

    private State currentState;

    public Transform target;

    public float traceDist = 10.0f;
    public float attackDist = 2.0f;

    public float damage = 20.0f;
    public float lastAttackTime;
    public float attackDelay = 1.0f;

    private Animator animator;
    private NavMeshAgent navMeshAgent;

    public State state
    {
        get { return currentState; }
        set
        {
            var prev = currentState;
            currentState = value;
            switch (currentState)
            {
                case State.Idle:
                    animator.SetBool("HasTarget", false);
                    navMeshAgent.isStopped = true;
                    break;
                case State.Trace:
                    animator.SetBool("HasTarget", true);
                    navMeshAgent.isStopped = false;
                    break;
                case State.Attack:
                    animator.SetBool("HasTarget", false);
                    navMeshAgent.isStopped = true;
                    break;
                case State.Die:
                    animator.SetTrigger("Die");
                    navMeshAgent.isStopped = true;
                    break;
            }
        }
    }

    public void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Trace:
                UpdateTrace();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.Die:
                UpdateDie();
                break;
        }
    }

    private void UpdateDie()
    {
        throw new NotImplementedException();
    }

    private void UpdateAttack()
    {
        if (target == null || (target != null && Vector3.Distance(transform.position, target.position) > attackDist))
        {
            state = State.Trace;
            return;
        }
        //transform.LookAt(target);
        var lookPos = target.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        if (Time.time - lastAttackTime > attackDelay)
        {
            lastAttackTime = Time.time;
            //animator.SetTrigger("Attack");
            //target.GetComponent<LivingEntity>().TakeDamage(damage, transform.position, Vector3.up);
            var damageable = target.GetComponent<IDamagable>();
            if (damageable != null)
            {
                damageable.OnDamage(damage, transform.position, -transform.forward);
            }
        }
    }

    private void UpdateTrace()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) <= attackDist)
        {
            state = State.Attack;
            return;
        }
        if (target == null && Vector3.Distance(transform.position, target.position) > traceDist)
        {
            state = State.Idle;
            return;
        }
        navMeshAgent.SetDestination(target.position);
    }

    private void UpdateIdle()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) <= traceDist)
        {
            state = State.Trace;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    protected override void Die()
    {
        base.Die();
        state = State.Die;
    }
}