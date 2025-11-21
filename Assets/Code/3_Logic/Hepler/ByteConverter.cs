using System;
using System.Collections.Generic;

public static class ByteConverter
{
    // -------- Int Conversion --------
    public static byte[] IntToBytes(int value)
    {
        return BitConverter.GetBytes(value);
    }

    public static int BytesToInt(byte[] data, int startIndex = 0)
    {
        if (data == null || data.Length < sizeof(int))
            throw new ArgumentException("Invalid byte array for int conversion.");
        return BitConverter.ToInt32(data, startIndex);
    }

    public static byte[] IntArrayToBytes(int[] values)
    {
        if (values == null || values.Length == 0)
            return Array.Empty<byte>();

        byte[] bytes = new byte[values.Length * sizeof(int)];
        Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public static int[] BytesToIntArray(byte[] data)
    {
        if (data == null || data.Length == 0)
            return Array.Empty<int>();

        if (data.Length % sizeof(int) != 0)
            throw new ArgumentException("Invalid byte array length for int array conversion.");

        int[] result = new int[data.Length / sizeof(int)];
        Buffer.BlockCopy(data, 0, result, 0, data.Length);
        return result;
    }

    // -------- Float Conversion --------
    public static byte[] FloatToBytes(float value)
    {
        return BitConverter.GetBytes(value);
    }

    public static float BytesToFloat(byte[] data, int startIndex = 0)
    {
        if (data == null || data.Length < sizeof(float))
            throw new ArgumentException("Invalid byte array for float conversion.");
        return BitConverter.ToSingle(data, startIndex);
    }

    public static byte[] FloatArrayToBytes(float[] values)
    {
        if (values == null || values.Length == 0)
            return Array.Empty<byte>();

        byte[] bytes = new byte[values.Length * sizeof(float)];
        Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public static float[] BytesToFloatArray(byte[] data)
    {
        if (data == null || data.Length == 0)
            return Array.Empty<float>();

        if (data.Length % sizeof(float) != 0)
            throw new ArgumentException("Invalid byte array length for float array conversion.");

        float[] result = new float[data.Length / sizeof(float)];
        Buffer.BlockCopy(data, 0, result, 0, data.Length);
        return result;
    }

    // -------- Combine Utility --------
    public static byte[] Combine(params byte[][] arrays)
    {
        List<byte> combined = new List<byte>();
        foreach (var arr in arrays)
        {
            if (arr != null)
                combined.AddRange(arr);
        }
        return combined.ToArray();
    }
}
