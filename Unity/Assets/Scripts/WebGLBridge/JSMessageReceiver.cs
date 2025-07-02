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

        ReactiveProperty<float> _intervalStream = new ReactiveProperty<float>();
        public ReadOnlyReactiveProperty<float> BeatStream() => _intervalStream;

        ReactiveProperty<bool> _isPlaying = new ReactiveProperty<bool>();
        public ReadOnlyReactiveProperty<bool> IsPlaying => _isPlaying;

        // Test
        void Start()
        {
            // Simulate receiving words every 2 seconds
            // UniTask.Void(async () =>
            // {
            //     while (true)
            //     {
            //         await UniTask.Delay(TimeSpan.FromSeconds(2));
            //         OnWord("Test Word " + DateTime.Now.ToString("HH:mm:ss"));
            //         await UniTask.Delay(TimeSpan.FromSeconds(2));
            //         OnWord("A");
            //     }
            // });
        }

        public void OnWord(string word)
        {
            // 同じ値が二連続で代入されるのを防止
            if (_wordStream.Value != word)
                _wordStream.Value = word;
            Debug.Log("Word received: " + word);
        }

        public void OnPlay()
        {
            _isPlaying.Value = true;
        }

        public void OnPause()
        {
            _isPlaying.Value = false;
        }
    }
}
