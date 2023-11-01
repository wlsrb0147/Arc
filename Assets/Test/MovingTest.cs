using UnityEngine;

public class MovingTest : MonoBehaviour
{
    public float speed = 10;

    public float jumpForce;


    private bool facingRight;
    private bool moving;

    private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rotVec = cam.position - transform.position;
        rotVec = SetYzero(rotVec);
    }

    private void FixedUpdate()
    {
        ArrowInput(); //wasd ����Ű �Է¹���
        CharMove(); // ĳ���� ������ ó����
        FieldAniTrigger(); // �����ӿ� ���� �ִϸ��̼� ó��
        CamRot(); // Q,E ������ �� ȸ�� ó��

        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeLayer();
    }

    private void ArrowInput()
    {
        // �Է� ���� 
        var xInput = Input.GetAxisRaw("Horizontal");
        var zInput = Input.GetAxisRaw("Vertical");

        arrowInputCheck = new Vector3(xInput, 0, zInput);

        // �Է��� ����
        var moveX = Input.GetAxis("Horizontal");
        var moveZ = Input.GetAxis("Vertical");


        move = new Vector3(moveX, 0, moveZ);

        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
        {
            move.z = 0;
            arrowInputCheck.z = 0;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            move.x = 0;
            arrowInputCheck.x = 0;
        }
    }

    private void CharMove()
    {
        Vector3 front;
        Vector3 right;

        if (Input.GetKeyDown(KeyCode.Space)) rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        if (rb.velocity.y < -8) rb.velocity = new Vector3(rb.velocity.x, -8, rb.velocity.z);


        if (move.magnitude > 1f) move = move.normalized;

        front = transform.position - cam.position;
        front = SetYzero(front);

        front = front.normalized;

        right = new Vector3(front.z, 0, -front.x);

        var moveVec = front * move.z + right * move.x;
        moveVec = new Vector3(moveVec.x * speed, rb.velocity.y, moveVec.z * speed);
        ;

        if (arrowInputCheck == Vector3.zero) rb.velocity = new Vector3(0, rb.velocity.y, 0);
        else rb.velocity = moveVec;
    }

    private void ChangeLayer()
    {
        if (ani.GetLayerWeight(1) == 1)
        {
            ani.SetLayerWeight(1, 0);
            ani.SetLayerWeight(2, 1);
        }
        else
        {
            ani.SetLayerWeight(1, 1);
            ani.SetLayerWeight(2, 0);
        }
    }

    private void FieldAniTrigger()
    {
        float xStop;
        float zStop;

        // ���� �ν�
        ani.SetFloat("AxisX", Mathf.Abs(move.x));
        ani.SetFloat("AxisZ", move.z);

        // �¿� �ν�
        if (move.x > 0 && !facingRight)
        {
            facingRight = !facingRight;
            spr.localScale = new Vector3(-1, 1, 1);
        }
        else if (move.x < 0 && facingRight)
        {
            facingRight = !facingRight;
            spr.localScale = new Vector3(1, 1, 1);
        }


        if (arrowInputCheck.x == 0 && arrowInputCheck.z == 0 && moving) // ��ǲ�� ����, moving�� true�϶� ����
        {
            if (Mathf.Abs(move.x) == 0)
                xStop = 0;
            else
                xStop = 1f;

            if (Mathf.Abs(move.z) == 0)
                zStop = 0;
            else
                zStop = Mathf.Sign(move.z);

            ani.SetFloat("MoveX", xStop);
            ani.SetFloat("MoveZ", zStop);

            moving = false;
            ani.SetBool("Move", false);
        }
        else if ((arrowInputCheck.x != 0 || arrowInputCheck.z != 0) && !moving) // �Է��� �ְ�, moving�� false�϶� ����
        {
            moving = true;
            ani.SetBool("Move", true);
        }
    }

    private void CamRot()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rotVec = RotateQuarter(0);

            rotLerp = 0;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            rotVec = RotateQuarter(1);

            rotLerp = 0;
        }


        if (rotLerp < 1)
            // ���⿡ ȸ���ֱ�
            // cam.rotation = rotAngle;
            rotLerp += Time.deltaTime;

        startVec = cam.position - transform.position;
        startVec = SetYzero(startVec);


        cam.LookAt(transform.position);
        cam.position = transform.position + Vector3.Slerp(startVec, rotVec, rotLerp);
        cam.position = new Vector3(cam.position.x, transform.position.y + 4.5f, cam.position.z);
    }


    private Vector3 RotateQuarter(int x)
    {
        Vector3 result;
        var vec = rotVec;

        if (x == 0)
            result = new Vector3(
                vec.x * Mathf.Cos(45 * Mathf.Deg2Rad) + vec.z * Mathf.Sin(45 * Mathf.Deg2Rad),
                vec.y,
                -vec.x * Mathf.Sin(45 * Mathf.Deg2Rad) + vec.z * Mathf.Cos(45 * Mathf.Deg2Rad));

        else if (x == 1)
            result = new Vector3(
                vec.x * Mathf.Cos(45 * Mathf.Deg2Rad) - vec.z * Mathf.Sin(45 * Mathf.Deg2Rad),
                vec.y,
                vec.x * Mathf.Cos(45 * Mathf.Deg2Rad) + vec.z * Mathf.Sin(45 * Mathf.Deg2Rad));
        else
            result = cam.position;

        result = SetYzero(result);
        return result;
    }

    private Vector3 SetYzero(Vector3 vec)
    {
        vec = new Vector3(vec.x, 0, vec.z);
        return vec;
    }

    #region define

    public Transform cam;
    public Transform spr;

    private Rigidbody rb;
    private Animator ani;

    #endregion

    #region rotation

    private float rotLerp = 1;
    private Vector3 startVec;
    private Vector3 rotVec;

    #endregion

    #region inputs

    public Vector3 arrowInputCheck; // �Է� ����
    public Vector3 move; // �Է� ����ġ

    #endregion
}