using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 followDistanceVector = new Vector3(0f, 1f, -5f);
    [SerializeField] private Vector2 verticalRotationAngleRange = new Vector2(-20f, 20f);
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private bool invertMouseXAxis = false;
    [SerializeField] private bool invertMouseYAxis = false;

    private float rotationX;
    private float rotationY;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        int invertMouseYAxisVal = invertMouseYAxis ? -1 : 1;
        rotationX += Input.GetAxis("Mouse Y") * mouseSensitivity * invertMouseYAxisVal * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, verticalRotationAngleRange.x, verticalRotationAngleRange.y);

        int invertMouseXAxisVal = invertMouseXAxis ? -1 : 1;
        rotationY += Input.GetAxis("Mouse X") * mouseSensitivity * invertMouseXAxisVal * Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler(-rotationX, rotationY, 0f);
        transform.position = followTarget.position + targetRotation * followDistanceVector;
        transform.rotation = targetRotation;
    }
}
