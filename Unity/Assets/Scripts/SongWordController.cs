using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame
{
    public class SongWordController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _text;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            DelayAsync(destroyCancellationToken).Forget();
        }

        // 非同期メソッド
        private async UniTask DelayAsync(CancellationToken token)
        {
            // 3秒間待つ
            await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: token);

            // 3秒後に原点にワープ
            Destroy(gameObject);
        }

        public void SetWord(string word)
        {
            _text.text = word;
        }
    }
}
