using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class h
{
    /// <summary>
    /// 安全(大嘘)获取组件，如果目标GameObject没有对应组件则Add一个
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="targetGameObject">目标GameObject</param>
    /// <returns>组件T</returns>
    public static T GetOrAddComponent<T>(GameObject targetGameObject) where T : Component
    {
        T theComponent = targetGameObject.GetComponent<T>();
        if (theComponent == null)
        {
            theComponent = targetGameObject.AddComponent<T>();
        }
        return theComponent;
    }

    /// <summary>
    /// 快捷使用h.l(可以ToString的Object)即可输出一些什么东西
    /// 省点时间 省点人生 :D
    /// </summary>
    /// <param name="someObj">可以ToString的什么Object</param>
    public static void l(System.Object someObj)
    {
        Debug.Log(someObj);
    }
    /// <summary>
    /// 在指定位置画球，用以Debug
    /// </summary>
    /// <param name="WorldPos">球的世界坐标</param>
    /// <param name="ballColor">球的颜色</param>
    /// <param name="IsKinematic">球是否*不受*物理影响</param>
    /// <param name="size">球的大小</param>
    /// <param name="LiveTime">球的存活时间</param>
    /// <returns>球的Game Object</returns>
    public static GameObject GenerateBall(Vector3 WorldPos, Color ballColor, bool IsKinematic = true, float size = 0.2f, float LiveTime = 1)
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.transform.position = WorldPos;
        ball.transform.localScale = Vector3.one * size;
        ball.GetComponent<Renderer>().material.color = ballColor;
        if (!IsKinematic)
        {
            Rigidbody rb = ball.AddComponent<Rigidbody>();
            rb.isKinematic = false;
        }
        else
        {
            GameObject.Destroy(ball.GetComponent<Collider>());
        }
        SelfDestruct sd = ball.AddComponent<SelfDestruct>();
        sd.timer = LiveTime;
        return ball;
    }
    /// <summary>
    /// 因为WorldToScreenPoint给的坐标和Canvas上的坐标有差，所以用这个方法可以取得在Canvas上的坐标
    /// </summary>
    /// <param name="WorldPos">需要求得的世界坐标</param>
    /// <returns>在Canvas上对应的坐标</returns>
    public static Vector3 WorldOnCanvasPos(Vector3 WorldPos)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(WorldPos);
        return pos - new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    public static string GetStateName(List<string> AllPossibleNames, AnimatorStateInfo StateInfo)
    {
        foreach (string str in AllPossibleNames)
        {
            if (StateInfo.IsName(str))
            {
                return str;
            }
        }
        return "";
    }

    /// <summary>
    /// write a TimeStampedState object to a file
    /// </summary>
    /// <param name="fileName">the name of the target file</param>
    /// <param name="data">the object</param>
    public static void writeMovementToFile(string fileName, ReversedMovementContainer data)
    {
        try
        {
            using (Stream stream = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, data);
            }
        }
        catch (IOException)
        {
            Debug.Log("write to file failure");
        }
    }

    /// <summary>
    /// reads movement data from a pre-existing file
    /// </summary>
    /// <param name="fileName">the name of the file</param>
    /// <returns>a TimeStampedState object</returns>
    public static ReversedMovementContainer readMovementFromFile(string fileName)
    {
        try
        {
            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();

                var recievedData = (ReversedMovementContainer)bin.Deserialize(stream);

                return recievedData;
            }
        }
        catch (IOException)
        {
            Debug.Log("read from file failure");
            throw new IOException();
        }
    }
}
/// <summary>
/// 定时自毁组件
/// </summary>
public class SelfDestruct : MonoBehaviour
{
    public float timer = 1;
    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            DestroyImmediate(this.gameObject);
        }
    }
}

/// <summary>
/// 会自动重置的数值容器，可以用于计算冷却计时
/// 举例：
/// if(autoResetContainer.IsZeroReached(Time.fixedDeltaTime){
///     ...
/// }
/// 此IsZeroReached会在内置值到达0的时候返回true并重置内置值
/// </summary>
[System.Serializable]
[SerializeField]
public class AutoResetCounter
{
    public float Temp = 0;
    public float Max;

    /// <summary>
    /// Constructor. Temp is 0 at the start.
    /// </summary>
    /// <param name="MaxToSet"></param>
    public AutoResetCounter(float MaxToSet)
    {
        Max = MaxToSet;
    }

    /// <summary>
    /// Check if is completed, while reducing temp by reduction, if temp <=0 then true returned and will reset temp if resetToMax = true. 
    /// Note that the reduce will run before the calculation of lessThanZero. 
    /// </summary>
    /// <param name="reduction"> Amount of temp that will be reduced. </param>
    /// <param name="resetToMax"> Should reset the temp if temp reached 0? </param>
    /// <param name="DoNotGoLessThanZero"> Should temp always be no less than zero</param>
    /// <returns>True if zero reached, false if not. </returns>
    public bool IsZeroReached(float reduction, bool resetToMax = true, bool DoNotGoLessThanZero = true)
    {
        Temp -= reduction;
        Temp = (DoNotGoLessThanZero && Temp <= 0) ? 0 : Temp;
        if (Temp > 0)
        {
            return false;
        }
        else
        {
            Temp = resetToMax ? Max : Temp;
            return true;
        }
    }
    /// <summary>
    /// Make Temp = Max. 
    /// </summary>
    public void MaxmizeTemp()
    {
        Temp = Max;
    }
}

[System.Serializable]
[SerializeField]
public struct ARCStruct
{
    public float Temp;
    public float Max;

    /// <summary>
    /// Constructor. Temp is 0 at the start.
    /// </summary>
    /// <param name="MaxToSet"></param>
    public ARCStruct(float MaxToSet)
    {
        Max = MaxToSet;
        Temp = 0;
    }

    /// <summary>
    /// Check if is completed, while reducing temp by reduction, if temp <=0 then true returned and will reset temp if resetToMax = true. 
    /// Note that the reduce will run before the calculation of lessThanZero. 
    /// </summary>
    /// <param name="reduction"> Amount of temp that will be reduced. </param>
    /// <param name="resetToMax"> Should reset the temp if temp reached 0? </param>
    /// <param name="DoNotGoLessThanZero"> Should temp always be no less than zero</param>
    /// <returns>True if zero reached, false if not. </returns>
    public bool IsZeroReached(float reduction, bool resetToMax = true, bool DoNotGoLessThanZero = true)
    {
        Temp -= reduction;
        Temp = (DoNotGoLessThanZero && Temp <= 0) ? 0 : Temp;
        if (Temp > 0)
        {
            return false;
        }
        else
        {
            Temp = resetToMax ? Max : Temp;
            return true;
        }
    }
    /// <summary>
    /// Make Temp = Max. 
    /// </summary>
    public void MaxmizeTemp()
    {
        Temp = Max;
    }
}
