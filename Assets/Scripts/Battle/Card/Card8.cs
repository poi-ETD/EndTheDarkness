using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Card8 : TeamTargetCard
{
	public override bool UseCard()
	{
		cardcost = BM.leftCost;
		if (!base.UseCard()) return false;
		BM.getArmor((int)values[0]* cardcost, BM.selectedCharacter);
		return true;
	}
	 private void Update()
	{
		if (gameObject.active)
		{ costT.text = BM.leftCost.ToString();
		}
	}
}
