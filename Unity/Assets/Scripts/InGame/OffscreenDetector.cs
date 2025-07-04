using R3.Triggers;
using UnityEngine;
using R3;

/// <summary>
/// オブジェクトがカメラの画面外に出たことを検出し、どの端から出たかを通知するコンポーネント
/// </summary>
public class OffscreenDetector : MonoBehaviour
{
    // 画面の端を明確に表すためのenum（列挙型）
    public enum ScreenEdge { None, Left, Right, Top, Bottom }

    // 直前のフレームで画面内にいたかどうかを記録するフラグ
    bool _wasOnScreen = false;
    Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;

        // 初期状態をチェックしておく
        CheckIfOnScreen();

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                CheckIfOnScreen();
            })
            .AddTo(this);
    }

    /// <summary>
    /// オブジェクトが画面内にいるか、外に出たかをチェックする
    /// </summary>
    private void CheckIfOnScreen()
    {
        if (_mainCamera == null) return;

        // オブジェクトのワールド座標をカメラのビューポート座標に変換する
        Vector3 viewportPos = _mainCamera.WorldToViewportPoint(transform.position);

        // 現在、画面内にいるかどうかを判定 (X, Y共に0から1の範囲内か)
        bool isOnScreenNow = (viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1);

        // 「直前は画面内にいた」が「今は画面外にいる」という"出た瞬間"を検出
        if (_wasOnScreen && !isOnScreenNow)
        {
            // どの端から出たかを判定
            ScreenEdge edge = GetExitedEdge(viewportPos);

            TeleportToOppositeSide(edge);
        }

        // 現在の状態を次フレームのために保存
        _wasOnScreen = isOnScreenNow;
    }

    /// <summary>
    /// ビューポート座標を元に、どの端から出たかを判定する
    /// </summary>
    private ScreenEdge GetExitedEdge(Vector3 viewportPos)
    {
        if (viewportPos.x < 0) return ScreenEdge.Left;
        if (viewportPos.x > 1) return ScreenEdge.Right;
        if (viewportPos.y < 0) return ScreenEdge.Bottom;
        if (viewportPos.y > 1) return ScreenEdge.Top;

        return ScreenEdge.None;
    }

    /// <summary>
    /// オブジェクトを反対へテレポートさせる
    /// </summary>
    private void TeleportToOppositeSide(ScreenEdge edge)
    {
        if (_mainCamera == null) return;

        // 現在のワールド座標を取得
        Vector3 currentWorldPos = transform.position;

        // 現在の位置をビューポート座標に変換
        Vector3 currentViewportPos = _mainCamera.WorldToViewportPoint(currentWorldPos);

        switch (edge)
        {
            case ScreenEdge.Left:
                currentViewportPos.x = 1f;
                break;
            case ScreenEdge.Right:
                currentViewportPos.x = 0f;
                break;
            case ScreenEdge.Top:
                break;
            case ScreenEdge.Bottom:
                currentViewportPos.y = 1f;
                break;
        }

        // ビューポート座標をワールド座標に変換して適用
        Vector3 rightEdgeWorldPos = _mainCamera.ViewportToWorldPoint(currentViewportPos);
        transform.position = rightEdgeWorldPos;
    }
}
