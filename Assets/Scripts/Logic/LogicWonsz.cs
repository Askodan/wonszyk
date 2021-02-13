using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EatenApple
{
    none,
    normal,
    players
}
public class LogicWonsz
{
    //input
    uint playerId;
    NetResults results = new NetResults();
    LogicWonszPart[] parts;
    PlayerDirection direction;
    bool shootLaser = false;
    int changeLength = 0;

    //output
    bool shotHit = false;
    int laserCutParts = 0;
    bool stopped = false;
    bool collide = false;
    EatenApple ate = EatenApple.none; // index of eaten apple
    public LogicWonsz()
    {

    }
    public LogicWonsz(uint id, Vector2Int position, int size)
    {
        PlayerId = id;
        Results.playerId = id;
        Parts = new LogicWonszPart[size];
        for (int i = 0; i < Parts.Length; i++)
        {
            Parts[i] = new LogicWonszPart(this, position);
        }
    }
    // state
    public int stoppedTill = 0;

    public void Reset()
    {
        ShotHit = false;
        laserCutParts = 0;
        Stopped = false;
        Collide = false;
        Ate = EatenApple.none;
        ChangeLength = 0;
        foreach (var part in parts)
        {
            part.SavePosition();
        }
    }
    public LogicWonszPart[] Parts { get { return parts; } set { parts = value; } }
    public PlayerDirection Direction { get { return direction; } set { direction = value; } }
    public bool Collide { get { return collide; } set { collide = value; } }
    public uint PlayerId { get { return playerId; } set { playerId = value; } }
    public bool Stopped { get { return stopped; } set { stopped = value; } }
    public int ChangeLength { get { return changeLength; } set { changeLength = value; } }
    public int LaserCutParts { get { return laserCutParts; } set { laserCutParts = value; } }
    public bool ShootLaser { get { return shootLaser; } set { shootLaser = value; } }
    public bool ShotHit { get { return shotHit; } set { shotHit = value; } }
    public EatenApple Ate { get { return ate; } set { ate = value; } }
    public NetResults Results { get { return results; } set { results = value; } }

    public void MoveForward(int MapSize)
    {
        for (int i = 0; i < parts.Length; i++)
        {
            int index = parts.Length - i - 1;
            LogicItemOnMap bodyPart = parts[index];
            if (index > 0)
            {
                parts[index].Position = parts[index - 1].Position;
            }
        }

        parts[0].Position = LogicMap.KeepOnMap(parts[0].Position + direction.ToVector2Int(), MapSize);
    }
    public void BackOff()
    {
        foreach (var part in parts)
        {
            part.LoadPosition();
        }
    }
    public LogicWonszPart[] Cut(LogicWonszPart hitElement, int minLength)
    {
        List<LogicWonszPart> stays = new List<LogicWonszPart>();
        List<LogicWonszPart> cut = new List<LogicWonszPart>();
        int i = 0;
        bool hit = false;
        foreach (var part in parts)
        {
            if (part == hitElement)
            {
                hit = true;
            }
            if (i < minLength || !hit)
            {
                stays.Add(parts[i]);
            }
            else if (part != hitElement)
            {
                if (parts[i - 1].Position != parts[i].Position)
                    cut.Add(parts[i]);
            }
            i++;
        }
        laserCutParts -= parts.Length - stays.Count;
        parts = stays.ToArray();
        return cut.ToArray();
    }
    public void ApplyChangeLength()
    {
        if (ChangeLength != 0)
        {
            LogicWonszPart[] newparts = new LogicWonszPart[parts.Length + ChangeLength];
            int copyCount = Mathf.Min(newparts.Length, parts.Length);
            for (int i = 0; i < copyCount; i++)
            {
                newparts[i] = parts[i];
            }
            if (ChangeLength > 0)
            {
                for (int i = copyCount; i < newparts.Length; i++)
                {
                    if (i < parts.Length - 1)
                    {
                        newparts[i] = new LogicWonszPart(this, parts[i - 1].OldPosition);
                    }
                    else
                    {
                        newparts[i] = new LogicWonszPart(this, parts[parts.Length - 1].OldPosition);
                    }
                    newparts[i].SavePosition();
                }
            }
            ChangeLength = 0;
            parts = newparts;
        }
    }
    public override string ToString()
    {
        string result = "";
        foreach (var pos in parts)
        {
            result += pos.ToString();
        }
        result += ShootLaser ? "shot " : "not shot ";
        result += Ate != EatenApple.none ? "ate " : "not ate ";
        result += Collide ? "hit " : "not hit ";
        return result;
    }
}
