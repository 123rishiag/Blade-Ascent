using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;

    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    [Header("Locomotion Variables")]
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float accelerationSpeed = 5f;
    [SerializeField] private float decelerationSpeed = 5f;
    [SerializeField] private float rotationSpeed = 500f;

    [Header("Ground Check Variables")]
    [SerializeField] private float groundCheckRadius = 0.2f;

    [SerializeField] private Vector3 groundCheckOffset = new Vector3(0f, 0.1f, 0.07f);
    [SerializeField] private LayerMask groundLayer;

    // Private Variables
    private Vector3 lastMoveDirection;
    private float currentSpeed;
    private float verticalVelocity;
    private Quaternion targetRotation;
    private bool isGrounded;

    private void Start()
    {
        lastMoveDirection = Vector3.zero;
        currentSpeed = 0;
        verticalVelocity = 0;
        targetRotation = Quaternion.identity;
        isGrounded = true;
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 moveDirection;
        SetSpeedAndDirection(out moveDirection);

        PerformGroundCheck();
        ApplyVelocity();
        MoveTowards(moveDirection);
        RotateTowards(moveDirection);

        animator.SetFloat("moveAmount", currentSpeed / moveSpeed);
    }

    private void SetSpeedAndDirection(out Vector3 _moveDirection)
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        if (moveInput.magnitude > 0.01f)
        {
            _moveDirection = cameraController.GetPlanarRotation() * moveInput;
            lastMoveDirection = _moveDirection;
            currentSpeed += accelerationSpeed * Time.deltaTime;
        }
        else
        {
            _moveDirection = lastMoveDirection;
            currentSpeed -= decelerationSpeed * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, moveSpeed);
    }

    private void ApplyVelocity()
    {
        if (isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void MoveTowards(Vector3 _moveDirection)
    {
        Vector3 currentVelocity = _moveDirection * currentSpeed;
        currentVelocity.y = verticalVelocity;
        characterController.Move(currentVelocity * Time.deltaTime);
    }

    private void RotateTowards(Vector3 _moveDirection)
    {
        if (_moveDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(_moveDirection);
        }

        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void PerformGroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius,
            groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}