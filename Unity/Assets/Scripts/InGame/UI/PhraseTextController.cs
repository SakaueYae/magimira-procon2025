using TMPro;
using UnityEngine;

namespace InGame.UI
{
    public class PhraseTextController : MonoBehaviour
    {
        TextMeshProUGUI _phraseText;

        void Start()
        {
            _phraseText = GetComponent<TextMeshProUGUI>();
        }

        public void UpdatePhraseText(string phrase)
        {
            _phraseText.text = phrase;
        }
    }
}
