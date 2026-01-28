using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class MultiplayerMainMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject panelLoading;
    [SerializeField] private GameObject panelWaitingInRoom;
    [SerializeField] private Button multiplayerStartButton;
    [SerializeField] private Button playerReadyButton;
    [SerializeField] private TextMeshProUGUI usersText;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private int maxPlayers;

    private void Awake()
    {
        multiplayerStartButton.onClick.AddListener(HandleMultiplayerStartButtonClick);
        playerReadyButton.onClick.AddListener(HandlePlayerReadyButtonClick);
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void HandlePlayerReadyButtonClick()
    {
        Debug.Log("Player ready");
    }

    private void HandleMultiplayerStartButtonClick()
    {
        panelLoading.SetActive(true);
        PhotonNetwork.NickName = nicknameInput.text;
        PhotonNetwork.ConnectUsingSettings(); // connect to photon server, calls back to OnConnectedToMaster
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom(); // calls back to OnJoinRandomFailed or OnJoinedRoom
    }

    private void HandleJoinRoomFail()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions
        {
            MaxPlayers = maxPlayers,
        });
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        HandleJoinRoomFail();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        HandleJoinRoomFail();
    }

    public void PaintAllCurrentPlayers()
    {
        usersText.text = "";
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            usersText.text += player.Value.NickName + "\n\n";
        }
    }

    public override void OnJoinedRoom()
    {
        panelWaitingInRoom.SetActive(true);
        PaintAllCurrentPlayers();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        usersText.text += newPlayer.NickName + "\n\n";
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PaintAllCurrentPlayers();
    }
}