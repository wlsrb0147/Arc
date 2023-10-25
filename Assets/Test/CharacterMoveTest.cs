using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveTest : MonoBehaviour
{
    float xmove;
    float zmove;
    float front;
  //  float moveVelocity = 1;
    Vector3 rotVec;

    public float jumpPower = 20;

    float rot;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        front = Input.GetAxis("Vertical");
        rot += Input.GetAxis("Horizontal");

        rb.rotation = Quaternion.Euler(0, rot, 0);
        xmove = Mathf.Sin(rot * Mathf.Deg2Rad);
        zmove = Mathf.Cos(rot * Mathf.Deg2Rad);
        //    rotVec = new Vector3 (xmove, 0 ,zmove);
        rb.velocity = new Vector3(xmove * front, rb.velocity.y, zmove * front);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode.Impulse);
        }

    }
}


