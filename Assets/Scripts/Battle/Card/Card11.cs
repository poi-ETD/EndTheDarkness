using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card11 : EnemyTargetCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;
		BM.OnAttack((int)values[1],BM.selectedEnemy, BM.actCharacter,1);
		return true;
	}
	public override void TurnCardUse(int count)
	{
		base.TurnCardUse(count);
		if(count == 4)
		{
			//4번째라면,
			changeTurnCardCost(-(int)values[0]);
		}
	}
}
