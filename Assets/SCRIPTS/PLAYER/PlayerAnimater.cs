using UnityEngine;
using System.Collections;

public class PlayerAnimater : MonoBehaviour {

	[ SerializeField ] GameObject
		StandingAnimation,
		RunAnimation,
		AttackAnimation,
		JumpAnimation,
		BlockAnimation,
		FlyAnimation;

	bool
		DirectionLeft,
		DirectionRight,
		InAir,
		IsBlocking;

	Vector3
		StandingScaleLeft,
		StandingScaleRight,
		RunScaleLeft,
		RunScaleRight,
		AttackScaleLeft,
		AttackScaleRight,
		JumpScaleLeft,
		JumpScaleRight,
		BlockScaleLeft,
		BlockScaleRight,
		FlyingScaleLeft,
		FlyingScaleRight;

	// Use this for initialization
	void Start () {
	
		SetUpPlayerAnimater();
	}

	public void SetUpPlayerAnimater(){

		SetPlayerStanding();

		StandingScaleRight = StandingAnimation.transform.localScale;
		StandingScaleLeft = StandingAnimation.transform.localScale;
		float standing_scale_left = StandingScaleRight.x;
		StandingScaleLeft.x = - standing_scale_left;

		RunScaleRight = RunAnimation.transform.localScale;
		RunScaleLeft = RunAnimation.transform.localScale;
		float run_scale_left = RunScaleRight.x;
		RunScaleLeft.x = - run_scale_left;

		if(AttackAnimation)
		{
			AttackScaleRight = AttackAnimation.transform.localScale;
			AttackScaleLeft = AttackAnimation.transform.localScale;
			float attack_scale_left = AttackScaleRight.x;
			AttackScaleLeft.x = - attack_scale_left;
		}

		JumpScaleRight = JumpAnimation.transform.localScale;
		JumpScaleLeft = JumpAnimation.transform.localScale;
		float jump_scale_left = JumpScaleRight.x;
		JumpScaleLeft.x = - jump_scale_left;

		if(BlockAnimation)
		{
			BlockScaleRight = BlockAnimation.transform.localScale;
			BlockScaleLeft = BlockAnimation.transform.localScale;
			float block_scale_left = BlockScaleRight.x;
			BlockScaleLeft.x = - block_scale_left;
		}

		FlyingScaleRight = FlyAnimation.transform.localScale;
		FlyingScaleLeft = FlyAnimation.transform.localScale;
		float flying_scale_left = FlyingScaleRight.x;
		FlyingScaleLeft.x = - flying_scale_left;
	}

	public bool GetInAir(){
		bool in_air = InAir;
		return in_air;
	}

	public void SetAnimationDirection(bool direction_left, bool direction_right){

		if (direction_left){

			StandingAnimation.transform.localScale = StandingScaleLeft;
			RunAnimation.transform.localScale = RunScaleLeft;
			if(AttackAnimation)
				AttackAnimation.transform.localScale = AttackScaleLeft;
			JumpAnimation.transform.localScale = JumpScaleLeft;
			if(BlockAnimation)
				BlockAnimation.transform.localScale = BlockScaleLeft;
			FlyAnimation.transform.localScale = FlyingScaleLeft;
		}
		else if (direction_right){

			StandingAnimation.transform.localScale = StandingScaleRight;
			RunAnimation.transform.localScale = RunScaleRight;
			if(AttackAnimation)
				AttackAnimation.transform.localScale = AttackScaleRight;
			JumpAnimation.transform.localScale = JumpScaleRight;
			if(BlockAnimation)
				BlockAnimation.transform.localScale = BlockScaleRight;
			FlyAnimation.transform.localScale = FlyingScaleRight;
		}
	}

	public void SetPlayerStanding(){

		if (!IsBlocking){

			StandingAnimation.renderer.enabled = true;

			RunAnimation.renderer.enabled = false;
			if(AttackAnimation)
				AttackAnimation.renderer.enabled = false;
			JumpAnimation.renderer.enabled = false;
			if(BlockAnimation)
				BlockAnimation.renderer.enabled = false;
			FlyAnimation.renderer.enabled = false;
		}
	}

	public void SetPlayerRunning (){

		RunAnimation.renderer.enabled = true;

		StandingAnimation.renderer.enabled = false;
		if(AttackAnimation)
			AttackAnimation.renderer.enabled = false;
		JumpAnimation.renderer.enabled = false;
		if(BlockAnimation)
			BlockAnimation.renderer.enabled = false;
		FlyAnimation.renderer.enabled = false;
	}

	public void SetPlayerJumping (){

		JumpAnimation.renderer.enabled = true;

		StandingAnimation.renderer.enabled = false;
		if(AttackAnimation)
			AttackAnimation.renderer.enabled = false;
		RunAnimation.renderer.enabled = false;
		if(BlockAnimation)
			BlockAnimation.renderer.enabled = false;
		FlyAnimation.renderer.enabled = false;
	}

	public void SetPlayerFlying (){

		FlyAnimation.renderer.enabled = true;

		StandingAnimation.renderer.enabled = false;
		if(AttackAnimation)
			AttackAnimation.renderer.enabled = false;
		RunAnimation.renderer.enabled = false;
		if(BlockAnimation)
			BlockAnimation.renderer.enabled = false;
		JumpAnimation.renderer.enabled = false;
	}

	public void SetPlayerAttacking(){

		if(AttackAnimation)
		{
			Debug.Log ("player attacking");

			AttackAnimation.renderer.enabled = true;

			StandingAnimation.renderer.enabled = false;
			FlyAnimation.renderer.enabled = false;
			RunAnimation.renderer.enabled = false;
			if(BlockAnimation)
				BlockAnimation.renderer.enabled = false;
			JumpAnimation.renderer.enabled = false;
		}
	}

	public void SetPlayerBlocking(){

		Debug.Log ("character blocks");
		BlockAnimation.renderer.enabled = true;

		if(AttackAnimation && BlockAnimation)
		{
			AttackAnimation.renderer.enabled = true;

			StandingAnimation.renderer.enabled = false;
			FlyAnimation.renderer.enabled = false;
			RunAnimation.renderer.enabled = false;
			if(!BlockAnimation)
				BlockAnimation.renderer.enabled = true;
			JumpAnimation.renderer.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
