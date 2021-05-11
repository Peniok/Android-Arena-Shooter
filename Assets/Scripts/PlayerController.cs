﻿using Photon.Pun;
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
    //
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

        Invoke("SpawnStabilize", 0.01f);
        Invoke("SpawnStabilize", 0.3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        ShotAndAim();
        Look(AimField, 1);
        Move();
        Rigidbody.MovePosition(Rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime*5);
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
    public void SpawnStabilize()
    {
        if (Rigidbody != null)
        {
            Rigidbody.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        }
    }
    void Look(TouchField AimerOrShooter, float LookMultiplier)
    {
        Rigidbody.MoveRotation(Rigidbody.rotation * Quaternion.Euler(new Vector3(0, AimerOrShooter.TouchDistance.x*LookMultiplier, 0)));
        verticalLookRotation += AimerOrShooter.TouchDistance.y* LookMultiplier;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        PlayerCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    void ShotAndAim()
    {

        if (ShootField.Pressed == true)
        {
            WeaponHolder.Use();
            Look(ShootField, 0.5f);
        }

    }
    
    void Move()
    {
        Vector3 moveDir = new Vector3(Mover.Horizontal, 0, Mover.Vertical).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir, ref smoothMoveVelocity, smoothTime);
    }
    
    public void TakeDamage()
    {
        photonView.RPC(nameof(RPC_TakeDamage), RpcTarget.All);
    }

    [PunRPC]
    public void RPC_TakeDamage()
    {
        if (!photonView.IsMine)
            return;
        if (HealthBar.fillAmount <= 0)
        {
            Die();
        }
        HealthBar.fillAmount -= 0.34f;
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
        LineRenderer  LazerBeam = WeaponHolder.ChosedWeapon.GetComponent<Rifle>().LazerBeam;
        if (PositionOfPoint == new Vector3(580, -4547, 40))
        {
            LazerBeam.SetPosition(NumderOfPoint, PositionOfPoint);
        }
        else
        {
            LazerBeam.SetPosition(NumderOfPoint, LazerBeam.transform.InverseTransformPoint(PositionOfPoint));
        }
        LazerBeam.enabled = true;
        WeaponHolder.ChosedWeapon.GetComponent<Rifle>().Invoke("LazerOff",0.2f);
    }
    public void Die() 
    {
        GameNetworkManager.SpawningPlayer();
    }
}
