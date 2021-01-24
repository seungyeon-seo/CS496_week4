using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{   public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    public static GameManager instance;
    public int mapNumber;

    public GameObject playerPrefab;
    public int playerNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnPlayer()
    {
        playerNumber = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Vector3 initPosition;
        switch (mapNumber)
        {
            case 0:
            default:
                initPosition = new Vector3(-0.41f, 1.23f, -0.09f);
                break;
            case 2:
                initPosition = new Vector3(15f, 0.75f, -0.19f);
                break;
        }
        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        PhotonNetwork.Instantiate(playerPrefab.name, initPosition, rotation);
    }
}
