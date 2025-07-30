using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    [Header("UI - unselected")]
    public List<GameObject> unselectedUI = new List<GameObject>();

    [Header("UI - selected")]
    public List<GameObject> selectedUI = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        /*
        for (int i = 0; i < unselectedUI.Count; i++)
        {
            unselectedUI[i].SetActive(false);
        }
        
        for (int i = 0; i < selectedUI.Count; i++)
        {
            selectedUI[i].SetActive(false);
        }
        */
    }

    public void changeWeaponUI(int weaponIndex)
    {
        for (int i = 0; i < selectedUI.Count; i++)
        {
            bool isActive = (i == weaponIndex);

            if (selectedUI[i] != null)
                selectedUI[i].SetActive(isActive);  // ��ɫ�򣺽���ǰ�������ʾ

            if (unselectedUI[i] != null)
                unselectedUI[i].SetActive(!isActive); // ��ɫ�����༤�������ʾ
        }
    }
}
