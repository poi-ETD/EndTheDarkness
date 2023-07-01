using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card9 : RenewalCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;
		BM.NextTurnArmor((int)values[0],BM.actCharacter);
		return true;
	}
}
