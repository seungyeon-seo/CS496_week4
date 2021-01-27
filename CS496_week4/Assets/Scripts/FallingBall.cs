using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (transform.position.y < 1.5f)
        {
            PhotonNetwork.Destroy(gameObject);
            // gameObject.SetActive(false);
        }
    }
}
