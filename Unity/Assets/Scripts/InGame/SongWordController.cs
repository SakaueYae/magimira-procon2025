using Cysharp.Threading.Tasks;
using R3.Triggers;
using TMPro;
using UnityEngine;
using R3;

namespace InGame
{
    public class SongWordController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] float _moveSpeed = 1f; // テキストの移動速度

        RectTransform _rectTransform;
        BoxCollider2D _collider2d;

        void Awake()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _collider2d = gameObject.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                // テキストを左方向に移動
                Vector3 position = _rectTransform.position;
                position.x -= _moveSpeed * Time.deltaTime; // 毎フレーム移動
                _rectTransform.position = position;

                // 画面外判定（左端より左に出た場合）
                if (position.x < -_rectTransform.sizeDelta.x)
                {
                    Destroy(gameObject);
                }
            })
            .AddTo(this);
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

            // Colliderのサイズを更新
            if (_collider2d != null)
            {
                _collider2d.size = textSize;
            }
        }
    }
}
