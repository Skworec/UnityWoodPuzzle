using UnityEngine;
using UnityEngine.UI;

public class BestScoreChangeHandler : MonoBehaviour
{
    [SerializeField] private Text bestScoreText;

    void Start()
    {
        bestScoreText = GetComponent<Text>();
        DataController.instance.onBestScoreChange.AddListener(OnBestScoreChangeHandler);
    }

    private void OnBestScoreChangeHandler()
    {
        bestScoreText.text = DataController.instance.BestScore.ToString();
    }

    private void OnDestroy()
    {
        DataController.instance.onBestScoreChange.RemoveListener(OnBestScoreChangeHandler);
    }
}
