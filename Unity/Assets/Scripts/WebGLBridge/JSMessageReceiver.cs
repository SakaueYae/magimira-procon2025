using System;
using System.Globalization;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;

namespace WebGLBridge
{
    /// <summary>
    /// JavaScriptからのメッセージを受信するためのクラス
    /// </summary>
    public class JSMessageReceiver : MonoBehaviour
    {
        // Wordを保持
        ReactiveProperty<string> _wordStream = new ReactiveProperty<string>();
        public ReadOnlyReactiveProperty<string> WordStream() => _wordStream;

        // Phraseを保持
        ReactiveProperty<string> _phraseStream = new ReactiveProperty<string>();
        public ReadOnlyReactiveProperty<string> PhraseStream() => _phraseStream;

        // 音楽の再生状態を保持
        ReactiveProperty<bool> _isPlaying = new ReactiveProperty<bool>();
        public ReadOnlyReactiveProperty<bool> IsPlaying => _isPlaying;

        // Wordsのカウントを保持(未実装)
        ReactiveProperty<int> _wordsCount = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> WordsCount => _wordsCount;

        // 音楽のセグメントが変化したときに通知する
        Subject<Unit> _onSegmentChangeStream = new Subject<Unit>();
        public Observable<Unit> OnSegmentChangeStream() => _onSegmentChangeStream;

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

            UniTask.Void(async () =>
            {
                while (true)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(10));
                    OnSegmentChange();
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

        public void OnSegmentChange()
        {
            _onSegmentChangeStream.OnNext(Unit.Default);
        }
    }
}
