using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public float jumpPower;
    public float speed;
    
    float curSpeed;
    float h;

    public bool jumpAble;
    public bool jumping;

    KeyCode jumpKey = KeyCode.Space;
    KeyCode runKey = KeyCode.LeftShift;

    public AnimationClip[] animClip;
    public Animation anim;
    Rigidbody2D rbody;
    Animator animator;

    public LayerMask enemyLayer;
    public Vector2 attackOffset;
    public Vector2 attackSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();
    }

    void Start(){
        attackOffset = new Vector2(1.5f, -0.45f);
        attackSize = new Vector2(1.2f, 2f);
        jumpAble = true;
        jumping = false;
    }

    // Update is called once per frame
    void Update(){
        Move();
        Motion();
    }

    void Move(){
        h = Input.GetAxisRaw("Horizontal");
        if(Input.GetKey(runKey)){
            curSpeed = speed*1.5f;
        }else{
            curSpeed = speed;
        }if(Input.GetKeyDown(jumpKey) && jumpAble == true){
            jumpAble = false;
            jumping = true;
            rbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, 0, 0) * curSpeed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }

    void Motion(){
        if(h > 0){
            attackOffset = new Vector2(1.5f, -0.45f);
            transform.localScale = new Vector3(1, 1, 1);
        }else if(h < 0){
            attackOffset = new Vector2(-1.5f, -0.45f);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if(jumping == true){
            animator.Play("Player_Jump");
        }else if(h != 0){
            if(curSpeed>speed){
                animator.Play("Player_Run");
            }else{
                animator.Play("Player_Walk");
            }
        }else{
            animator.Play("Player_Idle");
        }
    }

    void Attack(){
        Vector2 attackPos = (Vector2)transform.position + attackOffset;
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos, attackSize, 0f, enemyLayer);

        /*foreach (var hit in hits)
        {
            Debug.Log("적 감지됨: " + hit.name);
            hit.GetComponent<Enemy>()?.TakeDamage(1);
        }*/
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Vector2 attackPos = (Vector2)transform.position + attackOffset;
        Gizmos.DrawWireCube(attackPos, attackSize);
    }


    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground")){
            jumping = false;
            jumpAble = true;
        }
    }
}