using R3.Triggers;
using UnityEngine;
using R3;

namespace Stages
{
    public class BackgroundTeleport : MonoBehaviour
    {
        [SerializeField] float ADJUSTMENT_FACTOR = -2f; // 背景の調整係数
        float _backgroundWidth;

        void Start()
        {
            // 背景の幅を取得
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            _backgroundWidth = spriteRenderer.bounds.size.x;

            this.OnBecameInvisibleAsObservable()
            .Subscribe(_ =>
            {
                Vector3 position = transform.position;
                position.x += _backgroundWidth * 2f + ADJUSTMENT_FACTOR;
                transform.position = position;
            })
            .AddTo(this);
        }
    }
}
