using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    float jumpForce = 650.0f;
    float walkSpeed = 5.0f;
    float m_Height = 0.0f;
    public float fallSpeed = 3f;

    bool DropBool = false;
    bool isFbColl = false;
    public GameObject pauseObject;
    GameObject m_OverlapBlock = null; //연속 충돌 방지 변수
    [SerializeField] Animator transitionAnim;

    bool isSpeedDebuffImmune = false; // can_2에 의한 이동 속도 감소 면역 상태 추적
    float speedDebuffImmuneTimer = 0f; // 면역 지속 시간 타이머

    Coroutine slowCoroutine = null; // 슬로우 상태 코루틴을 저장할 변수


    void Start()
    {
        //실행 프레임 속도 60프레임 고정
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        //프레임이 다른 컴퓨터일 경우 캐릭터 조작시 빠르게 움직일 수 있기 때문에 이를 방지하기 위한

        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        transform.localScale = new Vector3(0.165f, 0.165f, 1.0f);
        pauseObject.SetActive(false);
        m_Height = transform.position.y;

    }

    void Update()
    {
        isFbColl = CheckIsFbColl();

        if (Input.GetKeyDown(KeyCode.Space) && this.rigid2D.velocity.y == 0)
        {
            this.animator.SetTrigger("JumpTrigger");
            this.rigid2D.velocity = new Vector2(rigid2D.velocity.x, 0.0f);
            this.rigid2D.AddForce(transform.up * this.jumpForce);
        }

        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;

        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        rigid2D.velocity = new Vector2((key * walkSpeed), rigid2D.velocity.y);

        if (key != 0)
        {
            transform.localScale = new Vector3(0.165f * key, 0.165f, 1.0f);
        }

        if (this.rigid2D.velocity.y == 0)
        {
            this.animator.speed = speedx / 2.0f;
        }
        else
        {
            this.animator.speed = 1.0f;

            float currentHeight = transform.position.y;
            if (currentHeight > m_Height)
            {
                GameMgr.m_CurScore += 1f;
            }
            else if (currentHeight < m_Height)
            {
                GameMgr.m_CurScore -= 1f;
            }
            m_Height = currentHeight;
        }

        if (transform.position.y < -20)
        {
            SceneManager.LoadScene("FirstScene");
        }

        Vector3 pos = transform.position;
        if (pos.x < -2.5f) pos.x = -2.5f;
        if (pos.x > 2.5f) pos.x = 2.5f;
        transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isPaused = !pauseObject.activeSelf;
            pauseObject.SetActive(isPaused);
            Time.timeScale = isPaused ? 0.0f : 1.0f;
        }

        if (isSpeedDebuffImmune)
        {
            speedDebuffImmuneTimer -= Time.deltaTime;
            if (speedDebuffImmuneTimer <= 0f)
            {
                isSpeedDebuffImmune = false; // 면역 상태 비활성화
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("window") == true)
        {
            GameMgr.m_CurScore += 250;
            StartCoroutine(normal());
            SceneManager.LoadScene("SecondScene");
        }
        else if (other.gameObject.name.Contains("thirdline") == true)
        {
            GameMgr.m_CurScore += 500;
            StartCoroutine(normal());
            SceneManager.LoadScene("ThirdScene");
        }
        else if (other.gameObject.name.Contains("forthline") == true)
        {
            GameMgr.m_CurScore += 1000;
            StartCoroutine(normal());
            SceneManager.LoadScene("ForthScene");
        }
        else if (other.gameObject.name.Contains("obsPrefab") == true)
        {
            if (SceneManager.GetActiveScene().name == "FirstScene")
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
            else
            {
                DeadlineAudio();

                MusicOff();

                if (m_OverlapBlock != other.gameObject)
                {
                    DropBool = true;

                    this.animator.SetTrigger("Drop");

                    StartCoroutine(FallAndTransition());

                    m_OverlapBlock = other.gameObject;
                }

                Destroy(other.gameObject);
            }
        }
        else if (other.gameObject.name.Contains("DeadlineRoot") == true)
        {
            DeadlineAudio();

            MusicOff();

            if (m_OverlapBlock != other.gameObject)
            {
                DropBool = true;

                this.animator.SetTrigger("Drop");

                StartCoroutine(FallAndTransition());

                m_OverlapBlock = other.gameObject;
            }
        }
        else if (other.gameObject.name.Contains("can_1") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                GameMgr.m_CurScore += 25;

                isSpeedDebuffImmune = true; // 면역 상태 활성화
                speedDebuffImmuneTimer = 2f;

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("can_2") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                GameMgr.m_CurScore += 25;
                StartCoroutine(IncreasePlayerSpeedOverlapping(6.0f, 1.0f));

                m_OverlapBlock = other.gameObject;
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("can_3") == true)
        {
            if (m_OverlapBlock != other.gameObject)
            {
                // can_3 아이템과 충돌했을 때 플레이어 점수를 75점 감소
                GameMgr.m_CurScore -= 75;

                // 이동 속도 감소 면역 상태가 아닐 경우
                if (!isSpeedDebuffImmune)
                {
                    // 슬로우 상태 코루틴이 이미 실행 중이면 중지
                    if (slowCoroutine != null)
                    {
                        StopCoroutine(slowCoroutine);
                    }
                    // 플레이어의 이동 속도를 3.0f로 제한하는 코루틴을 실행. 지속 시간은 1.5초.
                    slowCoroutine = StartCoroutine(LimitPlayerSpeedOverlapping(3.0f, 1.5f));
                }
                else // 면역 상태일 경우
                {
                    // 슬로우 상태 코루틴을 중지하고, 이동 속도를 원래 값으로
                    if (slowCoroutine != null)
                    {
                        StopCoroutine(slowCoroutine);
                        slowCoroutine = null;
                    }
                    walkSpeed = 5;
                }

                // 중복 충돌 방지를 위해 현재 충돌한 게임 오브젝트를 저장
                m_OverlapBlock = other.gameObject;
            }

            // can_3 아이템을 제거합니다.
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

    void MusicOff()
    {
        GameObject music = GameObject.Find("SoundMgr");

        music.GetComponent<AudioSource>().Stop();
    }

    void DeadlineAudio()
    {
        GameObject deadlineRoot = GameObject.Find("DeadlineRoot");

        deadlineRoot.GetComponent<AudioSource>().Play();
    }

    private IEnumerator FallAndTransition()
    {
        float fallDuration = 0.5f;
        float elapsedTime = 0f;

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

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

        foreach (var col in colliders)
        {
            col.enabled = true;
        }

        SceneManager.LoadScene("OverScene");
    }

    IEnumerator IncreasePlayerSpeedOverlapping(float newSpeed, float duration)
    {
        float originalSpeed = walkSpeed;
        walkSpeed = newSpeed;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        walkSpeed = originalSpeed;
    }

    IEnumerator LimitPlayerSpeedOverlapping(float newSpeed, float duration)
    {
        // 플레이어의 원래 걷는 속도를 저장
        float originalSpeed = walkSpeed;

        // 플레이어의 걷는 속도를 새로운 지정된 속도로 설정
        walkSpeed = newSpeed;

        // 경과 시간을 초기화하여 지속 시간을 추적
        float elapsedTime = 0f;

        // 지정된 지속 시간이 지날 때까지 반복
        while (elapsedTime < duration)
        {
            // 지난 프레임 이후 경과 시간을 증가
            elapsedTime += Time.deltaTime;

            // 여기서 실행을 중단하고 다음 프레임에서 계속
            yield return null;
        }

        // 플레이어의 이동 속도를 원래 속도로 복원
        walkSpeed = originalSpeed;
    }

    IEnumerator normal()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        transitionAnim.SetTrigger("End");

        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        transitionAnim.SetTrigger("Start");

        foreach (var col in colliders)
        {
            col.enabled = true;
        }
    }
}
