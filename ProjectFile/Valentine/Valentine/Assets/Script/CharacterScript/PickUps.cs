using UnityEngine;
using System.Collections;

public class PickUps : MonoBehaviour {
    public int i_HP;
    public int i_DMG;
    public float i_SPDMod;
    public float i_JMPMod;

    public bool b_ResetBlock;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            PickUpsManager.PickMng_instance.PickUpsUsed(transform.position);
            CharacterMovement Char1 = col.transform.GetComponent<CharacterMovement>();
            if (b_ResetBlock)
            {
                Char1.i_Health += i_HP;
                Char1.i_Damage += i_DMG;
                Char1.i_SpeedMod = 1;
                Char1.i_JumpMod = 1;
            }
            else
            {
                Char1.i_Health += i_HP;
                Char1.i_Damage += i_DMG;
                Char1.i_SpeedMod += i_SPDMod;
                Char1.i_JumpMod += i_JMPMod;
            }
            Char1.Mods();
            Destroy(this.gameObject);
        }
    }
}
