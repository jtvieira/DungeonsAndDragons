using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AiMoveOverlay : MonoBehaviour
{
	public RectTransform panelRectTransform;
	public TextMeshProUGUI AiMoveResults;
	public TextMeshProUGUI AiMoveLocation;
	public TextMeshProUGUI AiAttackResults;
	private bool buttonClicked = false;

	// This ensures the RectTransform object is not null when the class instantiates
	private void Awake()
	{
		if (panelRectTransform == null)
		{
			panelRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
		}
	}

	private void showAiMoveOverlay()
	{
		panelRectTransform.gameObject.SetActive(true);
	}

	public void hideAiMoveOverlay()
	{
		panelRectTransform.gameObject.SetActive(false);
	}

	public void aiMoveOkayButton()
	{
		this.buttonClicked = true;
	}

	public bool isButtonClicked()
	{
		return buttonClicked;
	}

	public void destroyOverlay()
	{
		Destroy(transform.parent.gameObject);
	}

	public void setAiMoveResults(string newString)
	{
		this.AiMoveResults.text = newString;
	}

	public void setAiMoveLocation(string newString)
	{
		this.AiMoveLocation.text = newString;
	}

	public void setAiAttackResults(string newString)
	{
		this.AiAttackResults.text = newString;
	}
}
