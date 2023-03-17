using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChoiceOverlay : MonoBehaviour
{
	public RectTransform panelRectTransform;

	private void showMoveChoiceOverlay()
	{
		panelRectTransform.gameObject.SetActive(true);
	}

	public void hideMoveChoiceOverlay()
	{
		panelRectTransform.gameObject.SetActive(false);
	}

	public void moveMoveButton()
	{
		print("move move");
		hideMoveChoiceOverlay();
	}

	public void attackMoveButton()
	{
		print("attack move");
		hideMoveChoiceOverlay();
	}

	public void moveAttackButton()
	{
		print("move attack");
		hideMoveChoiceOverlay();
	}
}
