using MoreMountains.Tools;
using UnityEngine;

public class MainCharacterMovement : MonoBehaviour
{
    private Rigidbody2D playerRb;

    private float speed = 2f;
    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Move(Vector2 vector2)
    {
        movement = vector2;
        transform.localScale = new Vector3(vector2.x < 0 ? -1 : 1, 1, 1);
        animator.Play(Mathf.Abs(vector2.x) == 0 ? "p_idle" : "p_run");
    }
    void Update()
    {
        if(!MainCharacterController.instance.isAbilityAction)
        {
            movement = movement.normalized * speed;

            playerRb.velocity = new Vector3(movement.x, playerRb.velocity.y, movement.y);
        }
       
    }
}