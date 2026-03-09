using System;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class MultiplayerMainMenuManager : MonoBehaviourPunCallbacks
{
    private const string PLAYER_READY_KEY = "PlayerReady";
    [SerializeField] private GameObject panelLoading;
    [SerializeField] private GameObject panelWaitingInRoom;
    [SerializeField] private Button soloStartButton;
    [SerializeField] private Button multiplayerStartButton;
    [SerializeField] private Button playerReadyButton;
    [SerializeField] private TextMeshProUGUI usersText;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private TMP_Dropdown maxPlayersDropdown;
    private int maxPlayers;
    private bool isSoloPlay;

    private void Awake()
    {
        soloStartButton.onClick.AddListener(HandleSoloStartButtonClick);
        multiplayerStartButton.onClick.AddListener(HandleMultiplayerStartButtonClick);
        playerReadyButton.onClick.AddListener(HandlePlayerReadyButtonClick);
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void HandlePlayerReadyButtonClick()
    {
        var hashTable = new Hashtable
        {
            {
                PLAYER_READY_KEY, true
            }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);
    }
    
    private void HandleSoloStartButtonClick()
    {
        panelLoading.SetActive(true);
        PhotonNetwork.NickName = nicknameInput.text;
        PhotonNetwork.ConnectUsingSettings(); // connect to photon server, calls back to OnConnectedToMaster
        isSoloPlay = true;
    }

    private void HandleMultiplayerStartButtonClick()
    {
        if (!nicknameInput) return;
        panelLoading.SetActive(true);
        PhotonNetwork.NickName = nicknameInput.text;
        PhotonNetwork.ConnectUsingSettings(); // connect to photon server, calls back to OnConnectedToMaster
    }

    public override void OnConnectedToMaster()
    {
        if (isSoloPlay)
        {
            CreateNewRoom();
        }
        else
        {
            PhotonNetwork.JoinRandomRoom(); // calls back to OnJoinRandomFailed or OnJoinedRoom
        }
    }

    private void HandleJoinRoomFail()
    {
        CreateNewRoom();
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
        if (isSoloPlay)
        {
            StartGame();
            return;
        }
        
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey(PLAYER_READY_KEY)) return;

        var allUsersAreReady = CheckIfAllUsersAreReady();

        if (allUsersAreReady)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    private bool CheckIfAllUsersAreReady()
    {
        if (!PhotonNetwork.IsMasterClient) return false;
        if (PhotonNetwork.CurrentRoom.PlayerCount != maxPlayers) return false;

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            var isReady = player.Value.CustomProperties.ContainsKey(PLAYER_READY_KEY);
            if (!isReady) return false;
        }

        return true;
    }

    private void CreateNewRoom()
    {
        maxPlayers = isSoloPlay ? 1 : maxPlayersDropdown.value;
        PhotonNetwork.CreateRoom(null, new RoomOptions
        {
            MaxPlayers = maxPlayers,
        });
    }
}