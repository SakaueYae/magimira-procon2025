using TMPro;
using UnityEngine;

public class JSTransmitter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshProUGUI;

    public void OnWord(string word)
    {
        _textMeshProUGUI.text = word;
    }
}
