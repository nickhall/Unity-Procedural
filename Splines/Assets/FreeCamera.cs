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

    Vector3 lastMousePosition;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

	void Update()
    {
        if (EnableMovement)
        {
            Vector3 mouseDelta = lastMousePosition - Input.mousePosition;

            Vector3 offset = Vector3.zero;
            Vector3 rotation = Vector3.zero;
            Vector3 dragPositionOffset = Vector3.zero;
            Vector3 targetPosition;
            Quaternion targetRotation;

            // Forward/back
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                offset += Input.GetKey(KeyCode.S) ? -1 * Vector3.forward : Vector3.forward;
            }

            // Left/right
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                offset += Input.GetKey(KeyCode.A) ? Vector3.left : -1 * Vector3.left;
            }

            // Rotation
            // By key:
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
            {
                float rotationOffset = Input.GetKey(KeyCode.Q) ? -RotationKeySpeed : RotationKeySpeed;
                rotation.y += rotationOffset * Time.deltaTime;
            }
            // By right click:
            if (Input.GetMouseButton(1))
            {
                float xRotation = Input.GetAxis("Mouse X");
                float yRotation = Input.GetAxis("Mouse Y");
                rotation += new Vector3(yRotation, -xRotation, 0).normalized * RotationSensitivity;
            }

            // Middle click drag movement
            if (Input.GetMouseButton(2))
            {
                //Vector3 newMovement = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
                //dragPositionOffset.x = mouseDelta.x;
                //dragPositionOffset.z = mouseDelta.y;
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

            offset = offset.normalized;
            offset *= MovementSpeed;
            transform.Translate(offset * Time.deltaTime);
            transform.Translate(dragPositionOffset, Space.World);

            Vector3 eulers = transform.eulerAngles;
            transform.eulerAngles = new Vector3(Mathf.Clamp(eulers.x + rotation.x, -80, 80), eulers.y + rotation.y, eulers.z + rotation.z);

            lastMousePosition = Input.mousePosition;
        }
	}
}