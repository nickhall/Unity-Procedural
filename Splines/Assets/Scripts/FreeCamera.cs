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
    public float MinimumLookAngle = -20f;
    public float MaximumLookAngle = 75f;
    public float CameraSnap = 10f;
    public float RotationSnap = 15f;
    public float MinCameraHeight = 2f;
    public float MaxCameraHeight = 100f;
    public float DistanceModifier;
    public float MaximumZoomAngle = 50f;
    Vector3 newRotation;
    Vector3 zoomDirection;

    Vector3 lastMousePosition;
    Quaternion originalRotation;
    Vector3 targetPosition;
    Quaternion targetRotation;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
        newRotation = Vector3.zero;
        targetPosition = transform.position;
    }

	void Update()
    {
        if (EnableMovement)
        {
            Vector3 mouseDelta = lastMousePosition - Input.mousePosition;
            DistanceModifier = Mathf.Sqrt(transform.position.y);
            float totalModifier =  MovementSpeed * DistanceModifier;

            Vector3 movementOffset = Vector3.zero;
            Vector3 dragPositionOffset = Vector3.zero;
            bool keyMovement = false;

            // Movement
            // Forward/back
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                Vector3 newForward = transform.forward;
                newForward.y = 0;
                movementOffset += Input.GetKey(KeyCode.S) ? -1 * newForward : newForward;
                keyMovement = true;
            }

            // Left/right
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                movementOffset += Input.GetKey(KeyCode.A) ? -transform.right : transform.right;
                keyMovement = true;
            }

            // Middle click drag movement
            // Disabled if we're moving with wasd because it looks and feels awkward
            if (Input.GetMouseButton(2) && !keyMovement)
            {
                dragPositionOffset += transform.forward * -Input.GetAxis("Mouse Y");
                dragPositionOffset += transform.right * -Input.GetAxis("Mouse X");
                dragPositionOffset.y = 0;
                dragPositionOffset = dragPositionOffset.normalized * DragSpeed * DistanceModifier;
            }

            // Zoom
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            Vector3 zoomOffset = Vector3.zero;
            if (zoom != 0)
            {
                float angle = Vector3.Angle(Vector3.down, transform.forward);
                zoomDirection = transform.forward;
                if (angle > MaximumZoomAngle)
                {
                    zoomDirection = Quaternion.AngleAxis(MaximumZoomAngle, -transform.right) * Vector3.down;
                    zoomDirection = zoomDirection.normalized;
                }
                zoomOffset = zoomDirection * Mathf.Sign(zoom) * ZoomSpeed;
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
                newRotation.x = Mathf.Clamp(newRotation.x - Input.GetAxis("Mouse Y") * RotationSensitivity, MinimumLookAngle, MaximumLookAngle);
            }

            movementOffset = movementOffset.normalized * totalModifier;
            targetPosition += movementOffset * Time.deltaTime;// +zoomOffset;
            targetPosition += dragPositionOffset;
            
            Vector3 testVector = targetPosition + zoomOffset;
            if (testVector.y > MinCameraHeight && testVector.y < MaxCameraHeight)
            {
                targetPosition += zoomOffset;
            }
            targetPosition.y = Mathf.Clamp(targetPosition.y, MinCameraHeight, MaxCameraHeight);

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * CameraSnap);

            float t = Time.deltaTime * RotationSnap;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0), Quaternion.Euler(newRotation), t);

            Debug.DrawLine(transform.position, transform.position + transform.forward, Color.green);
            Debug.DrawLine(transform.position, transform.position + zoomDirection, Color.blue);
        }
	}
}