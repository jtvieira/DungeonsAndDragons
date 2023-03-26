using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterInfoOverlay : MonoBehaviour
{
	public RectTransform panelRectTransform;
	public TextMeshProUGUI characterChoice;
	public TextMeshProUGUI health;

	// This ensures the RectTransform object is not null when the class instantiates
	private void Awake()
	{
		if (panelRectTransform == null)
		{
			panelRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
		}
	}

	public void setCharacterChoice(string playerId)
	{
		this.characterChoice.text = "Selected: " + playerId;
	}

	public void setHealth(float health)
	{
		this.health.text = "Heatlh: " + health.ToString();
	}

	public void destroyOverlay()
	{	
		if (this.transform.parent.gameObject != null)
			Destroy(transform.parent.gameObject);
	}

	public void hideOverlay() {
		if(this.gameObject.activeSelf){
			this.gameObject.SetActive(false);
		} else {
			this.gameObject.SetActive(true);
		}
	}
}
