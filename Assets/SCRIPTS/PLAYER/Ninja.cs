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

		if (inRange && playerController.special)
		{
			if (!controller.isGrounded)
			{
				onFeedback();
			}

			GetEnemy().GetComponent<Character>().onAttacked();
		}
    }

    public override void onAttacked()
    {
    	if(attackIBCT >= attackedInBetweenDelay)
    	{
    		attackIBCT = 0.0f;
        	Debug.Log("Change to wurst!");
    	}
    }

    public override void onFeedback()
    {
    	if(feedbackIBCT >= feedbackInBetweenDelay)
    	{
    		feedbackIBCT = 0.0f;
        	Debug.Log("Ouch");
    	}
    }
}
