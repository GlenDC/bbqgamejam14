using UnityEngine;
using System.Collections;

public class Ninja : Character
{
	public MeshRenderer meshRenderer;
	public NinjaRange rangeChecker;

	public Material normal, inRange;

	public float attackedInBetweenDelay = 0.5f;
	public float feedbackInBetweenDelay = 0.5f;

	float attackIBCT;
	float feedbackIBCT;

	protected override void Awake()
    {
    	base.Awake();

    	attackIBCT = attackedInBetweenDelay;
		feedbackIBCT = feedbackInBetweenDelay;
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

			GetEnemy().GetComponent<Character>().onAttacked(enemyDir);
		}
    }

    public override void onAttacked(Vector3 dir)
    {
    	if(attackIBCT >= attackedInBetweenDelay)
    	{
    		attackIBCT = 0.0f;
        	throwback.currentThrowback.x = throwback.throwBackStrength * dir.x;
    	}
    }

    public override void onFeedback(Vector3 dir)
    {
    	if(feedbackIBCT >= feedbackInBetweenDelay)
    	{
    		feedbackIBCT = 0.0f;
        	throwback.currentThrowback.x = throwback.feedbackStrength * dir.x * -1.0f;
    	}
    }

	public void OnTriggerEnter(Collider trigger_collider){

		if (trigger_collider.gameObject.tag == "SAUSAGE_WARP"){

			if (trigger_collider.gameObject.GetComponent<WarpTileObject>().GetWarpOn()){

				GameObject level_holder = GameObject.FindGameObjectWithTag("LEVEL_HOLDER");
				level_holder.GetComponent<LevelHolder>().TriggerNinjaWarp();
			}
		}
		else if (trigger_collider.gameObject.tag == "NINJA_WARP"){

			if (trigger_collider.gameObject.GetComponent<WarpTileObject>().GetWarpOn()){

				GameObject level_holder = GameObject.FindGameObjectWithTag("LEVEL_HOLDER");
				level_holder.GetComponent<LevelHolder>().TriggerSausageWarp();
			}
		}
	}
}
