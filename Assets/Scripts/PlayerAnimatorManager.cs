using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour {

	private Animator playerAnimator;
	
	[SerializeField]
	private float directionDampTime = 0.25f;

	// Use this for initialization
	void Start () {
		
		playerAnimator = GetComponent<Animator>();
		if (!playerAnimator)
		{
			Debug.LogError("PlayerAnimatorManager is missing animator component", this);
		}

		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!playerAnimator)
		{
			return;
		}

		// deal with Jumping
		AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
		
		// only allow jumping if we are running.
		if (stateInfo.IsName("Base Layer.Run")) {

			// when using trigger parameter
			if (Input.GetButtonDown("Fire2")) {

				playerAnimator.SetTrigger("Jump");
			}
		}

		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		if (verticalInput < 0)
		{
			verticalInput = 0;
		}

		playerAnimator.SetFloat("Speed", horizontalInput * horizontalInput + verticalInput * verticalInput);
		playerAnimator.SetFloat("Direction", horizontalInput, directionDampTime, Time.deltaTime);		
	}
}
