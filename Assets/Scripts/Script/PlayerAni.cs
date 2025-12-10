using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Idle,
    Walk,
    SwordAttack,
    BowAttack,
    SpearAttack,
    Die,
    roll
}




public class PlayerAni : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

    }
    public void ChangeAni(PlayerState aniNumber)
    {
        anim.SetInteger("aniName",(int)aniNumber);
        Debug.Log(aniNumber);

        if(aniNumber == PlayerState.Die)
        {
            anim.SetTrigger("Die");
        }
    }


}
