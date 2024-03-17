using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // 正式游戏场景的名称或索引
    public string gameSceneName = "Tutorial";

    void Update()
    {
        // 检测玩家是否按下了任意键
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 加载正式游戏场景
            SceneManager.LoadScene(gameSceneName);
        }
    }
}