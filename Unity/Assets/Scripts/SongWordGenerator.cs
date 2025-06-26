using UnityEngine;
using R3.Triggers;
using R3;
using GameData;
using UnityEngine.InputSystem;

namespace InGame
{
    public class SongWordGenerator : MonoBehaviour
    {
        [SerializeField] JSTransmitter _transmitter;
        [SerializeField] SongWordController _wordControllerPrefab;
        [SerializeField] Canvas _canvas;

        Vector3 _currentPos = Vector3.zero;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    Vector3 pos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _currentPos = pos2;
                })
                .AddTo(this);

            _transmitter.WordStream()
                .Subscribe(word =>
                {
                    if (word != null && word.Length > 0)
                    {
                        Debug.Log(_currentPos);
                        Debug.Log(word);
                        SongWordController songWord = Instantiate(_wordControllerPrefab, _currentPos, Quaternion.identity);
                        songWord.transform.SetParent(_canvas.gameObject.transform, false);
                        // songWord.transform.localScale = Vector3.one; // Reset scale to 1
                        // songWord.transform.localPosition = _currentPos; // Set position to current mouse position
                        songWord.SetWord(word);
                    }
                })
                .AddTo(this);
        }
    }
}