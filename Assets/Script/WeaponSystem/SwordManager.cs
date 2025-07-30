using System.Collections;
using UnityEngine;

public class SwordManager : MonoBehaviour, PlayerController.IWeaponHandler
{
    public int SwordAttackVal;
    public Animator anim;

    public PlayerController playerCon;
    public GameObject sword;

    private void OnEnable()
    {
        sword.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("SwordAttackActive", true);
        }

        SwordAttackModes();
    }
    void SwordAttackModes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SwordAttackVal = Random.Range(1, 6);

            if (SwordAttackVal >= 1 && SwordAttackVal <= 5)
            {
                //Play Animation
                StartCoroutine(PlayAttackAnimation(SwordAttackVal));
            }
        }
    }
    IEnumerator PlayAttackAnimation(int animationIndex)
    {
        switch (animationIndex)
        {
            case 1:
                yield return StartCoroutine(Attack("SingleAttackA"));
                break;
            case 2:
                yield return StartCoroutine(Attack("SingleAttackB"));
                break;
            case 3:
                yield return StartCoroutine(Attack("SingleAttackC"));
                break;
            case 4:
                yield return StartCoroutine(Attack("TwoHitCombo"));
                break;
            case 5:
                yield return StartCoroutine(Attack("ThreeHitCombo"));
                break;
        }
    }
    IEnumerator Attack(string attackType)
    {
        sword.GetComponent<Collider>().enabled = true;
        anim.SetBool(attackType, true);
        playerCon.movementSpeed = 0f;
        anim.SetFloat("Speed", 0);
        yield return new WaitForSeconds(0.4f);

        anim.SetBool(attackType, false);
        playerCon.movementSpeed = 5f;
    }
    public void QuitWeapon()
    {
        anim.SetBool("SwordAttackActive", false);
        sword.SetActive(false);
        return;
    }
}
