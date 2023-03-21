using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileInputOverlay : MonoBehaviour
{
	public RectTransform panelRectTransform;
	public TMP_InputField tileInput;
	private string tileInputString;
	private bool buttonClicked = false;

	// This ensures the RectTransform object is not null when the class instantiates
	private void Awake()
	{
		if (panelRectTransform == null)
		{
			panelRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
		}
	}

	private void showTileInputOverlay()
	{
		panelRectTransform.gameObject.SetActive(true);
	}

	public void hideTileInputOverlay()
	{
		panelRectTransform.gameObject.SetActive(false);
	}

	public void tileInputButton()
	{
		this.tileInputString = tileInput.text;
		if (tileInputString != null)
		{
			this.buttonClicked = true;
		}
	}

	public string getTileInputString()
	{
		return tileInputString;
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
