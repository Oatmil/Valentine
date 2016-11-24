using UnityEngine;
using System.Collections;

public class DmgScript : MonoBehaviour {

    int i_FlipDir = 0;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.transform.tag);
        if (col.transform.tag == "Player")
        {
            CharacterMovement Char1 = col.transform.GetComponent<CharacterMovement>();
            CharacterMovement SelfChar = transform.parent.GetComponent<CharacterMovement>();
            Char1.b_Stun = true;
            Char1.t_StunTime = 0;
            Char1.i_Health -= SelfChar.i_Damage;
            SelfChar.i_Damage = 0;
            CalculateDir(col);
            col.transform.GetComponent<Rigidbody2D>().AddForce(new Vector3(SelfChar.v_SlamKnockBack.x * i_FlipDir, SelfChar.v_SlamKnockBack.y, SelfChar.v_SlamKnockBack.z), ForceMode2D.Impulse);
        }
    }

    void CalculateDir(Collider2D col)
    {
        Vector3 SelfPos = transform.parent.transform.position;
        Vector3 enemyPos = col.transform.position;
        if (SelfPos.x < enemyPos.x)
        {
            i_FlipDir = 1;
        }
        else if (SelfPos.x > enemyPos.x)
        {
            i_FlipDir = -1;
        }
    }
}
