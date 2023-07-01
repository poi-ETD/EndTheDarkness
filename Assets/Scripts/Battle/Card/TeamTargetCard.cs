using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamTargetCard : RenewalCard
{
	public override bool UseCard()  //우리팀을 고르쟈
	{
		if (!base.UseCard()) return false;
		if (BM.selectedCharacter == null)
		{
			BM.WarnOn("카드의 효과를 받을 아군을 선택해주세요.");
			return false;
		}

		if (BM.selectedCharacter.isDie)
		{
			BM.WarnOn("이미 죽은 아군은 선택할 수 없습니다.");
			return false;
		}
		return true;
	}
}
