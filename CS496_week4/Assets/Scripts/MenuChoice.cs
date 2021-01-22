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
        Debug.Log("init pos is " + initPos.y);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.Log($"hit with {hit.collider.name}");
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
                SceneManager.LoadScene("SampleScene");
                break;
            case 1:
                Debug.Log("LoginScene");
                break;
            case 2:
                Debug.Log("GameInfoScene");
                break;
        }
    }
}
