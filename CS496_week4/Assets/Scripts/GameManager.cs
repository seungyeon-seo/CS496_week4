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
    public GameObject ballPrefab;
    public int playerNumber;

    int time;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += 1;
        if (ballPrefab != null && time == 200)
        {
            time = 0;
            SpawnBall();
        }
    }

    private void SpawnPlayer()
    {
        playerNumber = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Vector3 initPosition;
        Quaternion rotation;
        switch (mapNumber)
        {
            case 0:
            default:
                initPosition = new Vector3(-0.41f, 1.23f, -0.09f);
                rotation = new Quaternion(0, 0, 0, 0);
                break;
            case 2:
                initPosition = new Vector3(-3f, 1.3f, -15f);
                rotation = new Quaternion(0, 5, 0, 0);
                break;
        }

        PhotonNetwork.Instantiate(playerPrefab.name, initPosition, rotation);
    }

    private void FixedUpdate()
    {
        
    }

    void SpawnBall()
    {
        Vector3 initPosition = new Vector3(Random.Range(-4, 3.8f), 17.34f, 51.89f);
        PhotonNetwork.Instantiate(ballPrefab.name, initPosition, new Quaternion(0, 0, 0, 0));
    }
}
