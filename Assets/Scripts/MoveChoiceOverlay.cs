using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChoiceOverlay : MonoBehaviour
{
	public RectTransform panelRectTransform;

	public event System.Action<string> buttonClicked;

	public bool hasClicked = true;

	public void showMoveChoiceOverlay()
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

		buttonClicked?.Invoke("MM");
	}

	public void attackMoveButton()
	{
		print("attack move");
		hideMoveChoiceOverlay();

		buttonClicked?.Invoke("AM");
	}

	public void moveAttackButton()
	{
		print("move attack");
		hideMoveChoiceOverlay();

		buttonClicked?.Invoke("MA");
	}
}