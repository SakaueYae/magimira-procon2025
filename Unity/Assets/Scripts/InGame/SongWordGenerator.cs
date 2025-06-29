using UnityEngine;

namespace InGame
{
    public class SongWordGenerator : MonoBehaviour
    {
        [SerializeField] SongWordController _songWordControllerPrefab;

        Rigidbody2D _rb2;

        void Start()
        {
            _rb2 = gameObject.GetComponent<Rigidbody2D>();
        }

        public void GenerateSongWord(string word)
        {
            // 進行方向と逆の方向に生成
            int dir = _rb2.linearVelocityX > 0 ? -1 : 1;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            float halfWidth = rectTransform.rect.width * rectTransform.localScale.x / 2f;

            // ボールのちょうど後ろに座標を指定
            Vector3 pos = new Vector3(
                gameObject.transform.position.x + dir * halfWidth,
                gameObject.transform.position.y,
                0f
            );

            SongWordController songWord = Instantiate(_songWordControllerPrefab, pos, Quaternion.identity);
            songWord.InitializeSongWord(word, dir);
        }
    }

}