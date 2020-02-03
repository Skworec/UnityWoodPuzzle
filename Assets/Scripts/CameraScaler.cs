using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private Camera cam;
    private float defSize;
    private float defWidth = 1080;
    private float defHeigth = 1920;
    void Start()
    {
        cam = Camera.main;
        defSize = cam.orthographicSize;
        float newCamSize = (Screen.height / defHeigth * defWidth / Screen.width * defSize);
        Debug.Log(newCamSize.ToString());
        cam.orthographicSize = newCamSize;
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + (defSize - cam.orthographicSize), cam.transform.position.z);
        Debug.Log(cam.orthographicSize.ToString());
    }
}
