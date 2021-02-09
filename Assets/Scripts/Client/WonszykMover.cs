using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;


public class WonszykMover : MonoBehaviour
{
    private const int sliceSize = 3;
    [SerializeField] private GameObject blockedMovementEffect;
    private List<WonszykOnMap> body;
    private WonszykServerData wonszykServerData;
    [SerializeField] private WonszykOnMap bodyPart;
    public WonszykOnMap Head { get { return body.Count > 0 ? body[0] : null; } }
    public WonszykOnMap Neck { get { return body.Count > 1 ? body[1] : null; } }
    public WonszykOnMap Tail { get { return body.Count > 0 ? body[body.Count - 1] : null; } }
    public WonszykPlayerData data;
    private void Awake()
    {
        body = new List<WonszykOnMap>();
        blockedMovementEffect.SetActive(false);
        wonszykServerData = ((WonszykServerData)WonszykServerData.Instance);
        wonszykServerData.PropertyChanged += SetBlockMovementTimeAlive;
        SetBlockMovementTimeAlive(wonszykServerData, null);
    }
    private void SetBlockMovementTimeAlive(object sender, PropertyChangedEventArgs e)
    {
        var wsd = (WonszykServerData)sender;
        blockedMovementEffect.GetComponentInChildren<Scale>().CycleTime = wsd.FramesStopped / wsd.gameSpeed;
    }
    public int GetLength() { return body.Count; }
    public void UpdateColor()
    {
        foreach (var part in body)
        {
            part.SetColor(data.WonszColor);
        }
    }
    public void SetLength(int length, Vector2Int startPosition = new Vector2Int())
    {
        while (body.Count < length)
        {
            WonszykOnMap newPart = Instantiate(bodyPart);
            newPart.SetColor(data.WonszColor);
            if (body.Count > 0)
            {
                newPart.CopyPositionAndDirection(Tail, false);
            }
            else
            {
                newPart.Position = startPosition;
            }

            body.Add(newPart);
        }
        while (body.Count > length)
        {
            WonszykOnMap lastPart = Tail;
            body.Remove(lastPart);
            Destroy(lastPart.gameObject);
        }

        // Set correct body visualization
        int num = 0;
        int len = body.Count - 1;
        foreach (var part in body)
        {
            if (num != len && num != 0)
                part.PositionInWonszyk = WonszykPosition.Body;
            else
            if (num == 0)
                part.PositionInWonszyk = WonszykPosition.Head;
            else
            if (num == len)
                part.PositionInWonszyk = WonszykPosition.Tail;

            num++;
        }
    }

    public void BeMoved(Vector2Int newPos)
    {
        if (newPos.x == Head.Position.x && newPos.y == Head.Position.y)
            return;
        Move();
        Head.Position = newPos;
    }

    public void MoveForward()
    {
        Move();
        Head.MoveForward();
    }

    void Move()
    {
        for (int i = 0; i < body.Count; i++)
        {
            int index = body.Count - i - 1;
            WonszykOnMap bodyPart = body[index];
            if (index > 0)
            {
                body[index].CopyPositionAndDirection(body[index - 1]);
            }
        }
    }

    public PlayerDirection GetUnallowedDirection()
    {
        if (Head && Neck)
        {
            Vector2Int newPos = ItemOnMap.DirectionVector(Head.Position, Neck.Position, GameLogic.Instance.map.Size);
            return PlayerDirectionMethods.FromVector2Int(newPos);
        }
        else
        {
            return PlayerDirection.None;
        }
    }

    static public byte[] ToTailString(LogicWonszPart[] Bbody)
    {
        LogicWonszPart BHead = Bbody[0];
        List<byte> result = new List<byte>();
        List<PlayerDirection> temp = new List<PlayerDirection>();
        LogicWonszPart prev = BHead;
        foreach (var el in Bbody)
        {
            if (el == BHead)
            {
                continue;
            }
            temp.Add(PlayerDirectionMethods.FromVector2Int(ItemOnMap.DirectionVector(prev.Position, el.Position)));
            prev = el;
            if (temp.Count == sliceSize)
            {
                result.Add(Encode(temp.ToArray()));
                temp.Clear();
            }
        }
        if (temp.Count > 0)
        {
            result.Add(Encode(temp.ToArray()));
        }
        return result.ToArray();
    }
    static public List<PlayerDirection> PosesFromTailString(byte[] tail)
    {
        List<PlayerDirection> arrayFromTail = new List<PlayerDirection>();
        foreach (var el in tail)
        {
            PlayerDirection[] slice = Decode(el);
            foreach (var l in slice)
            {
                if (l != PlayerDirection.None)
                {
                    arrayFromTail.Add(l);
                }
                else
                {
                    break;
                }
            }
        }
        return arrayFromTail;
    }
    public void FromNetWonsz(NetWonsz original)
    {
        if (Head)
        {
            blockedMovementEffect.transform.position = Head.transform.position;
            if (original.collide || blockedMovementEffect.activeSelf)
                blockedMovementEffect.SetActive(Head.Position == original.positions[0]);
        }
        SetLength(original.positions.Length);
        int i = 0;
        foreach (var pos in original.positions)
        {
            Vector2Int trim = ItemOnMap.KeepOnMap(pos, GameLogic.Instance.data.mapSize);
            body[i].Position = trim;
            if (i > 0)
            {
                body[i].Direction = original.directions[i - 1].Opposite();
            }
            i++;
        }
    }
    public void FromTailString(byte[] tail)
    {
        List<PlayerDirection> arrayFromTail = PosesFromTailString(tail);
        SetLength(arrayFromTail.Count + 1);
        int i = 1;
        foreach (var el in arrayFromTail)
        {
            body[i].Position = ItemOnMap.KeepOnMap(body[i - 1].Position + el.ToVector2Int(), GameLogic.Instance.map.Size);
            body[i].Direction = el;
        }
    }

    static private List<PlayerDirection> DirectionsArray = new List<PlayerDirection> {
            PlayerDirection.Down,
            PlayerDirection.Left,
            PlayerDirection.Right,
            PlayerDirection.Up,
            PlayerDirection.None };

    static public byte Encode(PlayerDirection[] input)
    {
        if (input.Length < sliceSize)
        {
            PlayerDirection[] temp = new PlayerDirection[] { PlayerDirection.None, PlayerDirection.None, PlayerDirection.None };
            for (int i = 0; i < input.Length; i++)
            {
                temp[i] = input[i];
            }
            input = temp;
        }
        else if (input.Length > sliceSize)
        {
            Debug.LogError("Encoding input longer than slize size!");
            return 0;
        }
        byte result = 0;
        byte mul = 1;
        for (int i = 0; i < input.Length; i++)
        {
            result += (byte)(DirectionsArray.IndexOf(input[i]) * mul);
            mul *= (byte)DirectionsArray.Count;
        }
        return result;
    }

    static public PlayerDirection[] Decode(byte input)
    {
        int i_input = input;
        int first = (int)Mathf.Pow(DirectionsArray.Count, 1);
        int second = (int)Mathf.Pow(DirectionsArray.Count, 2);
        int mod_second = i_input % second;
        PlayerDirection one = DirectionsArray[mod_second % first];
        PlayerDirection two = DirectionsArray[Mathf.FloorToInt(mod_second / first)];
        PlayerDirection three = DirectionsArray[Mathf.FloorToInt(i_input / second)];

        return new PlayerDirection[] { one, two, three };
    }

    void TestCoding()
    {
        Dictionary<byte, PlayerDirection[]> Coder = new Dictionary<byte, PlayerDirection[]>();
        int num = 0;

        foreach (var dir3 in DirectionsArray)
        {
            foreach (var dir2 in DirectionsArray)
            {
                foreach (var dir1 in DirectionsArray)
                {
                    PlayerDirection[] tab = new PlayerDirection[] { dir1, dir2, dir3 };
                    Coder.Add((byte)num, tab);
                    num++;
                }
            }
        }
        foreach (var code in Coder)
        {
            //Encode test
            Debug.Log("Encoding");
            if (Encode(code.Value) == code.Key)
            {
                Debug.Log("ok " + code.Key.ToString());
            }
            else
            {
                string text = code.Key.ToString() + " ";
                foreach (var dir in code.Value)
                    text += dir.ToString() + " ";
                text += Encode(code.Value).ToString();
                Debug.Log(text);
            }
            //Decode test
            Debug.Log("Decoding (uses encoding to test)");
            if (Encode(Decode(code.Key)) == code.Key)
            {
                Debug.Log("ok " + code.Key.ToString());
            }
            else
            {
                string text = code.Key.ToString() + " ";
                foreach (var dir in Decode(code.Key))
                    text += dir.ToString() + " ";
                text += Encode(Decode(code.Key)).ToString();
                Debug.Log(text);
            }
        }
    }
}
