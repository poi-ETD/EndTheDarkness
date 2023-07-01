using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card17 : TeamTargetCard
{
	public override bool UseCard()
	{
		if (!base.UseCard())
			return false;

		BM.getArmor((int)values[0], BM.actCharacter);
		BM.NextTurnArmor((int)values[1], BM.FindCharacterByOwner(Owner.vangara));
	
		return true;
	}
}
