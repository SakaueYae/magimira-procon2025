using UnityEngine;

namespace InGame
{
    public class SongWordGenerator : MonoBehaviour
    {
        [SerializeField] SongWordController _songWordControllerPrefab;
        [SerializeField] float _rotationAngle = 30f; // 生成時の角度範囲

        Rigidbody2D _rb2;

        void Start()
        {
            _rb2 = gameObject.GetComponent<Rigidbody2D>();
        }

        public void GenerateSongWord(string word)
        {

            Vector3 currentPos = gameObject.transform.position;
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(currentPos);
            float y = Random.value;
            Quaternion rotation = Quaternion.Euler(0, 0, 0);

            if (y < 0.5f)
            {
                rotation = Quaternion.Euler(0, 0, Random.Range((-1) * _rotationAngle, 0));
            }
            else
            {
                rotation = Quaternion.Euler(0, 0, Random.Range(0, _rotationAngle));
            }

            Vector3 generatedPos = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, y, viewportPos.z));

            SongWordController songWord = Instantiate(_songWordControllerPrefab, generatedPos, rotation);
            songWord.InitializeSongWord(word, 1);
        }
    }

}