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
		this.selection = "movemove";
		this.buttonClicked = true;
		hideMoveChoiceOverlay();
	}

	public void attackMoveButton()
	{
		this.selection = "attackmove";
		this.buttonClicked = true;
		hideMoveChoiceOverlay();
	}

	public void moveAttackButton()
	{
		this.selection = "moveattack";
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