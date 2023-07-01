using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Card3 : EnemyTargetCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;
		BM.OnAttack((int)values[0], targetEnemy, BM.actCharacter, 1);
		BM.SpecialDrow((int)values[1]);
		return true;
	}
}
