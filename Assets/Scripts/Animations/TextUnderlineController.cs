using UnityEngine;
using TMPro;
public class TextUnderlineController : MonoBehaviour
{
    public TMP_Text buttonText;
    private string originalText;

    private void Start()
    {
        originalText = buttonText.text;
    }

    public void AddUnderLine()
    {
        buttonText.text = $"<u>{originalText}<u>";
    }

    public void RemoveUnderLine()
    {
        buttonText.text = originalText;
    }
}
