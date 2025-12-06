using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const string MOVEMENT_ACTION_NAME = "Move";
    private const string ANIMATOR_HORIZONTAL = "Horizontal";
    private const string ANIMATOR_VERTICAL = "Vertical";
    private const string ANIMATOR_SHOOTING = "Shooting";
    private const string CAMERA_LOOK = "Look";

    private Animator animator;
    private PlayerInput playerInput;
    private Rigidbody rb;
    [SerializeField] private float cameraSensitivity;
    [SerializeField] private float speed;
    [SerializeField] private Transform followTarget;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private Weapon weapon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector2 leftStickInput = playerInput.actions[MOVEMENT_ACTION_NAME].ReadValue<Vector2>();

        var currentHorizontal = animator.GetFloat(ANIMATOR_HORIZONTAL);
        var currentVertical = animator.GetFloat(ANIMATOR_VERTICAL);

        var lerpSpeedMultiplier = lerpSpeed * Time.deltaTime;
        var newHorizontal = Mathf.Lerp(currentHorizontal, leftStickInput.x, lerpSpeedMultiplier);
        var newVertical = Mathf.Lerp(currentVertical, leftStickInput.y, lerpSpeedMultiplier);

        animator.SetFloat(ANIMATOR_HORIZONTAL, newHorizontal);
        animator.SetFloat(ANIMATOR_VERTICAL, newVertical);

        Vector3 movement = ((transform.forward * leftStickInput.y) + (transform.right * leftStickInput.x)) * speed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }

    private void LateUpdate()
    {
        Vector2 lookInput = playerInput.actions[CAMERA_LOOK].ReadValue<Vector2>();

        followTarget.localEulerAngles += new Vector3(lookInput.y * cameraSensitivity * Time.deltaTime, 0, 0);
        transform.eulerAngles += new Vector3(0, lookInput.x * cameraSensitivity * Time.deltaTime, 0);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        weapon.Shoot();
        
        if (context.phase == InputActionPhase.Started)
        {
            animator.SetBool(ANIMATOR_SHOOTING, true);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            animator.SetBool(ANIMATOR_SHOOTING, false);
        }
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // reload
            weapon.Reload();
        }
    } 
}