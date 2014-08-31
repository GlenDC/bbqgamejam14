﻿using UnityEngine;
using System.Collections;

public class Ninja : Character
{
	public MeshRenderer meshRenderer;
	public NinjaRange rangeChecker;

	public Material normal, inRange;

	public float attackedInBetweenDelay = 0.5f;
	public float feedbackInBetweenDelay = 0.5f;

	public AudioClip[] AttackSounds = new AudioClip[5];

	float attackIBCT;
	float feedbackIBCT;

	System.Random rnd;

	protected override void Awake()
    {
    	base.Awake();

    	attackIBCT = attackedInBetweenDelay;
		feedbackIBCT = feedbackInBetweenDelay;

		rnd = new System.Random();
    }

	protected override void Update()
    {
    	base.Update();
		meshRenderer.material = rangeChecker.enemyIsInRange ? inRange : normal;

		if(attackIBCT < attackedInBetweenDelay)
			attackIBCT += Time.deltaTime;

		if(feedbackIBCT < feedbackInBetweenDelay)
			feedbackIBCT += Time.deltaTime;

		if (rangeChecker.enemyIsInRange && playerController.special)
		{
			Vector3 posEnemy = GetEnemy().transform.position;
			Vector3 enemyDir = posEnemy - transform.position;
			enemyDir.Normalize();

			if (!controller.isGrounded)
			{
				onFeedback(enemyDir);
			}

			if(posEnemy.x + 0.5 >= transform.position.x)
				enemyDir.y += 1.0f;
			else
				enemyDir.y -= 0.5f;
			GetEnemy().GetComponent<Character>().onAttacked(enemyDir);
		}
    }

    public override void onAttacked(Vector3 dir)
    {
    	if(attackIBCT >= attackedInBetweenDelay)
    	{
    		attackIBCT = 0.0f;
        	throwback.currentThrowback = throwback.throwBackStrength * dir;
        	audioSource.PlayOneShot(AttackSounds[rnd.Next(0, AttackSounds.Length)]);
    	}
    }

    public override void onFeedback(Vector3 dir)
    {
    	if(feedbackIBCT >= feedbackInBetweenDelay)
    	{
    		feedbackIBCT = 0.0f;
        	throwback.currentThrowback = throwback.feedbackStrength * dir * -1.0f;
    	}
    }

	public override void OnTriggerEnter(Collider trigger_collider){

		if (trigger_collider.gameObject.tag == "SAUSAGE_WARP"){

			if (trigger_collider.gameObject.GetComponent<WarpTileObject>().GetWarpOn()){
                OnSausageWarp();
			}
		}
	}
}
