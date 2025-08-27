using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public UiHud uiHud;

    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemPickupClip;

    private AudioSource audioSource;
    private Animator animator;

    private PlayerMovement movement;
    private PlayerShooter shooter;

    private static readonly int DieHash = Animator.StringToHash("Die");

    //private enum State
    //{
    //    Idle,
    //    Attacked,
    //    Shoot,
    //    Die,
    //}

    //private State currentState;

    //public State Currentstate;
    //{
    //    get { return currentState; }
    //    set
    //    {
    //        var prev = currentState;
    //        currentState = value;
    //        switch (currentState)
    //        {
    //            case State.Idle:

    //                break;
    //            case State.Attacked:

    //                break;
    //            case State.Shoot:

    //                break;
    //            case State.Die:

    //                break;
    //        }
    //    }
    //}
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        shooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.value = Health / MaxHealth;

        movement.enabled = true;
        shooter.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnDamage(10f, Vector3.zero, Vector3.zero);
        }
       
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead) return;

        base.OnDamage(damage, hitPoint, hitNormal);
        healthSlider.value = Health / MaxHealth;
        audioSource.PlayOneShot(hitClip);
    }

    //public void AddHealth(float amount)
    //{
    //    Health += amount;
    //}
    protected override void Die()
    {
        base.Die();

        healthSlider.gameObject.SetActive(false);
        animator.SetTrigger(DieHash);
        audioSource.PlayOneShot(deathClip);

        movement.enabled = false;
        shooter.enabled = false;

        uiHud.isGameOver = true;
    }

    public void Heal(int amount)
    {
        Health = Mathf.Min(Health + amount, MaxHealth);
        Debug.Log("heal");
        healthSlider.value = Health / MaxHealth;
    }
}