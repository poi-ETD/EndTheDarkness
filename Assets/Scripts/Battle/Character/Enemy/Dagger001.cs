using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Dagger001 : MonoBehaviour
{
	public TurnManager TM;
	public int curTurn;
	public Enemy myEnemy;
	public BattleManager BM;
	public List<Character> HaveArmor = new List<Character>();
	public List<Character> ForwardHaveArmor = new List<Character>();
	[SerializeField] Text t;
	[SerializeField] int plusname;
	[SerializeField] Dagger001 anotherDagger;
	public int pattern;
	bool[] done = new bool[3];
	int myturn;

	// YH
	public Image image_character;
	public Sprite sprite_idle;
	public Sprite sprite_highlight;

	[SerializeField] TextMeshProUGUI NameT;

	private void Start()
	{
		TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
		BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
		myEnemy.Name = "단검" + plusname;
		NameT.text = myEnemy.Name;

	}
	private void Update()
	{
		if (myEnemy.isAct)
		{
			StartPattern();
			myEnemy.isAct = false;
		}
	}
	void StartPattern()
	{
		if (BM.teamDieCount < BM.characters.Count)
		{
			if (!myEnemy.isDie)
			{
				//     myturn++;
				//     if (myturn%3!=0)
				//       {
				//           pattern = Random.Range(0, 2);
				//           if (plusname == 2)
				//           {
				//               while (pattern == anotherDagger.pattern)
				//               {
				//                   pattern = Random.Range(0, 2);
				//               }
				//           }

				//           if (pattern == 0)
				//           {
				//             BM.EnemyAttack(10, myEnemy, BM.SelectCharacterInEnemyTurn(0, 0));
				//             BM.EnemyAttack(10, myEnemy, BM.SelectCharacterInEnemyTurn(0, 0));
				//         }
				//           if (pattern == 1)
				//           {
				//             BM.EnemyGetAromor(3, myEnemy, myEnemy);
				//             BM.EnemyAttack(10, myEnemy, BM.SelectCharacterInEnemyTurn(0, 0));
				//         }

				//       }
				//       else
				//       {
				//         BM.EnemyAttack(3, myEnemy, BM.SelectCharacterInEnemyTurn(0, 0));
				//         BM.EnemyAttack(3, myEnemy, BM.SelectCharacterInEnemyTurn(0, 0));
				//         BM.EnemyAttack(3, myEnemy, BM.SelectCharacterInEnemyTurn(0, 0));
				//         BM.EnemyAttack(1, myEnemy, BM.SelectCharacterInEnemyTurn(0, 1));
				//     }
				//   }
			}
			myEnemy.BM.AM.EnemyAct();

			// myEnemy.EnemyEndTurn();


		}
	}
}
