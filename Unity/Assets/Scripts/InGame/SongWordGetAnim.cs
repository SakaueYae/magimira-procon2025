using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    public class SongWordGetAnim : MonoBehaviour
    {
        [SerializeField] Canvas _canvas;

        public void PlayWordGetAnim()
        {
            _canvas.gameObject.transform.DOScale(Vector3.zero, 0.5f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }
    }
}

