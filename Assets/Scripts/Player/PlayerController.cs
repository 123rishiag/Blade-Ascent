using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;

    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 500f;

    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector3 groundCheckOffset = new Vector3(0f, 0.1f, 0.07f);
    [SerializeField] private LayerMask groundLayer;

    private Quaternion targetRotation;
    private bool isGrounded;

    private void Start()
    {
        targetRotation = Quaternion.identity;
        isGrounded = true;
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        var moveDirection = cameraController.GetPlanarRotation() * moveInput;
        var moveVelocity = moveDirection * moveSpeed * Time.deltaTime;

        PerformGroundCheck();

        if (isGrounded)
        {
            moveVelocity.y = -0.5f;
        }
        else
        {
            moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        }

        if (moveInput.magnitude > 0f)
        {
            characterController.Move(moveVelocity);
            targetRotation = Quaternion.LookRotation(moveDirection);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", Mathf.Clamp01(moveInput.magnitude), 0.2f, Time.deltaTime);
    }

    private void PerformGroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}
