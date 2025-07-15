using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    private int whiteScore = 0;
    private int blackScore = 0;

    public static ScoreManager Instance { get; private set; }

    private TextMeshProUGUI scoreTmp;
    private TextMeshProUGUI msg_white;
    private TextMeshProUGUI msg_black;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        scoreTmp = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        msg_white = GameObject.Find("msg_white").GetComponent<TextMeshProUGUI>();
        msg_black = GameObject.Find("msg_black").GetComponent<TextMeshProUGUI>();

        msg_white.alpha = 0.0f;
        msg_black.alpha = 0.0f;
    }


    public void ChangeScore(GameObject checker)
    {
        DragShoot ds = checker.GetComponent<DragShoot>();
        TurnManager.PlayerTurn _color = ds.__color;
        if (_color == TurnManager.PlayerTurn.White)
            blackScore++;
        else
            whiteScore++;
        scoreTmp.text = whiteScore + ":" + blackScore;

        if (blackScore == 8)
        {
            msg_white.alpha = 1f;
            msg_black.alpha = 1f;
            msg_white.text = "You lose";
            msg_black.text = "You win";
        }
        if (whiteScore == 8)
        {
            msg_white.alpha = 1f;
            msg_black.alpha = 1f;
            msg_white.text = "You win";
            msg_black.text = "You lose";
        }
        if (blackScore == 8 && whiteScore == 8)
        {
            msg_white.alpha = 1f;
            msg_black.alpha = 1f;
            msg_white.text = "Draw";
            msg_black.text = "Draw";
        }

        if (blackScore == 8 || whiteScore == 8)
            Invoke("ExitLobby", 3f);
    }

    private void ExitLobby()
    {
        SceneManager.LoadScene(0);
    }
}
