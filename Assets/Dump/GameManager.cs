using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }
            return instance;
        }
    }

    [Header("Time")]
    public TextMeshProUGUI timeText; // UI�� ���� ���� �ð��� ǥ���ϴ� �ؽ�Ʈ
    public float gameTime = 0.0f; // ���� ������ �ð��� �ʷ� ����
    public float timeScale = 60.0f; // ���� �ð� 1�ʴ� ���� �ð� 60�� (1��)

    [Header("")]
    public int playerMoney = 10000;

    private UIManager UIManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UIManager = FindObjectOfType<UIManager>();
    }

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
