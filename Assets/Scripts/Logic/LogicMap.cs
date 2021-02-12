using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LogicMap
{
    WonszykServerData data;
    int frame = 0;
    List<LogicWonsz> all_wonsz;
    List<LogicWall> walls;
    List<LogicApple> apples;
    LogicApple currentApple;

    public LogicMap(WonszykServerData data_, uint[] wonszyks_id)
    {
        Apples = new List<LogicApple>();
        Walls = new List<LogicWall>();
        data = data_;
        PlaceWonszyks(wonszyks_id);
        currentApple = new LogicApple(GetRandomFreePlace());
    }

    void PlaceWonszyks(uint[] wonszyks_id)
    {
        all_wonsz = new List<LogicWonsz>();
        foreach (var wonszyk_id in wonszyks_id)
        {
            var wonszyk_size = Mathf.Max(data.startLength, data.minLength);
            var wonszyk_position = GetRandomFreePlace();
            all_wonsz.Add(new LogicWonsz(wonszyk_id, wonszyk_position, wonszyk_size));
        }
    }
    public WonszykServerData Data { get { return data; } }
    public List<LogicWonsz> All_wonsz { get { return all_wonsz; } set { all_wonsz = value; } }
    public List<LogicWall> Walls { get { return walls; } set { walls = value; } }
    public List<LogicApple> Apples { get { return apples; } set { apples = value; } }
    public LogicApple CurrentApple { get { return currentApple; } set { currentApple = value; } }

    public void Simulate()
    {
        frame++;
        foreach (var wonsz in all_wonsz)
        {
            wonsz.Reset();
            wonsz.Stopped = wonsz.stoppedTill > frame;
            wonsz.ShootLaser = wonsz.ShootLaser && !wonsz.Stopped;
        }
        CheckShooters();
        foreach (var wonsz in all_wonsz)
        {
            if (!wonsz.Stopped)
            {
                wonsz.MoveForward(data.mapSize);
            }
        }
        CheckCollisions();
        foreach (var wonsz in all_wonsz)
        {
            if (wonsz.Collide)
            {
                wonsz.BackOff();
                wonsz.stoppedTill = frame + data.FramesStopped;
            }
            wonsz.ApplyChangeLength();
            if (wonsz.Ate == EatenApple.normal)
            {
                currentApple.Position = GetRandomFreePlace();
            }
        }
    }

    void CheckCollisions()
    {
        for (int i = 0; i < all_wonsz.Count; i++)
        {
            LogicWonsz tested = all_wonsz[i];
            // if not moving not colliding
            if (tested.Stopped)
            {
                continue;
            }
            var head = tested.Parts[0];
            var hit_element = WhatIsHere(head.Position, new LogicWonszPart[] { head });
            hit_element?.PlayerHit(tested, this);
        }
    }

    void CheckShooters()
    {
        foreach (var wonsz in all_wonsz)
        {
            if (wonsz.ShootLaser)
            {
                wonsz.ChangeLength = -1;
                wonsz.ApplyChangeLength();
                // stwórz punkty trafienia lasera
                Vector2Int[] hitpoints = new Vector2Int[3];
                for (int j = 0; j < hitpoints.Length; j++)
                {
                    var hitpoint = LogicMap.KeepOnMap(wonsz.Parts[0].Position + wonsz.Direction.ToVector2Int() * (j + 1), data.mapSize);
                    var hit = WhatIsHere(hitpoint);
                    hit?.LaserHit(this);
                }
            }
        }
    }
    public void SetChangeLength(LogicWonsz wonsz, int change)
    {
        wonsz.ChangeLength = Mathf.Max(wonsz.ChangeLength + change, data.minLength - wonsz.Parts.Length);
    }
    public bool IsCurrentApple(LogicApple apple)
    {
        return apple == currentApple;
    }
    Vector2Int GetRandomFreePlace()
    {
        Vector2Int newPos;
        do
        {
            newPos = new Vector2Int(Random.Range(0, data.mapSize), Random.Range(0, data.mapSize));
        } while (!IsPlaceFree(newPos));
        return newPos;
    }
    bool IsPlaceFree(Vector2Int position)
    {
        return WhatIsHere(position) == null;
    }
    LogicItemOnMap WhatIsHere(Vector2Int position, LogicItemOnMap[] ignored = null)
    {
        ignored = ignored ?? new LogicItemOnMap[0];
        bool checkElement(LogicItemOnMap element) => element != null && element.Position == position && !ignored.Contains(element);
        foreach (var wonsz in all_wonsz)
        {
            foreach (var part in wonsz.Parts)
            {
                if (checkElement(part))
                {
                    return part;
                }
            }
        }
        foreach (var apple in Apples)
        {
            if (checkElement(apple))
            {
                return apple;
            }
        }
        foreach (var wall in Walls)
        {
            if (checkElement(wall))
            {
                return wall;
            }
        }
        if (checkElement(currentApple))
        {
            return currentApple;
        }
        return null;
    }
    public NetResults[] GetAllResults()
    {
        NetResults[] result = new NetResults[all_wonsz.Count];
        for (int i = 0; i < all_wonsz.Count; i++)
        {
            result[i] = all_wonsz[i].Results;
        }
        return result;
    }
    static public Vector2Int KeepOnMap(Vector2Int pos, int size)
    {
        return new Vector2Int((int)Mathf.Repeat(pos.x, size), (int)Mathf.Repeat(pos.y, size));
    }
}
