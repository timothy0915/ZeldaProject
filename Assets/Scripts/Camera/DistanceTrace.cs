using UnityEngine;

public class DistanceTrace : MonoBehaviour
{
    public Transform CamPoint;  // 鏡頭跟隨點
    public Transform player;    // 玩家
    public float moveSpeed = 4f;
    public float followThreshold = 2f;  // 超過這個距離才開始移動
    public float stopThreshold = 4f;    // 接近到這個距離後停止移動
    public float lerpSpeed = 1.2f;        // 用於平滑移動
    private bool ifOut = false;         // 是否超出範圍
    
    void LateUpdate()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, CamPoint.position);

        if (distance > followThreshold)
            ifOut = true;

        if (ifOut)
        {
            //Debug.Log(player.position.y);
            //CamPoint.position = player.position;//直接移動
            // 平滑移動至玩家位置
            CamPoint.position = Vector3.Lerp(CamPoint.position, player.position, Mathf.Clamp01(lerpSpeed * Time.deltaTime));

            
            // 讓鏡頭只對齊 `Y` 軸，避免多餘震動
            //CamPoint.position = new Vector3(CamPoint.position.x, player.position.y, CamPoint.position.z);

            // 讓鏡頭面向 `Player`
            //CamPoint.LookAt(player.position);

            // 若距離小於 `stopThreshold` 則停止移動
            if (distance < stopThreshold)
                ifOut = false;
        }
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawSphere(CamPoint.position, 1);
    }
}
