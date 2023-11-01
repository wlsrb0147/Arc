using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public GameObject cam;
    public Transform character;
    private Vector3 camPos;
    private Vector3 camRot;

    private void Start()
    {
        camRot = transform.rotation.eulerAngles;
        camPos = new Vector3(character.position.x, character.position.y + 10, character.position.z - 3);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.rotation = Quaternion.Euler(camRot);
        transform.position = new Vector3(character.position.x, character.position.y + 10, character.position.z - 3);
    }

    private void TurnLeft()
    {
    }

    private void TurnRight()
    {
    }
}