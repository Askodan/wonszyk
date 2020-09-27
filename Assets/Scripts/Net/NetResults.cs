using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetResults
{
    public uint playerId;
    public int points;
    public int hits;
    public int meals;
    public int shots;
    static public int CalcSize()
    {
        return 20;
    }
    public byte[] ToByteArray()
    {
        List<byte> result = new List<byte>();
        result.AddRange(System.BitConverter.GetBytes(playerId));
        result.AddRange(System.BitConverter.GetBytes(points));
        result.AddRange(System.BitConverter.GetBytes(hits));
        result.AddRange(System.BitConverter.GetBytes(meals));
        result.AddRange(System.BitConverter.GetBytes(shots));
        return result.ToArray();
    }
    static public byte[] ArrayToByteArray(NetResults[] arr)
    {
        List<byte> result = new List<byte>();
        foreach(var nr in arr)
        {
            result.AddRange(nr.ToByteArray());
        }
        return result.ToArray();
    }

    static public NetResults FromByteArray(byte[] arr)
    {
        NetResults result = new NetResults();
        result.playerId = System.BitConverter.ToUInt32(arr, 0);
        result.points = System.BitConverter.ToInt32(arr, 4);
        result.hits = System.BitConverter.ToInt32(arr, 8);
        result.meals = System.BitConverter.ToInt32(arr, 12);
        result.shots = System.BitConverter.ToInt32(arr, 16);
        return result;
    }

    static public NetResults[] ArrayFromByteArray(byte[] arr)
    {
        int size = NetResults.CalcSize();
        if(arr.Length % size != 0)
        {
            Debug.LogError("Size of byte array is wrong while deoding NetResult");
            return null;
        }
        List<byte> bytelist = new List<byte>(arr);
        NetResults[] result = new NetResults[Mathf.FloorToInt(arr.Length / size)];
        for(int i = 0; i < result.Length; i++)
        {
            result[i] = FromByteArray(bytelist.GetRange(i * size, size).ToArray());
        }

        return result;
    }
    public override string ToString()
    {
        string result = "";
        result += "id " + playerId + " ";
        result += "points " + points + " ";
        result += "hits " + hits + " ";
        result += "meals " + meals + " ";
        result += "shots " + shots + " ";
        return result;
    }

    static public void TestCoding()
    {
        NetResults test = new NetResults();
        test.playerId = 124;
        test.hits = 412;
        test.meals = -2;
        test.points = 4512;
        test.shots = 123;
        NetResults atest = NetResults.FromByteArray(test.ToByteArray());
        Debug.Log("Test single");
        Debug.Log(test);
        Debug.Log(atest);
        Debug.Log("test array");
        NetResults[] arr = new NetResults[2] { test, test };
        NetResults[] arrtest = NetResults.ArrayFromByteArray(NetResults.ArrayToByteArray(arr));
        Debug.Log(arrtest[0]);
        Debug.Log(arrtest[1]);
    }
}
