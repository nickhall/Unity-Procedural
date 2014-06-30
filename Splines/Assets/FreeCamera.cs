using UnityEngine;
using System.Collections;

public class FreeCamera : MonoBehaviour
{
    public float MovementSpeed = 30f;
    public float RotationKeySpeed = 50f;
    public bool EnableMovement = true;
    public float ZoomSpeed = 5f;
    public float DragSpeed = 10f;
    public float RotationSensitivity = 10f;
    public float MinimumAngle = -60f;
    public float MaximumAngle = 60f;
    public Vector3 MyRotation;
    Vector3 newRotation;

    Vector3 lastMousePosition;
    Quaternion originalRotation;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
        originalRotation = transform.rotation;
        newRotation = Vector3.zero;
    }

	void Update()
    {
        if (EnableMovement)
        {
            Vector3 mouseDelta = lastMousePosition - Input.mousePosition;

            Vector3 movementOffset = Vector3.zero;
            originalRotation = transform.rotation;
            Vector3 dragPositionOffset = Vector3.zero;
            Vector3 targetPosition;
            Quaternion targetRotation;

            // Movement
            // Forward/back
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                Vector3 newForward = transform.forward;
                movementOffset += Input.GetKey(KeyCode.S) ? -1 * newForward : newForward;
            }

            // Left/right
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                movementOffset += Input.GetKey(KeyCode.A) ? Vector3.left : -1 * Vector3.left;
            }

            // Middle click drag movement
            if (Input.GetMouseButton(2))
            {
                dragPositionOffset.x = -Input.GetAxis("Mouse X");
                dragPositionOffset.z = -Input.GetAxis("Mouse Y");
                dragPositionOffset = dragPositionOffset.normalized * DragSpeed;
            }

            // Zoom
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            if (zoom != 0)
            {
                Vector3 zoomOffset = Vector3.forward * Mathf.Sign(zoom) * ZoomSpeed;
                transform.Translate(zoomOffset);
            }

            // Rotation
            // By key:
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
            {
                float rotationOffset = Input.GetKey(KeyCode.Q) ? -RotationKeySpeed : RotationKeySpeed;
                newRotation.y += rotationOffset * Time.deltaTime;
            }
            // By right click:
            if (Input.GetMouseButton(1))
            {
                newRotation.y = (newRotation.y + Input.GetAxis("Mouse X") * RotationSensitivity) % 360;
                newRotation.x = Mathf.Clamp(newRotation.x - Input.GetAxis("Mouse Y") * RotationSensitivity, MinimumAngle, MaximumAngle);
            }

            movementOffset = movementOffset.normalized * MovementSpeed;
            transform.Translate(movementOffset * Time.deltaTime);
            transform.Translate(dragPositionOffset, Space.World);

            transform.eulerAngles = newRotation;

            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log();
            }

            Debug.DrawLine(transform.position, transform.position + transform.forward, Color.green);
        }
	}
}