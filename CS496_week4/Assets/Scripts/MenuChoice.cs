using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuChoice : MonoBehaviour
{
    public int menuOption;
    Vector3 initPos;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.tag == "Player" || hit.collider.tag == "Ground")
            {
                ChoiceMenu();
            }
        }

        if ((initPos.y-transform.position.y) > 2.0)
        {
            ChoiceMenu();
        }
    }

    void ChoiceMenu()
    {
        switch (menuOption)
        {
            case 0:
                SceneManager.LoadScene("LobbyScene");
                break;
            case 1:
                SceneManager.LoadScene("LobbyScene2");
                break;
            case 2:
                SceneManager.LoadScene("GameInfo");
                break;
            case 3:
                SceneManager.LoadScene("MenuScene");
                break;
            case 4:
                // GameObject.Find("Login_Manager").GetComponent<LoginManager>().Submit();
                break;
        }
    }
}
