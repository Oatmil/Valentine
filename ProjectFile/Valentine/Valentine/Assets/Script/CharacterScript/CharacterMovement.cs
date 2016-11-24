using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

    Rigidbody2D rigid2D;
    public Vector3 JumpForce;
    //Slamming
    public Vector3 SlamForce;
    public Vector3 v_SlamKnockBack;

    public float m_MoveSpeed;
    float m_TempMove = 0;
    public float m_WallSlideGravity;
    
    [SerializeField]
    private float m_Gravity;

    //all the bool
    [SerializeField]
    private bool b_Jump = false;
    [SerializeField]
    private bool b_Ground = false;
    [SerializeField]
    private bool b_Left = false;
    [SerializeField]
    private bool b_Right = true;
    [SerializeField]
    private bool b_TouchWall = true;
    public bool b_Stun = false;

    public float t_StunTime = 0;
    [SerializeField]
    private float XPos = 0;
    private int XScl = 1;
    public GameObject m_HurtBox;
   
    //Score Setting and Text Setting
    public GameObject m_PlayerText;
    private Text t_uiTxt;

    //Setting the KeyInput
    public string s_KeyInput;
    
    // Stats monitor
    public int i_Health;
    public int i_Damage;
    public float i_SpeedMod;
    public float i_JumpMod;

    Animator anim;
    Vector3 LclScalling;

    void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        m_Gravity = rigid2D.gravityScale;
        t_uiTxt = m_PlayerText.GetComponent<Text>();
        LclScalling = transform.localScale;
    }

    void Start()
    {
        m_HurtBox.SetActive(false);
        m_TempMove = m_MoveSpeed;
    }

    void Update()
    {
        FlipImage();
        if (b_Stun)
            Stun();
        else
        {
            if (Input.GetKeyDown(s_KeyInput))
            {
                if (b_Jump)
                    Jumping();
                else if (!b_Jump && !b_Ground)
                    Slaming();
            }

            if (b_Ground)
            {
                m_HurtBox.SetActive(false);
            }
            SetScoreText();
            TouchWallUpdate(); // touch wall update that need to be updated
        }

        AnimationUpdate();
    }

    void FlipImage()
    {
        if (b_Left)
            XScl = 1;
        else if(b_Right)
            XScl = -1;

        transform.localScale = new Vector3(LclScalling.x * XScl, LclScalling.y, LclScalling.z);
    }

    public void Mods()
    {
        JumpForce = new Vector3(JumpForce.x * i_JumpMod, JumpForce.y * i_JumpMod, JumpForce.z);
        m_MoveSpeed = m_MoveSpeed * i_SpeedMod;
        anim.SetTrigger("Eat");
    }

    void TouchWallUpdate()
    {
        if (b_Right && b_Ground)
        {
            XPos = + m_MoveSpeed * Time.deltaTime;
        }
        else if (b_Left && b_Ground)
        {
            XPos = - m_MoveSpeed * Time.deltaTime;
        }
        else if (b_Right && !b_Ground && b_TouchWall)
        {
            b_Jump = true;
            XPos = 0;
        }
        else if (b_Left && !b_Ground && b_TouchWall)
        {
            b_Jump = true;
            XPos = 0;
        }
        transform.position += new Vector3(XPos, 0.0f, 0.0f);
    } // Checking Of Jump on wall
   
    void Jumping() // setting Jump()
    {
        b_Jump = false;
        anim.SetTrigger("Jump");
        ResetGravity();
        XPos = 0;
        b_Ground = false;
        rigid2D.velocity = Vector3.zero;
        if (b_Right)
            rigid2D.AddForce(new Vector2(JumpForce.x, JumpForce.y), ForceMode2D.Impulse);
        else if (b_Left)
             rigid2D.AddForce(new Vector2(-JumpForce.x, JumpForce.y), ForceMode2D.Impulse);

    }

    void Slaming()
    {
        if(i_Damage >0)
            m_HurtBox.SetActive(true);
        XPos = 0;
        ResetGravity();
        rigid2D.velocity = Vector3.zero;
        rigid2D.AddForce(new Vector2(SlamForce.x, SlamForce.y), ForceMode2D.Impulse);
        anim.SetTrigger("Slam");
    } // Slam fuinction

    void OnCollisionEnter2D(Collision2D col) // chaning variable on collision
    {
        if (col.gameObject.tag == "Ground")
        {
            b_Jump = true;
            ResetGravity();
            b_Ground = true;
        }
        if (col.gameObject.tag == "LeftWall")
        {
            rigid2D.gravityScale = m_WallSlideGravity;
            rigid2D.velocity = Vector2.zero;
            b_TouchWall = true;
            if (b_Left)
            {
                b_Left = false;
                b_Right = true;
            }
        }
        if (col.gameObject.tag == "RightWall")
        {
            rigid2D.gravityScale = m_WallSlideGravity;
            rigid2D.velocity = Vector2.zero;
            b_TouchWall = true;
            if (b_Right)
            {
                b_Right = false;
                b_Left = true;
            }
        }
        
    }

    void OnCollisionExit2D(Collision2D col) // reset value upon exit
    {
        ResetGravity();
        if (col.gameObject.tag != "Ground")
        {
            if (!b_Ground)
                b_Jump = false;
            b_TouchWall = false;
        }
    }

    void SetScoreText()
    {
        t_uiTxt.text = "Health " + i_Health;
    }

    void ResetGravity()
    {
        rigid2D.gravityScale = m_Gravity;
    }

    void Stun()
    {
        if(t_StunTime < 0.5f)
        {
            m_MoveSpeed = 0;
            t_StunTime += Time.deltaTime;
        }
        else if(t_StunTime >= 0.5f)
        {
            m_MoveSpeed = m_TempMove;
            t_StunTime = 0;
            b_Stun = false;
        }
    }

    void AnimationUpdate()
    {
        // jump slam eat not here;
        anim.SetFloat("Movement", Mathf.Abs(XPos));
        anim.SetBool("Ground", b_Ground);
        anim.SetBool("Walling", b_TouchWall);
        anim.SetBool("Hurt", b_Stun);
         }
}
