using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public GameObject ChosedWeapon, Player;
    public List<GameObject> Weapons;
    public void Use()
    {
        ChosedWeapon.GetComponent<Gun>().Shot();
    }
}
