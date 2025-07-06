using Cysharp.Threading.Tasks;
using R3.Triggers;
using TMPro;
using UnityEngine;
using R3;

namespace InGame
{
    public interface ISongWord
    {
        void InitializeSongWord(string word, int dir);
        void GetWord();
    }
    public class SongWordController : MonoBehaviour, ISongWord
    {
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] float _moveSpeed = 1f; // テキストの移動速度

        RectTransform _rectTransform;
        BoxCollider2D _collider2d;
        float _textWidth;

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
                gameObject.transform.Translate(new Vector3(-_moveSpeed * Time.deltaTime, 0, 0));
            })
            .AddTo(this);

            this.OnBecameInvisibleAsObservable()
            .Subscribe(_ =>
            {
                Destroy(gameObject);
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

            _textWidth = textSize.x;
        }

        public void GetWord()
        {
            // TODO: 取得時のアニメーション、オブジェクトの削除
            Destroy(gameObject);
        }
    }
}
