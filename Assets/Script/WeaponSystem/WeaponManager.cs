using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    [Header("UI_deselected")]
    public GameObject unselected1;
    public GameObject unselected2;
    public GameObject unselected3;
    public GameObject unselected4;
    public GameObject unselected5;

    [Header("UI_selected")]
    public GameObject selected1;
    public GameObject selected2;
    public GameObject selected3;
    public GameObject selected4;
    public GameObject selected5;
    public GameObject selected6;

    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unselected1.SetActive(false);
        unselected2.SetActive(false);
        unselected3.SetActive(false);
        unselected4.SetActive(false);
        unselected5.SetActive(false);

        selected1.SetActive(false);
        selected2.SetActive(false);
        selected3.SetActive(false);
        selected4.SetActive(false);
        selected5.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeWeaponUI(int weaponIndex)
    {

    }
}
