using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    bool isFull;
    int count;

    // Start is called before the first frame update
    void Start()
    {
        isFull = false;
        count = 0; 
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            isFull = true;
        }
        if (isFull)
        {
            count++;
        }
        if (isFull && count >= 100)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
