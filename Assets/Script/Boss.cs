using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Rigidbody2D rigid;
    public int BossHp;
    public int Speed;
    public int jumpPower;
    public bool jumpAble;
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
        BossHp = 10000;
        jumpPower = 5;
        jumpAble = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        rigid.linearVelocity = new Vector2(Speed, rigid.linearVelocity.y);

        //플랫폼 체크
        Vector2 frontVec = new Vector2(rigid.position.x + Speed * 0.5f, rigid.position.y);
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

        if (Speed > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (Speed < 0)
        {
            spriteRenderer.flipX = true;
        }


        if (BossHp < 5000 && jumpAble == false)
        {
            StartCoroutine(BossJumpWithDelay(2f)); // 2초 대기 후 점프
        }

        if (jumpAble == true)
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


    void turn()
    {
        Speed *= -1;
        spriteRenderer.flipX = Speed == 1;

        CancelInvoke();
        Invoke("Think", 5);
    }

    void Think()
    {
        Speed = Random.Range(-2, 2);

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

    IEnumerator BossJumpWithDelay(float delay)
    {
        jumpAble = true;
    
        // 점프 실행
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

        // 점프 후 다시 점프하지 않도록 일정 시간 후 jumpAble을 false로
        yield return new WaitForSeconds(3f); // 예: 3초 후에 다시 점프 불가
        jumpAble = false;
    }
  
}