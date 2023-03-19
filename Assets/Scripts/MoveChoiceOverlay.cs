using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChoiceOverlay : MonoBehaviour
{
	public RectTransform panelRectTransform;

	private string selection = "";
	private bool buttonClicked = false;

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
		this.panelRectTransform.gameObject.SetActive(false);
	}

	public void moveMoveButton()
	{
		print("move move");
		this.selection = "MM";
		this.buttonClicked = true;
		hideMoveChoiceOverlay();
		print(this.GetInstanceID());
	}

	public void attackMoveButton()
	{
		print("attack move");
		this.selection = "AM";
		this.buttonClicked = true;
		hideMoveChoiceOverlay();
	}

	public void moveAttackButton()
	{
		print("move attack");
		this.selection = "MA";
		this.buttonClicked = true;
		hideMoveChoiceOverlay();
	}

	public string getSelection()
	{
		return selection;
	}

	public bool isButtonClicked()
	{
		return buttonClicked;
	}

	public void destroyOverlay()
	{
		Destroy(gameObject);
	}
}