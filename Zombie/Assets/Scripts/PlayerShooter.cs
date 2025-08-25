using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public static readonly int IdReload = Animator.StringToHash("Reload");
    public Gun gun;

    private PlayerInput input;
    private Animator animator;

    public Transform leftHandMount;
    public Transform rightHandMount;

    public Transform gunPivot;
    private void Awake()
    {
       input = GetComponent<PlayerInput>();
       animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(input.Fire)
        {
            gun.Fire();
        }
        else if(input.Reload)
        {
            if (gun.Reload())
            {
                animator.SetTrigger(IdReload);
            }
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);

        //weight => ����ġ
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);
        //SetIKPositionWeight , SetIKHintPositionWeight //?

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}