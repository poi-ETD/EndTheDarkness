using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveChoiceCard : RenewalCard
{
	public override bool UseCard()
	{
		BM.ReviveToField(graveReviveValue,this);
		return true;
	}
}
