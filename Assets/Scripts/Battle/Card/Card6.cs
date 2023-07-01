using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Card6 : RenewalCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;

		BM.GhostRevive((int)values[0]);
		BM.SpecialDrow((int)values[1]);
		BM.SetNextTurnCost((int)values[2]);
		return true;
	}
}
