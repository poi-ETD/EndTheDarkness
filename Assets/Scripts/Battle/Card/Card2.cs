using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Card2 : TeamTargetCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;
		BM.getArmor((int)values[0], BM.selectedCharacter);

		return true;
	}
}
