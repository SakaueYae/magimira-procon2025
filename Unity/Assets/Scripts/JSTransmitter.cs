using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;

namespace GameData
{
    public class JSTransmitter : MonoBehaviour
    {
        ReactiveProperty<string> _wordStream = new ReactiveProperty<string>();
        public ReadOnlyReactiveProperty<string> WordStream()=> _wordStream;

        // Test
        // void Start()
        // {
        //     // Simulate receiving words every 2 seconds
        //     UniTask.Void(async () =>
        //     {
        //         while (true)
        //         {
        //             await UniTask.Delay(TimeSpan.FromSeconds(2));
        //             OnWord("Test Word " + DateTime.Now.ToString("HH:mm:ss"));
        //         }
        //     });
        // }

        public void OnWord(string word)
        {
            _wordStream.Value = word;
            Debug.Log("Word received: " + word);
        }
    }
}
