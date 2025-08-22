using UnityEngine;

//public static class TagManager
//{
//    public static readonly string Player = "Player";
//    public static readonly string Enemy = "Enemy";
//    public static readonly string Item = "Item";
//    public static readonly string Obstacle = "Obstacle";
//    public static readonly string Projectile = "Projectile";
//}

public class PlayerMovement : MonoBehaviour
{
    private static readonly int MoveHash = Animator.StringToHash("Move");

    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;

    private PlayerInput playerInput;
    private Rigidbody rb;
    private Animator animator;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //ȸ��
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, playerInput.Roatate * rotationSpeed * Time.fixedDeltaTime, 0f));
      

        //�̵�
        rb.MovePosition(rb.position + transform.forward * playerInput.Move * moveSpeed * Time.fixedDeltaTime);
     

        //�ִϸ��̼� ����
        if (animator != null)
        {
            animator.SetFloat(MoveHash, playerInput.Move);
        }
    }
}