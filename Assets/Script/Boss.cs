using UnityEngine;

public class Boss : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    SpriteRenderer spriteRenderer;
    Animator anim;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Think();

        Invoke("Think", 5);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rigid.linearVelocity = new Vector2(nextMove, rigid.linearVelocityY);

        //플랫폼 체크
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove , rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 1, LayerMask.GetMask("Platform"));

        if (Physics.Raycast(transform.position, Vector3.forward, out rayHit, 5f)){
            if (rayHit.collider.CompareTag("Wall"))
            {
                turn();
                Debug.Log("Enemy에 맞음!");
            }
}
/*
        if(rayHit.collider == ){
            turn();
        }
        */

    }


    void turn(){
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think",  5);
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);



        anim.SetInteger("walkSpeed", nextMove);

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }
}
