using UnityEngine;
using UnityEngine.UI;

public class TileInputOverlay : MonoBehaviour
{
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private Button closeButton;
    [SerializeField] private InputField inputField;


    private void Start()
    {
        ShowMovePanel();
        // Add listener to the close button
        closeButton.onClick.AddListener(CloseOverlay);
    }

    private void CloseOverlay()
    {
        // Destroy the overlay game object
        Destroy(gameObject);
    }

    public void ShowMovePanel()
    {
        panelRectTransform.gameObject.SetActive(true);
    }

    // Set the RectTransform object as not visible
    public void HideMovePanel()
    {
        panelRectTransform.gameObject.SetActive(false);
    }

    // Get the text from the input field
    public string GetInputText()
    {
        return inputField.text;
    }
}
