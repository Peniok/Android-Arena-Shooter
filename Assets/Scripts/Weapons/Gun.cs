using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public float damage;
    public float fireRate;
    public Camera PLayerCam;
    public virtual void Shot()
    {

    }
    public virtual void Reload()
    {

    }
}
