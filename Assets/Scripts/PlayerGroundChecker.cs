using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
	[SerializeField] PlayerController playerController;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == playerController.gameObject)
			return;

		playerController.IsGrounded = true;
		playerController.JumpCounter = 2;
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == playerController.gameObject)
			return;

		playerController.IsGrounded = false;

		//playerController.JumpCounter = 1;
	}

	/*void OnTriggerStay(Collider other)
	{
		if (other.gameObject == playerController.gameObject)
			return;

		playerController.IsGrounded = true;
		playerController.JumpCounter = 2;
	}*/
}
