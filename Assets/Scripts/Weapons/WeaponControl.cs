using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public GameObject ChosedWeapon, Player;
    public List<GameObject> Weapons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Use()
    {
        ChosedWeapon.GetComponent<Gun>().Shot();
        //ChosedWeapon.GetComponent<Rifle>().Shot();
    }
}
