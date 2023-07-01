using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Card7 : GraveChoiceCard
{
	public override bool UseCard()
	{
		graveReviveValue = (int)values[1];
		if (!base.UseCard()) return false;
		return false;
	}
	public override void UseCardResult()
	{
		BM.GhostRevive((int)values[0]);
		base.UseCardResult();
	}
}
