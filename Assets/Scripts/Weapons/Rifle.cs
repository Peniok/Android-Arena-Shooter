using Photon.Pun;
using UnityEngine;

public class Rifle : Gun
{
    [SerializeField] Camera PlayerCam;
    [SerializeField] GameObject AimPoint, LazerBeamObj;
    public LineRenderer LazerBeam;
    public bool reloaded = true;
    [SerializeField] PlayerController PlayerScript;
    // Start is called before the first frame update
    /*public override void Shot()
    {
        transform.parent.GetComponent<WeaponControl>().PlayerPrefab.GetComponent<PlayerController>().photonView.RPC(nameof(RPC_Shot), RpcTarget.All);
    }
    [PunRPC]*/
    public override void Shot()
    {
        if (reloaded == true)
        {
            LazerBeam.enabled = true;
            Ray ray = PlayerCam.ScreenPointToRay(new Vector2(Screen.width/2,Screen.height/2)/*new Vector3(0.5f, 0, 0.5f)*/);
            //ray.origin = PlayerCam.transform.position;
            LazerBeam.SetPosition(0, Vector3.zero);
            Invoke("LazerOff", 0.2f);
            Invoke("Reload", 2.5f);
            reloaded = false;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                LazerBeam.SetPosition(1, transform.InverseTransformPoint(hit.point));
                PlayerScript.DrawShotLine(1, hit.point);
                if(hit.collider.gameObject.GetComponent<PlayerController>() != null)
                {
                    hit.collider.gameObject.GetComponent<PlayerController>().TakeDamage();
                    //PlayerScript.TakeDamage();
                }
                Debug.Log("We hit" + hit.collider.name);
            }
            else
            {
                LazerBeam.SetPosition(1, new Vector3(4,-7,40));
                PlayerScript.DrawShotLine(1, new Vector3(4, -7, 40));
            }
        }
    }
    public void LazerOff()
    {
        LazerBeam.enabled = false;
    }
    public void Reload()
    {
        reloaded = true;
    } 
}
