using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public GameObject ChosedWeapon, Player;
    public List<GameObject> Weapons;
    public void TakeRifle()
    {
        Weapons[0].SetActive(true);
        ChosedWeapon = Weapons[0];
        Weapons[1].SetActive(false);
    }
    public void TakeMachineGun()
    {
        Weapons[1].SetActive(true);
        ChosedWeapon = Weapons[1];
        Weapons[0].SetActive(false);
    }
    public void Use()
    {
        ChosedWeapon.GetComponent<Gun>().Shoot();
        //ChosedWeapon.GetComponent<Rifle>().Shot();
    }
}
