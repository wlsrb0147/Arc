using UnityEngine;

public class Child : MonoBehaviour
{
    private static readonly int FacingX = Animator.StringToHash("FacingX");
    private static readonly int FacingZ = Animator.StringToHash("FacingZ");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int Air = Animator.StringToHash("OnAir");
    public Transform parentCam;
    public Transform myCam;
    public Transform myPos;

    public Transform lookingPos;
    public Transform pos;
    public Transform cameraPos;
    private readonly float offset = -0.12f;
    private Animator _ani;

    private Vector3 _lookingVec;
    // Update is called once per frame

    private Rigidbody _rb;
    private Vector3 _standardVec;

    private float ypos;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _ani = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        ypos = transform.position.y;
    }

    private void FixedUpdate()
    {
        myCam.rotation = parentCam.rotation;

        Initialize();
        SetFacingPos();
        if (_ani.runtimeAnimatorController != null)
        {
            AniControl();
            GroundCheck();
        }
        else
        {
            onGround = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        _gizrot = -Vector2.SignedAngle(new Vector2(lookingPos.localPosition.x, lookingPos.localPosition.z),
            new Vector2(1, 0));

        // pos.position : 중심점, 중심점의 y축을 기준으로 gizrot만큼 회전, 스케일은 1,1,1 유지 
        var finalTransform = Matrix4x4.TRS(pos.position, Quaternion.Euler(0, _gizrot, 0), Vector3.one);

        // Gizmos의 매트릭스에 적용
        Gizmos.matrix = finalTransform;

        // 중심점에서 boxstate 사이즈로 생성
        Gizmos.DrawCube(Vector3.zero, boxstate);
    }

    private void Initialize()
    {
        _standardVec = cameraPos.position - transform.position;
        _standardVec.y = 0;
    }

    private void GroundCheck()
    {
        var halfExtents = boxstate * 0.5f; // BoxCast는 'half extents'를 사용하므로 크기를 반으로 나눕니다.
        var startPos = pos.position + Vector3.up * halfExtents.y;
        // 시작 위치, 방향, 회전, 크기를 Gizmos와 동일하게 설정


        var mask = 1 << 3;
        mask = ~mask;
        var hit = Physics.BoxCast(startPos, halfExtents, Vector3.down, out var hitInfo,
            Quaternion.Euler(0, _gizrot, 0), boxstate.y + offset, mask);


        _planeNormal = hitInfo.normal;

        var angle = Vector3.Angle(_planeNormal, Vector3.up);

        if (hit)
        {
            if (angle < 65)
                onGround = true;
            else
                onGround = false;
        }
        else
        {
            onGround = false;
        }

        _onAir = !onGround;
        _ani.SetBool(Air, _onAir);
    }

    private void AniControl()
    {
        if (Vector3.Dot(myPos.right, lookingPos.localPosition) > 0)
            myPos.localScale = new Vector3(-1, 1, 1);
        else
            myPos.localScale = new Vector3(1, 1, 1);


        if (transform.position.x != movingTest.x || (transform.position.z != movingTest.z && onGround))
        {
            _ani.SetBool(Move, true);
            movingTest = transform.position;
        }
        else
        {
            _ani.SetBool(Move, false);
        }

        _ani.SetFloat(FacingX, _lookingVec.x);
        _ani.SetFloat(FacingZ, _lookingVec.z);


        var yChange = ypos - transform.position.y;

        if (yChange < 0) _ani.SetFloat(MoveY, -1);
        else if (yChange > 0) _ani.SetFloat(MoveY, 1);
        ypos = transform.position.y;
    }

    private void SetFacingPos()
    {
        // 움직이는 방향에 Looking object 이동, 움직임이 없으면 위치 고정

        var lookingAngle = Vector3.Angle(_standardVec, lookingPos.localPosition);

        if (lookingAngle < 22.5f)
            _lookingVec = new Vector3(0, 0, -1);
        else if (lookingAngle < 67.5f)
            _lookingVec = new Vector3(1, 0, -1);
        else if (lookingAngle < 112.5f)
            _lookingVec = new Vector3(1, 0, 0);
        else if (lookingAngle < 157.5f)
            _lookingVec = new Vector3(1, 0, 1);
        else
            _lookingVec = new Vector3(0, 0, 1);
    }

    #region GroundCheck

    private float _gizrot;
    public float down = 0.204f;
    public float radius = 0.18f;

    private Vector3 _boxcurrent;
    public Vector3 boxstate = new(0.3f, 0.27f, 0.3f);
    public bool onGround;

    private Vector3 _planeNormal;
    private bool _onAir;

    private Vector3 movingTest;

    #endregion
}