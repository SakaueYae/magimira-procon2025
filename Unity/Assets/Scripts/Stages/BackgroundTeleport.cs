using R3.Triggers;
using UnityEngine;
using R3;

namespace Stages
{
    public class BackgroundTeleport : MonoBehaviour
    {
        [SerializeField] float _moveSpeed = 1f; // 背景の移動速度
        [SerializeField] float ADJUSTMENT_FACTOR = -2f; // 背景の調整係数
        float _backgroundWidth; // 背景の幅

        void Start()
        {
            // 背景の幅を取得
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            _backgroundWidth = spriteRenderer.bounds.size.x;

            this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                // 背景を左方向に移動
                Vector3 position = transform.position;
                position.x -= _moveSpeed * Time.deltaTime; // 毎フレーム移動
                transform.position = position;

                // 画面外判定（ゲーム画面の左端の座標-背景画像の大きさ）
                if (position.x < -Camera.main.orthographicSize * Camera.main.aspect - _backgroundWidth / 2)
                {
                    // 背景の長さの2倍分+指定した調整分右に移動
                    // x=0に背景画像を置くとなぜか中心からずれてしまったため、調整係数を追加
                    position.x += _backgroundWidth * 2f + ADJUSTMENT_FACTOR;
                    transform.position = position;
                }
            })
            .AddTo(this);
        }
    }
}
