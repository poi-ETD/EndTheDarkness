using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card12 : RenewalCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;
		BM.card12();
		//조금 늦게 반응해주자
		return false;
	}
}
