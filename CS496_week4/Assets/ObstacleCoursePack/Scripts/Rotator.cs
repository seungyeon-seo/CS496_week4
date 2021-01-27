using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	public float speed = 3f;
    public int type;


    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case 0:
            default:
                transform.Rotate(0f, 0f, speed * Time.deltaTime / 0.01f, Space.Self);
                break;
            case 1:
                transform.Rotate(0f, 0f, -(speed * Time.deltaTime / 0.01f), Space.Self);
                break;
        }
	}
}
