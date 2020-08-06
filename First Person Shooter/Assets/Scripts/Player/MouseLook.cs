using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float m_moveSensitivity = 50f;
    public Transform m_playerBody;

    private float xRot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MouseMovement();
    }

    public void MouseMovement()
    {
        float inputX = Input.GetAxis("Mouse X") * m_moveSensitivity * Time.deltaTime;
        float inputY = Input.GetAxis("Mouse Y") * m_moveSensitivity * Time.deltaTime;

        xRot -= inputY;
        xRot = Mathf.Clamp(xRot, -90f, 90);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        m_playerBody.Rotate(Vector3.up * inputX);
    }
}
