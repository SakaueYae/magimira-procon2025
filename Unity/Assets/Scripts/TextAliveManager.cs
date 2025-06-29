using System;
using System.Globalization;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;

namespace GameData
{
    public class TextAliveManager : MonoBehaviour
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
            //     }
            // });

            OnBeat("511.29999999999995");
        }

        public void OnWord(string word)
        {
            _wordStream.Value = word;
            Debug.Log("Word received: " + word);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intervalString">ms（文字列）</param>
        public void OnBeat(string intervalString)
        {
            Debug.Log("OnBeat called with: " + intervalString);
            float intervalValue;

            if (float.TryParse(intervalString, NumberStyles.Any, CultureInfo.InvariantCulture, out intervalValue))
            {
                Debug.Log("Interval parsed: " + intervalValue);
                _intervalStream.Value = intervalValue;
            }
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
