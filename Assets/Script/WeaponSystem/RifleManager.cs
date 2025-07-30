using System.Collections;
using UnityEngine;
using static PlayerController;

public class RifleManager : MonoBehaviour, IWeaponHandler
{
    [Header("Riffle Things")]
    public Transform shootingArea;
    //public float giveDamage = 10f;
    public float shootingRange = 100f;
    public Animator anim;
    public GameObject gun;

    [Header("Riffle Ammunition and Reloading")]
    private int maximumAmmunition = 100;
    public int currentAmmunition;
    public int mag;
    public float reloadingTime;
    private bool setReloading;

    private void Start()
    {
        currentAmmunition = maximumAmmunition;
    }

    private void OnEnable()
    {
        gun.SetActive(true);
        anim.SetBool("RifleActive", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (setReloading)
        {
            return;
        }

        if (currentAmmunition <= 0 && mag > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Shooting", true);
            Shoot();
        }
        else if (!Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Shooting", false);
        }
    }
    void Shoot()
    {
        if (mag <= 0)
        {
            return;
        }

        currentAmmunition--;

        if (currentAmmunition == 0)
        {
            mag--;
        }

        /*
        RaycastHit hitInfo;
        if (Physics.Raycast(shootingArea.position, shootingArea.forward, out hitInfo, shootingRange))
        {
            //Debug.Log(hitInfo.transform.name);

            //damage enemy
        }
        */
    }
    IEnumerator Reload()
    {
        setReloading = true;
        PlayerController.instance.canMove = false;
        anim.SetBool("ReloadRifle", true);
        //Reload Anim
        yield return new WaitForSeconds(reloadingTime);
        anim.SetBool("ReloadRifle", false);
        PlayerController.instance.canMove = true;

        currentAmmunition = maximumAmmunition;
        setReloading = false;
    }
    public void QuitWeapon()
    {
        anim.SetBool("RifleActive", false);
        return;
    }
}
