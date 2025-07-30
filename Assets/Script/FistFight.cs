using System.Collections;
using UnityEngine;
using static PlayerController;

public class FistFight : MonoBehaviour, IWeaponHandler
{
    public float timer = 0f;
    public int FistFightVal;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if(!Input.GetMouseButtonDown(0))
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            //Debug.Log("FistFight Mode On");
            anim.SetBool("FistFightActive", true);
        }

        if(timer > 5f)
        {
            //Debug.Log("FistFight Mode Off");
            anim.SetBool("FistFightActive", false);
        }

        FistFightModes();
    }

    IEnumerator SingleFist()
    {
        anim.SetBool("SingleFist", true);
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("SingleFist", false);
    }

    IEnumerator DoubleFist()
    {
        anim.SetBool("DoubleFist", true);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("DoubleFist", false);
    }

    IEnumerator FirstFistKick()
    {
        anim.SetBool("FirstFistKick", true);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("FirstFistKick", false);
    }

    IEnumerator KickCombo()
    {
        anim.SetBool("KickCombo", true);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("KickCombo", false);
    }

    IEnumerator LeftKick()
    {
        anim.SetBool("LeftKick", true);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("LeftKick", false);
    }

    void FistFightModes()
    {
        if(Input.GetMouseButtonDown(0))
        {
            FistFightVal = Random.Range(1, 7);

            if(FistFightVal == 1)
            {
                StartCoroutine(SingleFist());
            }
            if(FistFightVal == 2)
            {
                StartCoroutine(DoubleFist());
            }
            if(FistFightVal == 3)
            {
                StartCoroutine(FirstFistKick());
            }
            if (FistFightVal == 4)
            {
                StartCoroutine(KickCombo());
            }
            if (FistFightVal == 5)
            {
                StartCoroutine(LeftKick());
            }
        }
    }
    public void QuitWeapon()
    {
        anim.SetBool("FistFightActive", false);
        return;
    }
}
