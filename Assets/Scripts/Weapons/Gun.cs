using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public float damage;
    public float fireRate=0.5f, nextfire=0;
    public Camera PlayerCam;
    public bool reloaded;
    public GameObject EffectOfShooting;
    public PlayerController PlayerScript;
    public AudioSource ShotAudio;
    public virtual void Shoot()
    {

    }
    public virtual void Reload()
    {

    }
}
