using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int JumpCounter;
    public TouchField AimField, ShootField;
    public WeaponControl WeaponHolder;
    public GameNetworkManager GameNetworkManager;
    public PhotonView photonView;
    public Joystick Mover;
    public Rigidbody Rigidbody;
    public Button Jump;
    [SerializeField] private Camera PlayerCamera;
    public Canvas Canvas;
    [SerializeField] Image HealthBar;
    [SerializeField]float verticalLookRotation, smoothTime;
    public bool IsGrounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    void Start()
    {
        HealthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        HealthBar.fillAmount = 1;
        GameNetworkManager = FindObjectOfType<GameNetworkManager>();
        Rigidbody.velocity = Vector3.zero;
        if (!photonView.IsMine)
        {
            Destroy(PlayerCamera.gameObject.GetComponent<AudioListener>());
            Destroy(PlayerCamera);
            Destroy(Canvas.gameObject);
            Destroy(Rigidbody);
        }
    }
    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        ShotAndAim();
        Look(AimField, 0.5f);
        Move();

    }
    public void OnButtonJumpDown()
    {
        if (JumpCounter == 2)
        {
            Rigidbody.AddForce(new Vector3(0, 350f, 0));
            JumpCounter--;
        }
        else if(JumpCounter == 1)
        {
            Rigidbody.AddForce(new Vector3(0, 300f, 0));
            JumpCounter--;
        }
    }
    public void OnLeaveButtonDown()
    {
        GameNetworkManager.Leave();
    }
    void Look(TouchField AimerOrShooter, float LookMultiplier)
    {
        //transform.Rotate(Vector3.up * Aimer.Horizontal* LookMultiplier);
        Rigidbody.MoveRotation(Rigidbody.rotation * Quaternion.Euler(new Vector3(0, AimerOrShooter.TouchDistance.x*LookMultiplier, 0)));
        verticalLookRotation += AimerOrShooter.TouchDistance.y* LookMultiplier;//Aimer.Vertical;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        PlayerCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    void ShotAndAim()
    {
        if (ShootField.Pressed == true)
        {
            WeaponHolder.Use();
            Look(ShootField, 0.25f);
        }
        if(ShootField.Pressed == false && WeaponHolder.ChosedWeapon.GetComponent<Gun>().EffectOfShooting != null)
        {
            WeaponHolder.ChosedWeapon.GetComponent<Gun>().EffectOfShooting.GetComponent<ParticleSystem>().Stop();
            WeaponHolder.ChosedWeapon.GetComponent<MachineGun>().WorkingParticleSystem = false;
            ShowMachineGunShooting(false);
        }
    }
    void Move()
    {
        Vector3 moveDir = new Vector3(Mover.Horizontal, 0, Mover.Vertical).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir, ref smoothMoveVelocity, smoothTime);
        Rigidbody.MovePosition(Rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime * 5);
    }
    
    public void TakeDamage(float damage)
    {
        photonView.RPC(nameof(RPC_TakeDamage), RpcTarget.All, damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        if (!photonView.IsMine)
            return;
        if (HealthBar.fillAmount <= 0)
        {
            Die();
        }
        HealthBar.fillAmount -= damage;
    }

    public void DrawShotLine(int NumderOfPoint, Vector3 PositionOfPoint)
    {
        photonView.RPC(nameof(RPC_DrawShotLine), RpcTarget.All, NumderOfPoint, PositionOfPoint);
    }

    [PunRPC]
    public void RPC_DrawShotLine(int NumderOfPoint, Vector3 PositionOfPoint)
    {
        if (photonView.IsMine)
            return;
        LineRenderer  LazerBeam = WeaponHolder.Weapons[0].GetComponent<Rifle>().LazerBeam;
        if (PositionOfPoint == new Vector3(580, -4547, 40))
        {
            LazerBeam.SetPosition(NumderOfPoint, PositionOfPoint);
        }
        else
        {
            LazerBeam.SetPosition(NumderOfPoint, LazerBeam.transform.InverseTransformPoint(PositionOfPoint));
        }
        LazerBeam.enabled = true;
        WeaponHolder.Weapons[0].GetComponent<Rifle>().Invoke("LazerOff",0.2f);
    }
    public void ShowMachineGunShooting(bool Shooting)
    {
        photonView.RPC(nameof(RPC_ShowMachineGunShooting), RpcTarget.All, Shooting);
    }
    [PunRPC]
    public void RPC_ShowMachineGunShooting(bool Shooting)
    {
        if (photonView.IsMine)
            return;
        if (Shooting == true)
        {
            WeaponHolder.Weapons[1].GetComponent<MachineGun>().flash.Play();
        }
        if (Shooting == false)
        {
            WeaponHolder.Weapons[1].GetComponent<MachineGun>().flash.Stop();
        }
    }
    public void Die()
    {
        GameNetworkManager.SpawningPlayer();
    }
}
