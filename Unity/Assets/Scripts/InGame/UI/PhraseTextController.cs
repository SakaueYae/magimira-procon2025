using TMPro;
using UnityEngine;

namespace InGame.UI
{
    public class PhraseTextController : MonoBehaviour
    {
        TextMeshProUGUI _phraseText;

        // UpdatePhraseTextが初期化前に呼ばれてしまうため、Awakeで対応
        void Awake()
        {
            _phraseText = GetComponent<TextMeshProUGUI>();
        }

        public void UpdatePhraseText(string phrase)
        {
            _phraseText.text = phrase;
        }
    }
}
