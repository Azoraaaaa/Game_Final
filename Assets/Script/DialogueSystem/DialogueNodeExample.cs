using UnityEngine;

/// <summary>
/// 对话节点使用示例
/// 展示如何使用新的多段台词和多发言人功能
/// </summary>
public class DialogueNodeExample : MonoBehaviour
{
    [Header("示例对话节点")]
    public DialogueNode exampleNode;
    
    [Header("测试按钮")]
    public bool showTestButton = true;
    
    private void OnGUI()
    {
        if (!showTestButton) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 350, 250));
        
        if (GUILayout.Button("测试多发言人对话"))
        {
            if (exampleNode != null)
            {
                DialogueManager.Instance.StartDialogue(exampleNode);
            }
            else
            {
                Debug.LogWarning("请先设置示例对话节点！");
            }
        }
        
        GUILayout.Label("使用说明：");
        GUILayout.Label("1. 在DialogueNode中设置sentences数组");
        GUILayout.Label("2. 为每句台词设置对应的发言人");
        GUILayout.Label("3. 玩家点击继续按钮逐句播放");
        GUILayout.Label("4. 发言人姓名会动态更新");
        GUILayout.Label("5. 所有台词播放完毕后显示选择");
        
        GUILayout.Space(10);
        GUILayout.Label("多发言人示例：");
        GUILayout.Label("台词1: 村长 - '你好，冒险者！'");
        GUILayout.Label("台词2: 冒险者 - '你好，村长！'");
        GUILayout.Label("台词3: 村长 - '欢迎来到我们的村庄。'");
        GUILayout.Label("台词4: 村长 - '这里有很多有趣的任务。'");
        
        GUILayout.EndArea();
    }
}

/*
使用示例：

1. 创建对话节点：
   - 右键 -> Create -> Dialogue -> Dialogue Node
   - 设置默认发言人姓名
   - 在sentences数组中添加多段台词
   - 在speakerNames数组中设置对应的发言人

2. 多发言人台词设置示例：
   defaultSpeakerName: "村长"
   
   sentences[0]: "你好，冒险者！"
   speakerNames[0]: "村长"
   
   sentences[1]: "你好，村长！"
   speakerNames[1]: "冒险者"
   
   sentences[2]: "欢迎来到我们的村庄。"
   speakerNames[2]: "" (空字符串表示使用默认发言人)
   
   sentences[3]: "这里有很多有趣的任务等着你。"
   speakerNames[3]: "村长"

3. 设置选择选项：
   - 添加PlayerChoice
   - 设置选择文本和下一个节点

4. 在场景中使用：
   - 将对话节点赋值给DialogueBehaviour
   - 或在代码中调用DialogueManager.Instance.StartDialogue(node)

5. 播放效果：
   - 每句台词播放时会显示对应的发言人姓名
   - 如果speakerNames为空，则使用defaultSpeakerName
   - 支持多个角色轮流发言
*/ 