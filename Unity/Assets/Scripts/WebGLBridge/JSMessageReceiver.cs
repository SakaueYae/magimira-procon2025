using System;
using System.Globalization;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;

namespace WebGLBridge
{
    public class JSMessageReceiver : MonoBehaviour
    {
        ReactiveProperty<string> _wordStream = new ReactiveProperty<string>();
        public ReadOnlyReactiveProperty<string> WordStream() => _wordStream;

        ReactiveProperty<string> _phraseStream = new ReactiveProperty<string>();
        public ReadOnlyReactiveProperty<string> PhraseStream() => _phraseStream;

        ReactiveProperty<bool> _isPlaying = new ReactiveProperty<bool>();
        public ReadOnlyReactiveProperty<bool> IsPlaying => _isPlaying;

        ReactiveProperty<int> _wordsCount = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> WordsCount => _wordsCount;

        // Test
        void Start()
        {
            // Simulate receiving words every 2 seconds
#if UNITY_EDITOR
            UniTask.Void(async () =>
            {
                while (true)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(2));
                    OnWord("Test Word " + DateTime.Now.ToString("HH:mm:ss"));
                    await UniTask.Delay(TimeSpan.FromSeconds(2));
                    OnWord("A");
                }
            });

            UniTask.Void(async () =>
            {
                while (true)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(3));
                    OnPhrase("Test Phrase " + DateTime.Now.ToString("HH:mm:ss"));
                }
            });
#endif
        }

        public void OnWord(string word)
        {
            // 同じ値が二連続で代入されるのを防止
            if (_wordStream.Value != word)
                _wordStream.Value = word;
        }

        public void OnPhrase(string phrase)
        {
            // 同じ値が二連続で代入されるのを防止
            if (_phraseStream.Value != phrase)
                _phraseStream.Value = phrase;
        }

        public void OnPlay()
        {
            _isPlaying.Value = true;
        }

        public void OnPause()
        {
            _isPlaying.Value = false;
        }

        public void GetWordsCount(string word)
        {
            _wordsCount.Value = int.TryParse(word, NumberStyles.Integer, CultureInfo.InvariantCulture, out var count) ? count : 0;
        }
    }
}
