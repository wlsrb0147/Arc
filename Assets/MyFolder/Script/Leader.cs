using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyFolder.Script
{
    public class Leader : MonoBehaviour
    {
        [FormerlySerializedAs("test")] public float LengthOfGroundCheck;

        private readonly float _gravity = -39.24f;

        private void Awake()
        {
            _ani = GetComponentInChildren<Animator>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _rot = camPos.rotation.eulerAngles;
            var input = new RbV3
            {
                pos = transform.position,
                lookingPos = lookingPos.localPosition
            };
            for (var i = 0; i < _maxDelay[5]; i++) _inputs.Add(input);
        }

        private void FixedUpdate()
        {
            Initialize();
            InputCheck(); // 입력이 화살표인지, 클릭인지 확인 
            if (_currentInput == Mouse)
                MouseInput(); // 클릭 움직임 값 입력
            else if (_currentInput == Keyboard) KeyboardInput(); //wasd 방향키 입력받음

            ChildControl();
            GroundCheck(); // 땅 확인
            CharMove(); // 캐릭터 움직임 값 입력
            SetFacingPos(); // 보고있는 방향 인식
            FieldAniControl(); // 움직임에 대한 애니메이션 처리
            CamRot(); // Q,E 눌렀을 때 회전 처리
            CompassRot();
            if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeLayer();
            charaVel = _rb.velocity;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var angleWithUp = Vector3.Angle(collision.contacts[0].normal, Vector3.up);


            if (collision.gameObject.CompareTag("Telephote")) transform.position = new Vector3(-18f, 38.6f, 16f);
            
            
            if ( 48  <= angleWithUp)
            {
                wallContact = true;
                _wallCollisions.Add(collision.gameObject.name);
                _collisionNormal = collision.contacts[0].normal;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (_wallCollisions.Contains(collision.gameObject.name))
            {
                _wallCollisions.Remove(collision.gameObject.name);
                if (_wallCollisions.Count == 0) wallContact = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);

            _gizrot = -Vector2.SignedAngle(new Vector2(lookingPos.localPosition.x, lookingPos.localPosition.z),
                new Vector2(1, 0));

            // pos.position : 중심점, 중심점의 y축을 기준으로 gizrot만큼 회전, 스케일은 1,1,1 유지 
            var finalTransform = Matrix4x4.TRS(pos.position, Quaternion.Euler(0, _gizrot, 0), Vector3.one);

            // Gizmos의 매트릭스에 적용
            Gizmos.matrix = finalTransform;

            // 중심점에서 boxstate 사이즈로 생성
            Gizmos.DrawCube(Vector3.zero, boxstate);
        }


        // 경사면 벡터
        private void Jump()
        {
            if (onGround)
            {
                _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                onGround = false;
            }
        }

        private static Vector3 SlopeVec(Vector3 getNormalVec)
        {
            /*1. 법선벡터와 중력을 외적해서 평면 벡터 구함
        2. 평면벡터와 법선벡터를 외적하면 slope 벡터가 구해짐 */
            var faceVec = Vector3.Cross(getNormalVec, Vector3.up);
            var slopeVec = Vector3.Cross(getNormalVec, faceVec);
            slopeVec.Normalize();
            return slopeVec;
        }

        //정사영 벡터
        private static Vector3 TransVec(Vector3 vec1, Vector3 normal)
        {
            var result = Vector3.Dot(vec1, normal) / Vector3.Dot(normal, normal) * normal;

            return result;
        }

        private static Vector2 TransVec(Vector2 vec1, Vector2 normal)
        {
            var result = Vector3.Dot(vec1, normal) / Vector3.Dot(normal, normal) * normal;

            return result;
        }

        private void Initialize()
        {
            _standardVec = cameraPos.position - transform.position;
            _standardVec.y = 0;
        }

        private void InputCheck()
        {
            if (Input.GetMouseButton(0))
                _currentInput = Mouse;

            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
                                             || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)
                                             || Input.GetAxisRaw("Horizontal") != 0 ||
                                             Input.GetAxisRaw("Vertical") != 0)
                _currentInput = Keyboard;
        }

        private void MouseInput()
        {
            if (Input.GetMouseButton(0))
            {
                inputCheck = true;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    var clickedPosition = hit.point;
                    moveVec = clickedPosition - transform.position;
                    moveVec.y = 0;
                }

                if (Input.GetMouseButtonDown(1)) Jump();
            }
            else
            {
                inputCheck = false;
                moveVec = Vector3.zero;
            }
        }

        private void KeyboardInput()
        {
            var x = 0;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) x++;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) x--;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x += 2;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x -= 2;

            // 입력 유무 
            inputCheck = x != 0;

            //입력의 정도
            Vector3 moveInput = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) ||
                (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)))
                moveInput.z = 0;
            if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) ||
                (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)))
                moveInput.x = 0;

            var front = -_standardVec;
            front.y = 0;
            front = front.normalized;

            Vector3 right = new(front.z, 0, -front.x);

            moveVec = front * moveInput.z + right * moveInput.x;
        }

        private void ChildControl()
        {
            if (onGround &&
                child[0].onGround && child[1].onGround &&
                child[2].onGround && child[3].onGround &&
                child[4].onGround && !inputCheck) return;
            var input = new RbV3
            {
                pos = transform.position,
                lookingPos = lookingPos.localPosition
            };

            while (_inputs.Count > _maxDelay[5]) _inputs.RemoveAt(_inputs.Count - 1);

            for (var i = 0; i < 5; i++)
            {
                var push = _inputs[_maxDelay[i]];
                child[i].transform.position = push.pos;
                child[i].lookingPos.localPosition = push.lookingPos;
            }

            _inputs.Insert(0, input);
        }

        private void GroundCheck()
        {
            var halfExtents = boxstate * 0.5f; // BoxCast는 'half extents'를 사용하므로 크기를 반으로 나눕니다.
            var startPos = pos.position + Vector3.up * halfExtents.y;
            // 시작 위치, 방향, 회전, 크기를 Gizmos와 동일하게 설정


            var _mask = 1 << 3;
            _mask = ~_mask;
            var hit = Physics.BoxCast(startPos, halfExtents, Vector3.down, out var hitInfo,
                Quaternion.Euler(0, _gizrot, 0), boxstate.y - LengthOfGroundCheck, _mask);
            
            _planeNormal = hitInfo.normal;
            _slopVec = SlopeVec(_planeNormal);

            var angle = Vector3.Angle(_planeNormal, Vector3.up);

            if (hit)
            {
                if (angle < 48)
                {
                    onGround = true;
                    wallContact = false;
                    _rb.useGravity = false;
                }
                else
                {
                    onGround = false;
                    wallContact = transform;
                    _rb.useGravity = true;
                }
            }
            else
            {
                onGround = false;
                _rb.useGravity = true;
            }

            _onAir = !onGround;
            _ani.SetBool(Air, _onAir);
        }

        private void CharMove()
        {
            if (Input.GetKeyDown(KeyCode.Space)) Jump();

            moveVec = moveVec.normalized;

            _needleRot = moveVec; // 바늘 회전

            
            
            // 경사로를 움직이고 있을 때
            if (Vector3.Dot(moveVec, _slopVec) != 0 && onGround)
            {
                var downVec = TransVec(moveVec, _slopVec);
                var horizonVec = Vector3.Cross(_planeNormal, _slopVec);
                var horizonForceVec = TransVec(moveVec, horizonVec);

                moveVec = horizonForceVec + downVec;
                moveVec = moveVec.normalized;
            }
            else
            {
                moveVec.y = _rb.velocity.y / speed;
            }
            
            moveVec *= speed;

            if (wallContact)
                HandleWallContact();
            else
                _rb.velocity = moveVec;

            if (inputCheck == false && onGround && !Input.GetKeyDown(KeyCode.Space)) // 입력이 없고, 땅에 있으면서 스페이스바를 안눌렀을 때
                _rb.velocity = Vector3.zero;
            else if (inputCheck == false) //  && !wallContact 지움
                _rb.velocity = new Vector3(0, _rb.velocity.y, 0);

            if (_rb.velocity.y > 7.25f) _rb.velocity = new Vector3(_rb.velocity.x, 7.25f, _rb.velocity.z); // 하늘로 치솟기 방지
            if (_rb.velocity.y < -12) _rb.velocity = new Vector3(_rb.velocity.x, -12, _rb.velocity.z); // 낙하가속 땅뚫기 방지
        }

        private void HandleWallContact()
        {
            Vector2 move = new(moveVec.x, moveVec.z);
            Vector2 nor = new(_collisionNormal.x, _collisionNormal.z);

            if (Vector2.Dot(move, nor) == 0) // 벽이랑 같은방향으로 움직일 때
            {
                _rb.velocity = moveVec;
            }
            else
            {
                var realnormal = TransVec(move, nor);
                Vector3 dir = new(realnormal.x, 0, realnormal.y);

                if (Vector2.Dot(nor, realnormal) > 0) // 벽과 반대방향으로 움직일 때
                    _rb.velocity = moveVec;
                else // 벽 방향으로 움직일 때
                    _rb.velocity = moveVec - dir;
            }

            if (onGround || !(_rb.velocity.y < -3)) return;
            _rb.velocity = new Vector3(_rb.velocity.x, charaVel.y, _rb.velocity.z); // y값 버그 갱신

            var slopeDir = SlopeVec(_collisionNormal);
            var scaledVec = TransVec(new Vector3(0, _gravity, 0), slopeDir);
            if ((_rb.velocity + Time.fixedDeltaTime * scaledVec).y > -12)
            {
                _rb.velocity += Time.fixedDeltaTime * scaledVec;
            }
            else
            {
                var temp = _rb.velocity + Time.fixedDeltaTime * scaledVec;
                temp.y = -12;
                _rb.velocity = temp;
            }
        }

        private void SetFacingPos()
        {
            // 움직이는 방향에 Looking object 이동, 움직임이 없으면 위치 고정
            if (inputCheck) lookingPos.localPosition = new Vector3(moveVec.x, 0, moveVec.z).normalized;

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

        private void FieldAniControl()
        {
            // 현재 보고있는 방향 인식
            _ani.SetFloat(FacingX, _lookingVec.x);
            _ani.SetFloat(FacingZ, _lookingVec.z);

            // y축 속도 인식
            if (_rb.velocity.y > 2f) _ani.SetFloat(MoveY, -1);
            else if (_rb.velocity.y > 0) _ani.SetFloat(MoveY, 0);
            else if (_rb.velocity.y < 0) _ani.SetFloat(MoveY, 1);

            var rightleft = Vector3.Cross(_standardVec, lookingPos.localPosition);

            // 좌우 인식
            if (rightleft.y < 0.1f && !_facingRight)
            {
                _facingRight = !_facingRight;
                aiPos.localScale = new Vector3(-1, 1, 1);
            }
            else if (rightleft.y > 0.1f && _facingRight)
            {
                _facingRight = !_facingRight;
                aiPos.localScale = new Vector3(1, 1, 1);
            }

            // 인풋이 없고, moving이 true일때 갱신
            if (!inputCheck && _moving)
            {
                _moving = false;
                _ani.SetBool(Move, false);
            }
            // 입력이 있고, moving이 false일때 갱신
            else if (inputCheck && !_moving)
            {
                _moving = true;
                _ani.SetBool(Move, true);
            }
        }

        private void CamRot()
        {
            if (!Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
                _rot = new Vector3(_rot.x, _rot.y + mouseX, _rot.z);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                _rot = new Vector3(_rot.x, _rot.y + 45, _rot.z);
                _rotLerp = 0;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                _rot = new Vector3(_rot.x, _rot.y - 45, _rot.z);
                _rotLerp = 0;
            }


            if (_rotLerp < 1) _rotLerp += Time.fixedDeltaTime;


            camPos.rotation = Quaternion.Lerp(camPos.rotation, Quaternion.Euler(_rot), _rotLerp);
            cameraPos.LookAt(transform.position);
        }

        private void CompassRot()
        {
            var angle = Vector3.Angle(-_standardVec, _needleRot);
            var sign = Vector3.Cross(-_standardVec, _needleRot).y;

            if (sign > 0)
                sign = 1;
            else sign = -1;


            compass.rotation = Quaternion.Euler(0, 0, camPos.eulerAngles.y);
            if (inputCheck) needle.rotation = Quaternion.Euler(0, 0, angle * -sign);
        }

        private void ChangeLayer()
        {
            if (_ani.GetLayerWeight(1) == 1)
            {
                _ani.SetLayerWeight(1, 0);
                _ani.SetLayerWeight(2, 1);
            }
            else
            {
                _ani.SetLayerWeight(1, 1);
                _ani.SetLayerWeight(2, 0);
            }
        }

        #region 지렁이

        public Child[] child;
        private readonly List<RbV3> _inputs = new();
        private readonly int[] _maxDelay = { 3, 8, 13, 18, 23, 24 };

        private struct RbV3
        {
            public Vector3 pos;
            public Vector3 lookingPos;
        }

        #endregion

        #region 경사로

        private Vector3 _planeNormal;
        private Vector3 _slopVec;

        #endregion

        #region GroundCheck

        private float _gizrot;
        public float down = 0.204f;
        public float radius = 0.18f;

        private Vector3 _boxcurrent;
        public Vector3 boxstate = new(0.3f, 0.27f, 0.3f);

        #endregion

        #region WallCheck

        public bool wallContact;
        private Vector3 _collisionNormal;
        public bool onGround;
        private readonly HashSet<string> _wallCollisions = new();

        #endregion

        #region GameObject

        public Transform camPos;
        public Transform cameraPos;
        public Transform lookingPos;
        public Transform aiPos;
        public Transform pos;

        private Rigidbody _rb;
        private Animator _ani;

        #endregion

        #region Compass

        public Transform compass;
        public Transform needle;
        private Vector3 _needleRot;

        #endregion

        #region ani

        private bool _facingRight;
        private bool _currentInput;
        private const bool Keyboard = true;
        private const bool Mouse = false;
        private bool _moving;
        private Vector3 _standardVec;
        private Vector3 _lookingVec;
        private bool _onAir;

        #endregion

        #region rotation

        private Vector3 _rot;
        private float _rotLerp = 1;
        public float mouseSensitivity = 300f;

        #endregion

        #region speed

        public float jumpForce = 8f;
        public float speed = 5f;
        public Vector3 charaVel;

        #endregion

        #region inputs

        public static bool inputCheck;
        public Vector3 moveVec;
        private static readonly int FacingX = Animator.StringToHash("FacingX");
        private static readonly int FacingZ = Animator.StringToHash("FacingZ");
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int Air = Animator.StringToHash("OnAir");

        #endregion


        #region 새로만든 논리구조

        /*  1. 이니셜라이즈로 필요한 값 갱신
        2. 인풋체크로 현재 키보드입력인지, 마우스입력인지 확인
        3. 각각의 경우에 따른 논리구조    */

        #region 마우스 입력일경우 논리구조

        /*
    1. 마우스 클릭으로 스크립트 실행과 input ture/false 지정
    2. 마우스에서 raycast를 진행해 좌표를 받아옴
    3. 현재 위치에서 마우스방향으로 가는 벡터 moveVec 값 입력
    4. 좌클릭 중 우클릭시 점프 실행
     */

        #endregion

        #region 키보드 입력일경우 논리구조

        /*
    1. 입력유뮤 판단
    2. 키보드 입력값 input.getaxis를 moveInput으로 정의 , 가중치
    3. 동시입력시 좌우, 위아래 동시입력시 입력값 0 처리
    4. 카메라 위치의 반대방향을 front로, 앞과 우측 정의
    5. 움직임을 정의하는 moveVec 정의
     */

        #endregion

        /* 4. 인풋이 true일 때, lookingPos X,Z값 갱신, Y는 상시 갱신
          lookingVec과 standardVec의 각도 구함
          구한 각도로 애니메이션에 사용할 lookingVec 값 구함
       5. 스페이스바 입력시 점프 실행  */

        #region 6. 애니메이션 실행

        /*
    1. 애니메이션에 Facing X,Z에 lookingVec값 입력해서 방향 애니메이션 지정
    2. y축 속도 인식해서 점프 애니메이션 값 입력
    3. lookingVec의 외적으로 좌우 인식 후, scale.x 값을 1,-1 지정해줌.
    4. 좌우 인식해서 scale.x를 토글
     */

        #endregion

        //  7. Q,E로 카메라 회전

        #endregion
    }
}