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
    public GameObject startLinePrefab;
    public int playerNumber;

    int time;
    List<Vector3> checkPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;

        checkPoint = new List<Vector3>();
        if (PhotonNetwork.IsMasterClient)
            checkPoint.Add(new Vector3(-3f, 1.3f, -20f));
        else
            checkPoint.Add(new Vector3(-1f, 1.3f, -20f));
        checkPoint.Add(new Vector3(-0.52f, 16.1f, 106.49f));
        checkPoint.Add(new Vector3(32.27f, 30.125f, 158.89f));
        checkPoint.Add(new Vector3(128.06f, 30.13f, 157.5f));

        SpawnPlayer(0);
        
        switch (mapNumber)
        {
            case 2:
                time = 0;
                if (PhotonNetwork.IsMasterClient)
                {
                    SpawnStartLine();
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (mapNumber)
        {
            case 2:
                time += 1;
                if (time == 350 && PhotonNetwork.IsMasterClient)
                {
                    SpawnBall();
                }
                break;
        }
        
    }

    public void SpawnPlayer(int type)
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
                initPosition = checkPoint[type];
                rotation = new Quaternion(0, 5, 0, 0);
                break;

            case 3:
                if (!PhotonNetwork.IsMasterClient)
                {
                    // Ground
                    initPosition = new Vector3(0.09f, 1.23f, -0.09f);
                    rotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    // Ground (1)
                    initPosition = new Vector3(0.09f, 1.23f, 48.5f);
                    rotation = new Quaternion(0, 180, 0, 0);
                }
                break;
        }
        PhotonNetwork.Instantiate(playerPrefab.name, initPosition, rotation);
    }

    void SpawnBall()
    {
        time = 0; 
        Vector3 initPosition = new Vector3(Random.Range(-4, 3.8f), 17.34f, 51.89f);
        PhotonNetwork.Instantiate(ballPrefab.name, initPosition, new Quaternion(0, 0, 0, 0));
    }

    void SpawnStartLine()
    {
        Vector3 initPosition = new Vector3(-0.17f, 2.52f, -15.1f);
        PhotonNetwork.Instantiate(startLinePrefab.name, initPosition, new Quaternion(0, 0, 0, 0));
    }
}
