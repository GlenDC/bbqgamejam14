using UnityEngine;
using System.Collections;

public class Ninja : Character
{
	public MeshRenderer meshRenderer;
	public NinjaRange rangeChecker;

	public Material normal, inRange;


	protected override void Update()
    {
    	base.Update();
		meshRenderer.material = rangeChecker.enemyIsInRange ? inRange : normal;
    }
}
