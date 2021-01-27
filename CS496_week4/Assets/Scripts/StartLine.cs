using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    bool isFull;
    int count;
    TextMesh startText;

    // Start is called before the first frame update
    void Start()
    {
        isFull = false;
        count = 0;
        startText = GameObject.Find("startText").GetComponent<TextMesh>();
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.PlayerList.Length >= 1)
        {
            isFull = true;
        }
        if (isFull)
        {
            count++;
            if (count == 1)
            {
                startText.text = "3";
            } else if (count >= 50 && count < 100)
            {
                startText.text = "2";
            } else if (count >= 100 && count < 150)
            {
                startText.text = "1";
            } else if (count >= 150)
            {
                startText.text = null;
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
