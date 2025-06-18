using UnityEditor;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    Rigidbody2D rigid;
    public int jumpPower;
    public Boss boss;
    SpriteRenderer spriteRenderer;
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        BossJump();
    }

    void FixedUpdate()
    {

    }

    void BossJump()
    {
        if (boss.BossHp < 500)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
}
