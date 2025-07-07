using R3.Triggers;
using UnityEngine;
using R3;
using System.Threading.Tasks;

namespace InGame
{
    public class VerticalPlatform : MonoBehaviour
    {
        PlatformEffector2D _effector;
        [SerializeField] float _waitTime = 0.5f; // 待機時間

        Vector3 _startPos;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _effector = GetComponent<PlatformEffector2D>();

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0)) // マウスの左ボタンが押されたとき
                .Where(_ => Time.timeScale > 0)
                .Subscribe(_ =>
                {
                    _startPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0)) // マウスの左ボタンが離されたとき
                .SubscribeAwait(async (_, ct) =>
                {
                    Vector3 endPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
                    float distance = Vector3.Distance(_startPos, endPos);

                    // Y軸方向の移動量を計算（下方向が負の値）
                    float deltaY = endPos.y - _startPos.y;

                    if (deltaY < -30f) // 50ピクセル以上移動し、かつ30ピクセル以上下方向にフリック
                    {
                        _effector.rotationalOffset = 180f; // プラットフォームを反転
                        Debug.Log("Down flick detected!");
                        await Task.Delay(System.TimeSpan.FromSeconds(_waitTime), cancellationToken: ct); // 待機時間を設ける
                    }
                    _effector.rotationalOffset = 0f; // 待機後に元に戻す
                }, AwaitOperation.Sequential)
                .AddTo(this);
        }

        /// <summary>
        /// フリックの方向を判定する
        /// </summary>
        private bool IsDownwardFlick(Vector3 startPos, Vector3 endPos, float minDistance = 50f, float minVerticalDistance = 30f)
        {
            float deltaY = endPos.y - startPos.y;
            float totalDistance = Vector3.Distance(startPos, endPos);

            // 総移動距離が十分で、かつ下方向への移動が十分な場合
            return totalDistance > minDistance && deltaY < -minVerticalDistance;
        }

    }

}