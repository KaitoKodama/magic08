using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : StateMachineBehaviour
{
	private Enemy enemy;
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (enemy == null)
		{
			enemy = animator.transform.GetComponent<Enemy>();
		}
		enemy.OnStateExit();
	}
}
