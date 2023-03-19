using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChoiceOverlay : MonoBehaviour
{
	public RectTransform panelRectTransform;

	public event System.Action<string> buttonClicked;

	public bool hasClicked = true;

	// This ensures the RectTransform object is not null when the class instantiates
	private void Awake()
	{
		if (panelRectTransform == null)
		{
			panelRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
		}
	}

	public void showMoveChoiceOverlay()
	{
		this.panelRectTransform.gameObject.SetActive(true);
	}

	public void hideMoveChoiceOverlay()
	{
		print(this.panelRectTransform == null);
		this.panelRectTransform.gameObject.SetActive(false);
	}

	public void moveMoveButton()
	{
		hideMoveChoiceOverlay();
		print(this.panelRectTransform == null);

		buttonClicked?.Invoke("MM");
	}

	public void attackMoveButton()
	{
		print("attack move");
		// hideMoveChoiceOverlay();

		buttonClicked?.Invoke("AM");
	}

	public void moveAttackButton()
	{
		print("move attack");
		// hideMoveChoiceOverlay();

		buttonClicked?.Invoke("MA");
	}
}