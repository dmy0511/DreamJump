using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    float jumpForce = 650.0f;
    float walkSpeed = 5.0f;

    public float fallSpeed = 3f;
    
    bool DropBool = false;
    bool isFbColl = false;

    GameObject m_OverlapBlock = null; //연속 충돌 방지 변수

    [SerializeField] Animator transitionAnim;

    void Start()
    {
        //실행 프레임 속도 60프레임 고정
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        //프레임이 다른 컴퓨터일 경우 캐릭터 조작시 빠르게 움직일 수 있기 때문에 이를 방지하기 위한

        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        transform.localScale = new Vector3(0.165f, 0.165f, 1.0f);
    }

    void Update()
    {
        isFbColl = CheckIsFbColl();

        //점프
        if (Input.GetKeyDown(KeyCode.Space) && this.rigid2D.velocity.y == 0)
        {
            this.animator.SetTrigger("JumpTrigger");
            this.rigid2D.velocity = new Vector2(rigid2D.velocity.x, 0.0f);
            this.rigid2D.AddForce(transform.up * this.jumpForce);
        }

        //좌우 이동
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;

        //플레이어 속도
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        //캐릭터 이동
        rigid2D.velocity = new Vector2((key * walkSpeed), rigid2D.velocity.y);

        //움직이는 방향에 따라 이미지 반전
        if (key != 0)
        {
            transform.localScale = new Vector3(0.165f * key, 0.165f, 1.0f);
        }

        //플레이어의 속도에 맞춰 애니메이션 속도 변경
        if (this.rigid2D.velocity.y == 0)
        {
            this.animator.speed = speedx / 2.0f;
        }
        else
        {
            this.animator.speed = 1.0f;
        }

        if (transform.position.y < -20)
        {
            SceneManager.LoadScene("FirstScene");
        }

        //화면 밖으로 못 나가게
        Vector3 pos = transform.position;
        if (pos.x < -2.5f) pos.x = -2.5f;
        if (pos.x > 2.5f) pos.x = 2.5f;
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("window") == true)
        {
            StartCoroutine(normal());
            SceneManager.LoadScene("SecondScene");
        }
        else if (other.gameObject.name.Contains("thirdline") == true)
        {
            StartCoroutine(normal());
            SceneManager.LoadScene("ThirdScene");
        }
        else if (other.gameObject.name.Contains("forthline") == true)
        {
            StartCoroutine(normal());
            SceneManager.LoadScene("ForthScene");
        }
        else if (other.gameObject.name.Contains("obsPrefab") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                DropBool = true;

                this.animator.SetTrigger("Drop");

                StartCoroutine(FallAndTransition());

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("DeadlineRoot") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                DropBool = true;

                this.animator.SetTrigger("Drop");

                StartCoroutine(FallAndTransition());

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("can_1") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                GameMgr.m_CurScore += 50;

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("can_2") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                GameMgr.m_CurScore += 50;

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("can_3") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                GameMgr.m_CurScore -= 75;

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("chur_1") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                GameMgr.m_CurScore += 100;

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("chur_2") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                GameMgr.m_CurScore += 100;

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("chur_3") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                GameMgr.m_CurScore += 100;

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("milk") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                GameMgr.m_CurScore += 200;

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
    }

    bool CheckIsFbColl()
    {
        float a_CcSize = GetComponent<CircleCollider2D>().radius;
        Vector2 a_OffSet = GetComponent<CircleCollider2D>().offset;

        a_CcSize = a_CcSize * transform.localScale.y;

        Vector2 a_CcPos = Vector2.zero;
        a_CcPos.x = transform.position.x + a_OffSet.x;
        a_CcPos.y = transform.position.y + a_OffSet.y;

        Collider2D[] colls = Physics2D.OverlapCircleAll(a_CcPos, a_CcSize);

        foreach (Collider2D coll in colls)
        {
            if (coll.gameObject.name.Contains("FbColl") == true)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator FallAndTransition()
    {
        float fallDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < fallDuration)
        {
            this.rigid2D.velocity = new Vector2(this.rigid2D.velocity.x, -fallSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.rigid2D.velocity = Vector2.zero;
        Time.timeScale = 0;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        yield return new WaitForSecondsRealtime(animationLength);
        Time.timeScale = 1;

        SceneManager.LoadScene("OverScene");
    }

    IEnumerator normal()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        transitionAnim.SetTrigger("Start");
    }
}
