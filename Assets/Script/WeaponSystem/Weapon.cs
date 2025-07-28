using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*
    public GameObject bullet;

    public bool canAutoFire;
    public float fireRate;

    [HideInInspector]
    public float FireCounter;

    public int currentAmmo, pickupAmount;
    */
    public bool isUnlocked = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (FireCounter > 0)
        {
            FireCounter -= Time.deltaTime;
        }
        */
    }

    public void GetAmmo()
    {
        /*
        currentAmmo += pickupAmount;
        UIController.instance.AmmoText.text = "Ammo: " + currentAmmo;
        */
    }
}
