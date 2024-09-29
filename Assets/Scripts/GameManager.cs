using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timeText; // UI�� ���� ���� �ð��� ǥ���ϴ� �ؽ�Ʈ
    public float gameTime = 0.0f; // ���� ������ �ð��� �ʷ� ����
    public float timeScale = 60.0f; // ���� �ð� 1�ʴ� ���� �ð� 60�� (1��)

    void Update()
    {
        // ���� ������ ��� �ð��� ����Ͽ� ���� �ð� ����
        gameTime += Time.deltaTime * timeScale;

        // ���� �ð��� ��:�� �������� ��ȯ
        int hours = (int)(gameTime / 3600); // % 24�� �����Ͽ� 24 �̻� ���� ��� ����
        int minutes = (int)(gameTime / 60) % 60;

        // 25�ð� ������ ���� ����
        if (hours >= 25)
        {
            Debug.Log("������ ���� �Ǿ����ϴ�.");
        }

        // UI ������Ʈ (���� �ð� ���)
        timeText.text = string.Format("{0:00}:{1:00}", hours % 24, minutes);
    }
}
