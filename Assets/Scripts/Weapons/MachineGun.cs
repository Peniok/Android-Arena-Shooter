using UnityEngine;

public class MachineGun : Gun
{
    public bool WorkingParticleSystem;//to start show effects only on first calling of this function
    public ParticleSystem flash;
    public override void Shoot()
    {
        if (reloaded ==true && Time.time>=nextfire)
        {
            if(WorkingParticleSystem == false)
            {
                flash.Play();
                WorkingParticleSystem = true;
            }
            nextfire = Time.time + 1f / fireRate;
            RaycastHit hit;
            if (Physics.Raycast(PlayerCam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)), out hit))
            {
                
                if (hit.collider.gameObject.GetComponent<PlayerController>() != null)
                {
                    hit.collider.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                }
                Debug.Log("We hit" + hit.collider.name);
            }
        }
    }
}
