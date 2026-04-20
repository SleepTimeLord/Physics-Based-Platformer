using UnityEngine;
using System.Runtime.InteropServices;

public class SpeechManager : MonoBehaviour
{
    public static SpeechManager Instance;

    public RobotController robot;

    [DllImport("__Internal")]
    private static extern void StartSpeechRecognition();

    [DllImport("__Internal")]
    private static extern void StopSpeechRecognition();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        StartSpeechRecognition();
#else
        Debug.Log("[SpeechManager] Voice recognition only works in a WebGL build.");
#endif
    }

    public void OnSpeechResult(string word)
    {
        Debug.Log("Heard: " + word);

        if (robot == null)
        {
            Debug.LogWarning("[SpeechManager] No robot assigned!");
            return;
        }

        switch (word)
        {
            case "right":  robot.currentState = RobotState.MovingForward; break;
            case "left":   robot.currentState = RobotState.MovingBackward; break;
            case "up":     robot.currentState = RobotState.MovingUp; break;
            case "down":   robot.currentState = RobotState.MovingDown; break;
            case "stop":   robot.currentState = RobotState.Idle; break;
            case "debug":  robot.currentState = RobotState.Debug; break;
        }
    }
}