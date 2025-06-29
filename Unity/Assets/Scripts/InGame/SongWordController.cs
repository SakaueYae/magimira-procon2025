using System;
using System.Threading;
using System.Xml.Serialization;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame
{
    public class SongWordController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _text;
        // テキストの周囲に持たせる余白（パディング）
        [SerializeField]
        private Vector2 padding = new Vector2(30f, 20f);
        RectTransform _rectTransform;
        BoxCollider2D _collider2d;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _collider2d = gameObject.GetComponent<BoxCollider2D>();
            Debug.Log(_rectTransform);
            DelayAsync(destroyCancellationToken).Forget();
        }

        // 非同期メソッド
        private async UniTask DelayAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);

            Destroy(gameObject);
        }

        public void SetWord(string word)
        {
            _text.text = word;
        }

        public void InitializeSongWord(string word, int dir)
        {
            _text.text = word;

            // TextMeshProにメッシュを強制的に再計算させる
            // これを行わないと、テキストサイズが1フレーム前の古い情報のままになることがある
            _text.ForceMeshUpdate();

            // 描画されたテキストの実際のサイズを取得
            Vector2 textSize = _text.GetRenderedValues(false);

            // 親オブジェクトのサイズを更新
            _rectTransform.sizeDelta = textSize;

            gameObject.transform.position = new Vector3(
                // ボールにあたってしまうタイミングがあったため、0.2fのオフセットを追加
                gameObject.transform.position.x + dir * (textSize.x / 2f + 0.2f),
                gameObject.transform.position.y,
                gameObject.transform.position.z
            );

            // Colliderのサイズを更新
            if (_collider2d != null)
            {
                _collider2d.size = textSize;
            }
        }
    }
}
