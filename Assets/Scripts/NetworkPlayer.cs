using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    private const string MOVEMENT_ACTION_NAME = "Move";
    private const string LOOK_ZEN_ACTION_NAME = "LookZen";

    private PlayerInput playerInput;
    [SerializeField] private float speed;
    private NetworkCamera networkCamera;
    private Rigidbody rb;
    [SerializeField] private NetworkBullet bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    private float hp;
    [SerializeField] private bool offline;

    private void Awake()
    {
        if (offline)
        {
            PhotonNetwork.OfflineMode = true;
        }

        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            networkCamera = Camera.main.GetComponent<NetworkCamera>();
            networkCamera.SetPlayer(transform);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hp);
        }
        else
        {
            hp = (float)stream.ReceiveNext();
        }
    }

    private void HandleMovement()
    {
        Vector2 leftStickInput = playerInput.actions[MOVEMENT_ACTION_NAME].ReadValue<Vector2>();

        var verticalMovement = Vector3.right;
        var horizontalMovement = -Vector3.forward;
        Vector3 movement = ((verticalMovement * leftStickInput.y) + (horizontalMovement * leftStickInput.x)) * speed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }

    private void HandleRotation()
    {
        var cameraOffsetY = networkCamera.CameraOffset.y;
        var mousePosition = playerInput.actions[LOOK_ZEN_ACTION_NAME].ReadValue<Vector2>();

        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, cameraOffsetY));
        var playerRotation = transform.eulerAngles;
        transform.LookAt(worldMousePosition);
        transform.eulerAngles = new Vector3(playerRotation.x, transform.eulerAngles.y, playerRotation.z);
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        HandleMovement();
        HandleRotation();
    }

    /// <summary>
    /// Opcion 1 de disparo online, se sincroniza toda la funcion de disparar
    /// </summary>
    /// <param name="ctx"></param>
    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (!photonView.IsMine) return;

        if (ctx.performed)
        {
            this.PerformRPC(nameof(NetworkShoot), RpcTarget.All);
        }
    }

    [PunRPC]
    private void NetworkShoot()
    {
        Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }

    /// <summary>
    /// Opcion 2 de disparo online, solo se sincroniza la bala
    /// </summary>
    /// <param name="ctx"></param>
    public void Shoot2(InputAction.CallbackContext ctx)
    {
        if (!photonView.IsMine) return;

        if (ctx.performed)
        {
            var bulletInstance = PhotonNetwork.Instantiate(Constants.Prefabs.NetworkBullet, bulletSpawnPoint.position,
                bulletSpawnPoint.rotation);
            var rb = bulletInstance.GetComponent<Rigidbody>();
            rb.linearVelocity = bulletInstance.transform.forward * 0.1f;
        }
    }

    // TODO: move to network bullet 

    private void OnCollisionEnter(Collision other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.TryGetComponent<EnemyController>(out var enemy))
            {
                // enemy.TakeDamage(10, photonView.Owner);
            }
        }
    }


    // TODO: move to network enemy
    void TakeDamage(float damage, Player player)
    {
        hp -= damage;

        if (hp <= 0)
        {
            int playerDeathsInt;
            if (player.CustomProperties.TryGetValue("DeathCount", out var playerDeathsObject))
            {
                playerDeathsInt = (int)playerDeathsObject;
                playerDeathsInt++;
            }
            else
            {
                playerDeathsInt = 1;
            }

            var playerDeathHashTable = new Hashtable
            {
                {
                    "DeathCount", playerDeathsInt
                }
            };
            player.SetCustomProperties(playerDeathHashTable);
        }
        else
        {
            // take damage
        }
    }

    void CheckDeathCount()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            PhotonNetwork.CurrentRoom.Players[i].CustomProperties.TryGetValue("DeathCount", out var deathCount);
        }
    }
}