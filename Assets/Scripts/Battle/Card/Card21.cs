using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card21 : RenewalCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;
		BM.RandomReviveToField((int)values[0],true);
		return true;
	}
}
