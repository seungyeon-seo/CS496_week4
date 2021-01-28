using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Button joinButton;
    public Text infoText;
    public Text playerName;
    public int type;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        infoText.text = "Connecting To Master Service...";
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
        joinButton.interactable = false;
    }

    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        infoText.text = "Online: Connected to Master Server";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
        infoText.text = $"Offline: Connection Disabled {cause.ToString()}";
    }

    public void Connect()
    {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            infoText.text = "Connecting to Random Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            infoText.text = "Offline: Connection Disabled - Try reconnecting...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        infoText.text = "There is no empty room, Creating new Room";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 10 });
    }

    public override void OnJoinedRoom()
    {
        infoText.text = "Connected with Room";
        switch (type)
        {
            case 0:
                PhotonNetwork.LoadLevel("Map2");
                break;
            case 1:
                PhotonNetwork.LoadLevel("Map3");
                break;
        }
    }
}
