using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card5 : RenewalCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;
		BM.CopyCard((int)values[1]);
		BM.GhostRevive((int)values[0]);
		return true;
	}
}
