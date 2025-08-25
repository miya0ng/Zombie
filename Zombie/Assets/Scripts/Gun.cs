using System.Collections;
using System.Data;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading,
    }

    private State currentState = State.Ready;
    //상태별로 실행할 업데이트 만들어야함
    //바뀔 때 실행되는 애 자리 잡는거 필요

    public State CurrentState
    {
        get { return currentState; }
        private set
        {
            currentState = value;
            switch (currentState)
            {
                case State.Ready:
                    UpdateReady();
                    break;
                case State.Empty:
                    UpdateEmpty();
                    break;
                case State.Reloading:
                    UpdateReloading();
                    break;
            }
        }
    }

    public GunData gunData;

    public ParticleSystem muzzleEffect;
    public ParticleSystem shellEffect;

    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public Transform firePosition;

    public int ammoRemain;

    public int magAmmo;

    private float lastFireTime;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        lastFireTime = 0f;
        CurrentState = State.Ready;
    }
    private void Update()
    {

    }
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        audioSource.PlayOneShot(gunData.shootClip);

        muzzleEffect.Play();
        shellEffect.Play();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePosition.position);
        lineRenderer.SetPosition(1, hitPosition);
        Vector3 endPos = firePosition.position + firePosition.forward * 10f;

        lineRenderer.SetPosition(1, endPos);
        yield return new WaitForSeconds(1f);
        lineRenderer.enabled = false;
    }

    private void UpdateReady()
    {
        // 총쏘기 
    }

    private void UpdateEmpty()
    {

    }
    private void UpdateReloading()
    {

    }

    public void Fire()
    {
        if (currentState == State.Ready && Time.time > (lastFireTime + gunData.timeBetFire))
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }
    private void Shoot()
    {
        Vector3 hitPosition = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(firePosition.position, firePosition.forward, out hit, gunData.fireDistance))
        {
            hitPosition = hit.point;
            var taret = hit.collider.GetComponent<IDamagable>();
            //trigger or Collider 맞았니 안맞았니, -> 제일 먼저 맞은녀석이 충돌검사 결과로 넘어옴. out 으로 넘겨야 hit 조회해서 알수 있게됨
            // hit.point -> '.point' 부딪힌 점 반환하는애

            if (taret != null)
            {
                taret.OnDamage(gunData.damage, hit.point, hit.normal);//@
            }
        }
        else
        {
            hitPosition = firePosition.position + firePosition.forward * gunData.fireDistance;
        }
        StartCoroutine(ShotEffect(hitPosition));
        --magAmmo;
        if (magAmmo == 0)
        {
            CurrentState = State.Empty;
        }
    }

    public bool Reload()//@
    {
        if (CurrentState == State.Reloading || ammoRemain == 0 || magAmmo == gunData.magCapacity)
        {
            return false;
        }

        StartCoroutine(CoReload());
        return true;
    }

    public IEnumerator CoReload()
    {
        CurrentState = State.Reloading;
        audioSource.PlayOneShot(gunData.reloadClip);
        yield return new WaitForSeconds(gunData.reloadTime);

        magAmmo += ammoRemain;
        if(magAmmo>=gunData.magCapacity)
        {
            magAmmo = gunData.magCapacity;
            ammoRemain -= magAmmo;
        }
        else
        {
            ammoRemain = 0;
        }
        CurrentState = State.Ready;
    }
}