using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class BossStats : MonoBehaviour
{
    public int maxhp = 100;
    public int curhp;
    public GameObject bossbar;
    public Image HPbar;

    public ParticleSystem Hit;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        curhp = maxhp;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        HPbar.fillAmount = ((float)curhp/ (float)maxhp);
    }

    public void TakeDamage(int damage)
    {
        curhp -= damage;
        audioSource.PlayOneShot(audioSource.clip);
        Hit.Play();

        if (curhp <= 0)
        {
            Animator bossanimator = GetComponent<Animator>();
            Boss boss = GetComponent<Boss>();

            boss.SetState(Boss.BossState.Die);

            UIManager.s.Clear();

        }
    }
}
