using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card18 : RenewalCard
{
	public override bool UseCard()
	{
		if (!base.UseCard())
			return false;

		Character sparky = BM.FindCharacterByOwner(Owner.sparky);
		BM.TurnSpeedChange(sparky, -values[0]);
		BM.TurnAtkUp(sparky, (int)values[1]);

		return true;
	}
}
