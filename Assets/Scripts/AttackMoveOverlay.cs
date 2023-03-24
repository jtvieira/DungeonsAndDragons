using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AttackMoveOverlay : MonoBehaviour
{
	public RectTransform panelRectTransform;
	public TextMeshProUGUI AttackMoveResults;
	public TextMeshProUGUI TileInputLabel;
	public TextMeshProUGUI AttackResultsLabel;
	public TextMeshProUGUI EnterAttackLabel;
	public TextMeshProUGUI UserAttackResults;
	public TextMeshProUGUI SpellList;
	public TextMeshProUGUI SpellsAvaliable;
	public TextMeshProUGUI playerId;
	public TextMeshProUGUI SpellsLabel;
	public TMP_InputField spellInput;
	public TextMeshProUGUI badSpell;
	public Button inputButton;
	public Button closeOverlayButton;
	public Button spellButton;
	private bool button1Clicked = false;
	public TextMeshProUGUI badMove;
	public TMP_InputField tileInput;
	private string tileInputString;
	private string spellInputString = null;
	private bool button2Clicked = false;
	private bool button3Clicked = false;

	// This ensures the RectTransform object is not null when the class instantiates
	private void Awake()
	{
		badMove.enabled = false;
		badSpell.enabled = false;
		AttackMoveResults.enabled = false;
		UserAttackResults.enabled = false;
		tileInput.gameObject.SetActive(false);
		TileInputLabel.enabled = false;
		AttackResultsLabel.enabled = false;
		closeOverlayButton.gameObject.SetActive(false);
		inputButton.gameObject.SetActive(false);

		inputButton.onClick.AddListener(TaskOnClick);

		if (panelRectTransform == null)
		{
			panelRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
		}
	}

	private void showAttackMoveOverlay()
	{
		panelRectTransform.gameObject.SetActive(true);
	}

	public void hideAttackMoveOverlay()
	{
		panelRectTransform.gameObject.SetActive(false);
	}

	public void setPlayer(string newString)
	{
		this.playerId.text = newString;
	}

	public void setSpellList(string newString)
	{
		this.SpellList.text = newString;
	}

	public void setSpellsAvaliable(string newString)
	{
		this.SpellsAvaliable.text = newString;
	}

	public void attackMoveOkayButton()
	{
		this.button1Clicked = true;
	}

	public bool isButtonClicked()
	{
		return button1Clicked;
	}

	public void destroyOverlay()
	{
		Destroy(transform.parent.gameObject);
	}

	public void setAttackMoveResults(string newString)
	{
		this.AttackMoveResults.text = newString;
	}

	public void setAttackResults(string newString)
	{
		this.UserAttackResults.text = newString;
	}

	public void tileInputButton()
	{
	}

	void TaskOnClick()
	{
		this.tileInputString = tileInput.text.ToUpper();
		setTileInputString(this.tileInputString);
		this.button2Clicked = true;
	}

	public void spellInputButton()
	{
		this.spellInputString = spellInput.text.ToUpper();
		if (spellInputString != null)
		{
			this.button3Clicked = true;
		}
	}

	public void setTileInputString(String tile)
	{
		this.tileInputString = tile;
	}

	public string getTileInputString()
	{
		return tileInputString;
	}

	public bool isButton2Clicked()
	{
		return button2Clicked;
	}

	public void showBadMove()
	{
		this.badMove.enabled = true;
	}

	public void hideBadMove()
	{
		this.badMove.enabled = false;
	}

	public void showBadSpell()
	{
		this.badSpell.enabled = true;
	}

	public void showGoodSpell()
	{
		tileInput.gameObject.SetActive(true);
		TileInputLabel.enabled = true;
		spellInput.enabled = false;
		badMove.enabled = false;
		closeOverlayButton.gameObject.SetActive(true);
		inputButton.gameObject.SetActive(true);
		spellButton.gameObject.SetActive(false);
	}

	public void hideBadSpell()
	{
		this.badSpell.enabled = false;
	}

	public bool isButton3Clicked()
	{
		return button3Clicked;
	}

	public string getSpellInputString()
	{
		return spellInputString;
	}

	public void setValid()
    {
		badMove.enabled = false;
		AttackMoveResults.enabled = true;
		inputButton.gameObject.SetActive(false);
		tileInput.gameObject.SetActive(false);
		EnterAttackLabel.enabled = false;
		UserAttackResults.enabled = true;
		AttackResultsLabel.enabled = true;
	}

	public void setinValid()
	{
		badMove.enabled = true;
		inputButton.gameObject.SetActive(true);
		tileInput.gameObject.SetActive(true);
		this.button2Clicked = false;
	}
}
