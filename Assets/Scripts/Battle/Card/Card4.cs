using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card4 : RenewalCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;
		BM.SpecialDrow((int)values[0]);
		BM.SpeedChange(BM.actCharacter, -values[1]);

		return true;
	}
}
