using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraTest : MonoBehaviour
{
    Vector3 camRot;
    Vector3 camPos;
    public GameObject cam;
    public Transform character;

    private void Start()
    {
        camRot = transform.rotation.eulerAngles;
        camPos = new Vector3(character.position.x,character.position.y+10,character.position.z-3 );
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(camRot);
        transform.position = new Vector3(character.position.x, character.position.y + 10, character.position.z - 3);
    }

    void TurnLeft()
    {
    }

    void TurnRight()
    {

    }
}
