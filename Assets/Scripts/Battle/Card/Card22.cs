using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card22 : RenewalCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;
		BM.card22((int)values[0], (int)values[1]);
		return true;
	}
}
