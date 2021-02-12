using UnityEngine;

public class ArrayOnMapSerializator<T> where T : LogicItemOnMap, new()
{
    private int bytesLength;
    public ArrayOnMapSerializator()
    {
        bytesLength = new T().BytesLength;
    }
    public byte[] ArrayToBytes(T[] items)
    {
        var result = new byte[bytesLength * items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            items[i].ToBytes().CopyTo(result, i * bytesLength);
        }
        return result;
    }
    public T[] Bytes2Array(byte[] bytes)
    {
        var result = new T[bytes.Length / bytesLength];
        for (int i = 0; i < result.Length; i++)
        {
            var input = new byte[bytesLength];
            CopyBytes(bytes, input, i * bytesLength, bytesLength);
            result[i] = new T();
            result[i].LoadFromBytes(input);
        }
        return result;
    }
    void CopyBytes(byte[] source, byte[] dest, int start, int len)
    {
        for (int i = 0; i < len; i++)
        {
            dest[i] = source[start + i];
        }
    }
}