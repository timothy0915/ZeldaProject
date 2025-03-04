using UnityEngine;

public class DistanceTrace : MonoBehaviour
{
    public Transform CamPoint;  // ���Y���H�I
    public Transform player;    // ���a
    public float moveSpeed = 4f;
    public float followThreshold = 2f;  // �W�L�o�ӶZ���~�}�l����
    public float stopThreshold = 4f;    // �����o�ӶZ���ᰱ���
    public float lerpSpeed = 1.2f;        // �Ω󥭷Ʋ���
    private bool ifOut = false;         // �O�_�W�X�d��
    
    void LateUpdate()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, CamPoint.position);

        if (distance > followThreshold)
            ifOut = true;

        if (ifOut)
        {
            //Debug.Log(player.position.y);
            //CamPoint.position = player.position;//��������
            // ���Ʋ��ʦܪ��a��m
            CamPoint.position = Vector3.Lerp(CamPoint.position, player.position, Mathf.Clamp01(lerpSpeed * Time.deltaTime));

            
            // �����Y�u��� `Y` �b�A�קK�h�l�_��
            //CamPoint.position = new Vector3(CamPoint.position.x, player.position.y, CamPoint.position.z);

            // �����Y���V `Player`
            //CamPoint.LookAt(player.position);

            // �Y�Z���p�� `stopThreshold` �h�����
            if (distance < stopThreshold)
                ifOut = false;
        }
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawSphere(CamPoint.position, 1);
    }
}
