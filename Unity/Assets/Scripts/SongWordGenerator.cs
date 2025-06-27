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


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _transmitter.WordStream()
                .Subscribe(word =>
                {
                    if (word != null && word.Length > 0)
                    {
                        Vector3 _currentPos = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);

                        Debug.Log(_currentPos);
                        Debug.Log(word);
                        
                        SongWordController songWord = Instantiate(_wordControllerPrefab, _currentPos, Quaternion.identity);
                        // songWord.transform.localScale = Vector3.one; // Reset scale to 1
                        // songWord.transform.localPosition = _currentPos; // Set position to current mouse position
                        songWord.SetWord(word);
                    }
                })
                .AddTo(this);
        }
    }
}