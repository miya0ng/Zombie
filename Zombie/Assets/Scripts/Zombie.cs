using System;
using System.Linq;
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
    public ParticleSystem blood;

    public AudioClip zombieAttackClip;
    public AudioClip zombieDieClip;

    private State currentState;

    private Transform target;
    public Transform pivot;

    public float traceDist = 10.0f;
    public float attackDist = 2.0f;

    public float damage = 20.0f;
    public float lastAttackTime;
    public float attackDelay = 1.0f;

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Collider collider;

    private AudioSource audioSource;
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
        collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        collider.enabled = true;
    }

    private void Update()
    {
        blood.transform.position = pivot.transform.position;

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

        target = FindTarget(traceDist);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
        audioSource.PlayOneShot(zombieAttackClip);
        blood.Play();
    }

    protected override void Die()
    {
        base.Die();
        state = State.Die;
        collider.enabled = false;
        audioSource.PlayOneShot(zombieDieClip);
    }

    public LayerMask targetLayer;
    protected Transform FindTarget(float radius)
    {
        var colliders =  Physics.OverlapSphere(transform.position, radius, targetLayer.value);
        if(colliders.Length == 0)
        {
            return null;
        }

        var target = colliders.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();

        return target.transform;
    }
}