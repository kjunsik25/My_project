using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Rigidbody2D rigid;
    public int BossHp;  //보스 체력
    public int BossDmg;  //보스 기본 공격력
    public int Speed;  //보스 이동속도
    public int jumpPower;  //보스 점프 패턴 점프력
    public int BossJumpDmg;  //보스 점프 패턴 데미지
    public bool jumpAble;  //보스 점프 가능 여부
    SpriteRenderer spriteRenderer;
    Animator anim;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Think();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(DelayTime());
        BossDmg = 2;
        BossHp = 10000;
        jumpPower = 0;
        BossJumpDmg = 10;
        jumpAble = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 보스 점프 패턴 모션 정리
        if (jumpAble == true && rigid.linearVelocity.normalized.y > 0)
        {
            anim.SetBool("isJump", true);
        }

        if (rigid.linearVelocity.normalized.y < 0)
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", true);
        }
        else
            anim.SetBool("isFall", false);
    }

    void FixedUpdate()
    {
        //보스 이동
        rigid.linearVelocity = new Vector2(Speed, rigid.linearVelocity.y);

        //플랫폼 체크
        Vector2 frontVec = new Vector2(rigid.position.x + Speed * 0.4f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector2.down * 2f, Color.red);

        RaycastHit2D groundHit = Physics2D.Raycast(frontVec, Vector2.down, 2f, LayerMask.GetMask("Platform"));

        if (groundHit.collider == null && jumpAble != true)
        {
            turn();
            Debug.Log("플랫폼 없음 방향 전환");
        }

        //벽 체크
        Vector2 wallCheckDir = new Vector2(Speed, 0);
        Debug.DrawRay(frontVec, wallCheckDir * 1f, Color.blue);

        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, wallCheckDir, 1f, LayerMask.GetMask("Wall"));

        //가는 방향에 맞게 방향전환
        if (wallHit.collider != null && wallHit.collider.CompareTag("Wall"))
        {
            turn();
            Debug.Log("벽 감지됨");
        }

        //보스 이동 방향 정리
        if (Speed > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (Speed < 0)
        {
            spriteRenderer.flipX = true;
        }

        //보스 점프 패턴
        if (BossHp < 5000 && jumpAble == false && Speed == 0)
        {
            jumpPower = 10;
            StartCoroutine(BossJumpWithDelay(1f)); // 2초 대기 후 점프
            jumpAble = true;
            jumpPower = 0;
        }


    }


    //보스 방향 정리
    void turn()
    {
        Speed *= -1;
        spriteRenderer.flipX = Speed == 1;

        CancelInvoke();
        Invoke("Think", 5);
    }

    //보스 이동 정리
    void Think()
    {
        if (BossHp < 5000 && jumpPower > 0)
        {
            Speed = 0;
        }
        else
        {
            Speed = Random.Range(-1, 1);   
        }
    
        Speed = Speed * 2;

        anim.SetInteger("walkSpeed", Speed);

        
        if (Speed == 0)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWaliking", true);
        }

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    //보스 점프 패턴 딜레이 정리
    IEnumerator BossJumpWithDelay(float delay)
    {


        // 점프 실행
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

        jumpAble = true;

        // 점프 후 다시 점프하지 않도록 일정 시간 후 jumpAble을 false로
        yield return new WaitForSeconds(10f); // 예: 10초 후에 다시 점프 불가
        jumpAble = false;
    }
  
}