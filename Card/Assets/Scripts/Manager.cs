using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject targetObject; // 要将脚本附加到的目标物体
    public GameObject targetObject1; // 要将脚本附加到的目标物体
    public MonoBehaviour scriptToAttach; // 要附加的脚本

    void Start()
    {
        // 检查目标物体和脚本是否存在
        if (targetObject != null && scriptToAttach != null && targetObject1!= null)
        {
            // 将脚本附加到目标物体上
            targetObject.AddComponent(scriptToAttach.GetType());
            targetObject1.AddComponent(scriptToAttach.GetType());
        }
    }
}
