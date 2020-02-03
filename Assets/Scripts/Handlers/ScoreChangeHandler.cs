using UnityEngine;
using UnityEngine.UI;

public class ScoreChangeHandler : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    void Start()
    {
        scoreText = GetComponent<Text>();
        DataController.instance.onScoreChange.AddListener(OnScoreChangeHandler);
    }

    private void OnScoreChangeHandler()
    {
        scoreText.text = DataController.instance.Score.ToString();
    }

    private void OnDestroy()
    {
        DataController.instance.onScoreChange.RemoveListener(OnScoreChangeHandler);
    }
}
