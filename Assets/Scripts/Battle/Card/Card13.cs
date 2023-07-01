using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card13 : GraveChoiceCard
{
	public override bool UseCard()
	{
		graveReviveValue = (int)values[0];
		if (!base.UseCard()) return false;
		return false;
	}
}
