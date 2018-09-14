using UnityEngine;

//----------------------------------------------------
// 左右キーでY軸周りを秒速90度で回転させるサンプル
//----------------------------------------------------
public class AngleAxisSample : MonoBehaviour
{
    private void Update()
    {
        // 左右キー入力に応じて回転速度を決定
        float horizontalSpeed = 0.0f;
        float x = Input.GetAxisRaw("Horizontal");
        if (x < 0) { horizontalSpeed += -90.0f; }
        if (x > 0) { horizontalSpeed += 90.0f; }

        float verticalSpeed = 0.0f;
        float y = Input.GetAxisRaw("Vertical");
        if (y < 0) { verticalSpeed += -90.0f; }
        if (y > 0) { verticalSpeed += 90.0f; }

        // Y軸(Vector3.up)周りを１フレーム分の角度だけ回転させるQuaternionを作成
        Quaternion yAxisRot = Quaternion.AngleAxis(horizontalSpeed * Time.deltaTime, Vector3.up);
        Quaternion xAxisRot = Quaternion.AngleAxis(verticalSpeed * Time.deltaTime, Vector3.right);

        // 元の回転値と合成して上書き
        transform.rotation = yAxisRot * xAxisRot * transform.rotation;
//        transform.rotation = yAxisRot * transform.rotation;

    }
}