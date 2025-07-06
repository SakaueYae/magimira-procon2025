using UnityEngine;
using R3;
using WebGLBridge;
using InGame.UI;

namespace InGame
{
    public class DataTransmitter : MonoBehaviour
    {
        [SerializeField] JSMessageReceiver _jsMessageReceiver;

        [SerializeField] SongWordGenerator _songWordGenerator;
        [SerializeField] PhraseTextController _phraseTextController;
        [SerializeField] GameObject _ballController;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _jsMessageReceiver.WordStream()
                .Subscribe(word =>
                {
                    _songWordGenerator.GenerateSongWord(word);
                })
                .AddTo(this);

            _jsMessageReceiver.PhraseStream()
                .Subscribe(phrase =>
                {
                    _phraseTextController.UpdatePhraseText(phrase);
                })
                .AddTo(this);

            _jsMessageReceiver.IsPlaying
                .Subscribe((isPlaying) =>
                {
                    _ballController.GetComponent<IBounce>().HandleBounce(isPlaying);
                })
                .AddTo(this);
        }
    }
}
