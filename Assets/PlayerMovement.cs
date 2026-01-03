using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private float jumpForce = 2f;
    private float speed = 10f;
    private float gravity = -16f;
    private float mouseSensitivity = 250f;

    private CharacterController CController;
    public GameObject CamRotate;
    public GameObject PlayerObject;
    public Transform groundCheck;
    public LayerMask groundMask;
    private float groundDistance = 0.2f;
    private float yRotation = 0f;
    private float xRotation = 0f;

    private Vector3 resetPoint;

    private Vector3 velocity;

    private bool canJump = false;
    private bool IsCursorLocked = true;
    
    private void Start()
    {
        CController = GetComponent<CharacterController>();

        RespawnPlayer();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        ResetPosition();
        CursorLock();
        FallGravity();

        if (IsCursorLocked)
        {
            Move();
            PlayerRotate();
            Jump();
        }
    }

    private void RespawnPlayer()
    {
        int randomX = Random.Range(-13, 14);
        int randomY = Random.Range(-13, 14);
        resetPoint = new Vector3(randomX, 1.08f, randomY);
        CController.enabled = false;
        transform.position = resetPoint;
        CController.enabled = true;
    }

    private void ResetPosition()
    {
        if (transform.position.y < -5)
        {
            RespawnPlayer();
        }
    }

    //玩家物理與操作
    private void FallGravity()
    {
        canJump = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (canJump && velocity.y < 0)
        {
            velocity.y = -0.2f;
        }

        velocity.y += gravity * Time.deltaTime;
        CController.Move(velocity * Time.deltaTime);
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * y;
        CController.Move(move * speed * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    private void PlayerRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation -= mouseX;
        transform.localRotation = Quaternion.Euler(0f, -yRotation, 0f);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        CamRotate.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }


    private void CursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsCursorLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            IsCursorLocked = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !IsCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            IsCursorLocked = true;
        }
    }
}