using UnityEngine;

public class StartEnd : MonoBehaviour
{
    public void GoToNextLevel()
    {
        GameManager.Instance.NextLevel();
    }
}
