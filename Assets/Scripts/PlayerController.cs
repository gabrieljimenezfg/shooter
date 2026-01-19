using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static readonly int HoldingGrenade = Animator.StringToHash("HoldingGrenade");
    private const string MOVEMENT_ACTION_NAME = "Move";
    private const string RELOAD_INPUT = "Reload";
    private const string SHOOT_INPUT = "Shoot";
    private const string ANIMATOR_HORIZONTAL = "Horizontal";
    private const string ANIMATOR_VERTICAL = "Vertical";
    private const string ANIMATOR_SHOOTING = "Shooting";
    private const string ANIMATOR_RELOAD = "Reload";
    private const string CAMERA_LOOK = "Look";

    private Animator animator;
    private PlayerInput playerInput;
    private Rigidbody rb;

    [SerializeField] private float cameraSensitivity;
    [SerializeField] private float speed;
    [SerializeField] private Transform followTarget;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float throwForce;

    private LineRenderer lineRenderer;

    [SerializeField] private Transform leftHand, rightHand, grenadeSpawnPoint;

    public static event Action<float> TookDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        lineRenderer = grenadeSpawnPoint.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        GameManager.Instance.Died += PlayerDied;
    }

    private Vector3 GetGrenadeThrowDirection()
    {
        var throwDirection = (Camera.main.transform.forward + Vector3.up) * throwForce;
        return throwDirection;
    }

    private void DrawGrenadeTrajectory()
    {
        if (lineRenderer.enabled)
        {
            var positionCount = 100;
            var throwDirection = GetGrenadeThrowDirection();
            lineRenderer.positionCount = positionCount;

            for (int i = 0; i < positionCount; i++)
            {
                float t = i * 0.1f;
                // MRUA
                Vector3 position = grenadeSpawnPoint.position + throwDirection * t + 0.5f * Physics.gravity * t * t;
                lineRenderer.SetPosition(i, position);
            }
        }
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

        DrawGrenadeTrajectory();
    }

    private void LateUpdate()
    {
        Vector2 lookInput = playerInput.actions[CAMERA_LOOK].ReadValue<Vector2>();

        followTarget.localEulerAngles += new Vector3(lookInput.y * cameraSensitivity * Time.deltaTime, 0, 0);
        transform.eulerAngles += new Vector3(0, lookInput.x * cameraSensitivity * Time.deltaTime, 0);
    }

    private Weapon GetEquippedWeapon()
    {
        return GameManager.Instance.GetEquippedWeapon();
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GetEquippedWeapon().TriggerPressed();
            animator.SetBool(ANIMATOR_SHOOTING, true);
            playerInput.actions[RELOAD_INPUT].Disable();
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            GetEquippedWeapon().TriggerReleased();
            animator.SetBool(ANIMATOR_SHOOTING, false);
            playerInput.actions[RELOAD_INPUT].Enable();
        }
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            GetEquippedWeapon().Reload();
            animator.SetTrigger(ANIMATOR_RELOAD);
            playerInput.actions[SHOOT_INPUT].Disable();
        }
    }

    private void PlayerDied()
    {
        var ragdollPrefab = Resources.Load("SwatRagdoll");
        Instantiate(ragdollPrefab, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        TookDamage?.Invoke(damage);
    }

    public void GrabGrenade()
    {
    }

    public void LetGoOfGrenade()
    {
        lineRenderer.enabled = false;
        var grenade = grenadeSpawnPoint.GetChild(0).transform;
        grenade.parent = null;

        var grenadeRigidbody = grenade.GetComponent<Rigidbody>();
        var grenadeCollider = grenade.GetComponent<Collider>();

        grenadeCollider.enabled = true;
        grenadeRigidbody.isKinematic = false;
        grenadeRigidbody.linearVelocity = GetGrenadeThrowDirection();

        grenade.GetComponent<Grenade>().StartCountdown();
    }

    public void ThrowGrenade(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetBool(HoldingGrenade, true);
            lineRenderer.enabled = true;
            GetEquippedWeapon().transform.parent = leftHand;
            Instantiate(grenadePrefab, grenadeSpawnPoint.position, grenadeSpawnPoint.rotation, grenadeSpawnPoint);
        }

        if (context.canceled)
        {
            animator.SetBool(HoldingGrenade, false);
        }
    }
}