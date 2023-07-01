using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card15 : EnemyTargetCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;
		BM.OnAttack(CM.Grave.Count, targetEnemy, BM.actCharacter, 1);

		return true;
	}
}
