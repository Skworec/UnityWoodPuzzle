using UnityEngine;

public class LooseHandler : MonoBehaviour
{
    [SerializeField] private GameObject LooseWindow;
    private void Start()
    {
        DataController.instance.onLoose.AddListener(OnLooseHandler);
    }

    private void OnLooseHandler()
    {
        LooseWindow.SetActive(true);
    }
}
