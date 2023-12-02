using UnityEngine;

public class atest : MonoBehaviour
{
    float deltaTime = 0.0f;

    /*void Update()
    {
        // Delta time은 이전 프레임과 현재 프레임 사이의 시간 간격입니다.
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 6 / 100;
        style.normal.textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }*/
}