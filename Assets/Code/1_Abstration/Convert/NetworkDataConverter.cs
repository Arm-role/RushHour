using System;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkDataConverter
{
    public static byte[] PackIntList(List<int> intList)
    {
        if (intList == null)
            intList = new List<int>();

        byte[] data = new byte[4 + (intList.Count * 4)];

        Buffer.BlockCopy(BitConverter.GetBytes(intList.Count), 0, data, 0, 4);

        for (int i = 0; i < intList.Count; i++)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(intList[i]), 0, data, 4 + (i * 4), 4);
        }

        return data;
    }

    public static List<int> UnpackIntList(byte[] data)
    {
        var intList = new List<int>();
        if (data == null || data.Length < 4)
        {
            return intList;
        }

        int count = BitConverter.ToInt32(data, 0);

        if (data.Length < 4 + (count * 4))
        {
            Debug.LogError("Data corruption detected: Byte array is too short for the specified count.");
            return intList;
        }

        for (int i = 0; i < count; i++)
        {
            int value = BitConverter.ToInt32(data, 4 + (i * 4));
            intList.Add(value);
        }

        return intList;
    }
}