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

            Vector3 currentPos = gameObject.transform.position;
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(currentPos);
            Vector3 generatedPos = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, Random.value, viewportPos.z));

            SongWordController songWord = Instantiate(_songWordControllerPrefab, generatedPos, Quaternion.identity);
            songWord.InitializeSongWord(word, 1);
        }
    }

}