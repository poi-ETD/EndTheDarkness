using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetCard : RenewalCard
{
	protected Enemy targetEnemy;

	public override bool UseCard()  //적을 고르쟈
	{
		if (!base.UseCard()) return false;

		//흉귀의 첫번째 패시브가 on이라면?
		if (BM.actCharacter.passive[0] > 0 && BM.actCharacter.characterNo == 6)
		{
			GameObject randomEnemy = BM.Enemys[Random.Range(0, BM.Enemys.Length)];
			while (randomEnemy.GetComponent<Enemy>().isDie)
			{
				randomEnemy = BM.Enemys[Random.Range(0, BM.Enemys.Length)];
			}
			BM.enemySelectMode = false;
			targetEnemy = randomEnemy.GetComponent<Enemy>();
		}
		else if (BM.ei.SelectedEnemy != null)
		{
			if (BM.prvokingEnemy != null && BM.prvokingEnemy != BM.ei.SelectedEnemy)
			{
				//도발중인 다른 몬스터가 있다!!
				BM.WarnOn("다른 적이 가로막고 있습니다.");
				return false;
			}
			if(BM.ei.SelectedEnemy.Shadow)
			{
				BM.WarnOn("해당 적이 은신중입니다.");
				return false;
			}
			BM.enemySelectMode = false;
			targetEnemy = BM.ei.SelectedEnemy;
		}
		else
		{
			//적 지정이 안 된 상태이다.
			return false;
		}
		return true;
	}
}
