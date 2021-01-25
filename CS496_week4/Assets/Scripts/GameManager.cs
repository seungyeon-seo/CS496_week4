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
        switch (mapNumber)
        {
            case 2:
                time += 1;
                if (time == 200)
                {
                    SpawnBall();
                }
                checkRespawn();
                break;
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

    void checkRespawn()
    {
        GameObject player = GameObject.Find(playerPrefab.name+"(Clone)");
        if (player == null)
            return;
        Vector3 pos = player.transform.position;
        if (pos.x < -6.95 || pos.x > 6.67 )
        {
            RespawnPlayer(player);
        }
        else if (pos.y < -2.19)
        {
            RespawnPlayer(player);
        }
    }

    void RespawnPlayer(GameObject player)
    {
        player.SetActive(false);
        SpawnPlayer();
    }

    void SpawnBall()
    {
        time = 0; 
        Vector3 initPosition = new Vector3(Random.Range(-4, 3.8f), 17.34f, 51.89f);
        PhotonNetwork.Instantiate(ballPrefab.name, initPosition, new Quaternion(0, 0, 0, 0));
    }
}
