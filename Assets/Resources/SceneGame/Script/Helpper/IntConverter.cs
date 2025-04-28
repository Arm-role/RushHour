using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IntConverter
{
    public static byte[] PackRLEIDs(int[] ids)
    {
        List<byte> result = new List<byte>();

        for (int i = 0; i < ids.Length; i++)
        {
            int count = 1;
            while (i + 1 < ids.Length && ids[i] == ids[i + 1])
            {
                count++;
                i++;
            }

            result.AddRange(BitConverter.GetBytes((ushort)ids[i]));
            result.Add((byte)count);
        }

        return result.ToArray();
    }
    public static int[] UnpackRLEIDs(byte[] data)
    {
        List<int> ids = new List<int>();

        for (int i = 0; i < data.Length; i += 3)
        {
            int id = BitConverter.ToUInt16(data, i);
            int count = data[i + 2];

            for (int j = 0; j < count; j++)
            {
                ids.Add(id);
            }
        }
        return ids.ToArray();
    }
}
