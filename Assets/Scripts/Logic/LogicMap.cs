using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicMap
{
    WonszykServerData data;
    // ile musi zostać odcięte węża żeby stać się jabłkiem, a nie ścianą
    int frame = 0;
    LogicWonsz[] all_wonsz;
    // ściana z pozycją (-1, -1) nie istnieje
    List<LogicWall> Walls;
    List<LogicApple> Apples;
    LogicApple currentApple;

    public LogicMap(WonszykServerData data_, Dictionary<uint, Player> wonszyki)
    {
        Apples = new List<LogicApple>();
        Walls = new List<LogicWall>();
        data = data_;
        PlaceWonsz(wonszyki);
        currentApple = new LogicApple(GetRandomFreePlace());
    }

    void PlaceWonsz(Dictionary<uint, Player> wonszyki)
    {
        all_wonsz = new LogicWonsz[wonszyki.Count];
        int i = 0;
        foreach(var wonsz in wonszyki)
        {
            all_wonsz[i] = wonsz.Value.mywonsz.ToAbstract();
            all_wonsz[i].PlayerId = wonsz.Key;
            all_wonsz[i].Results.playerId = wonsz.Key;
            i++;
        }
    }

    public LogicWonsz[] All_wonsz    
    {
        get
        {
            return all_wonsz;
        }

        set
        {
            all_wonsz = value;
        }
    }

    public List<LogicWall> Walls1
    {
        get
        {
            return Walls;
        }

        set
        {
            Walls = value;
        }
    }

    public List<LogicApple> Apples1
    {
        get
        {
            return Apples;
        }

        set
        {
            Apples = value;
        }
    }

    public LogicApple CurrentApple
    {
        get
        {
            return currentApple;
        }

        set
        {
            currentApple = value;
        }
    }

    public void Simulate(List<uint> shooters)
    {
        frame++;
        foreach(var wonsz in all_wonsz)
        {
            wonsz.Reset();
            wonsz.Stopped = wonsz.stoppedTill > frame;
            
            if (!wonsz.Stopped)
            {
                wonsz.ShootLaser = shooters.Contains(wonsz.PlayerId);
            }
        }
        CheckShooters();
        foreach(var wonsz in all_wonsz)
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
                wonsz.stoppedTill = frame + data.framesStopped;
            }
            wonsz.ApplyChangeLength();
            if(wonsz.Ate == EatenApple.normal)
            {
                currentApple.Position = GetRandomFreePlace();
            }
        }
    }

    void CheckCollisions()
    {
        // Check all wonszes
        for (int i=0; i<all_wonsz.Length; i++)
        {
            LogicWonsz tested = all_wonsz[i];
            // if not moving not colliding
            if (tested.Stopped)
            {
                continue;
            }
            // if head
            Vector2Int head = tested.Positions[0].Position;
            // collides with wonszes
            for (int j=0; j<all_wonsz.Length; j++)
            {
                // every bit of them
                for (int k = 0; k < all_wonsz[j].Positions.Length; k++)
                {
                    // but not itself
                    if (i == j && k == 0)
                    {
                        continue;
                    }
                    Vector2Int current = all_wonsz[j].Positions[k].Position;
                    if(head.Equals(current))
                    {
                        // set it on wonsz
                        tested.Collide = true;
                        SetChangeLength(tested, -1);
                    }
                }
            }
            // collides with walls
            for (int j = 0; j < Walls.Count; j++)
            {
                if (head.Equals(Walls[j]))
                {
                    // eat it
                    tested.Collide = true;
                    SetChangeLength(tested, -1);
                }
            }
            // collides with apples
            for (int j = 0; j < Apples.Count; j++)
            {
                if (head.Equals(Apples[j]) && !tested.Collide)
                {
                    // eat it
                    tested.Ate = EatenApple.players;
                    Apples.RemoveAt(j);
                    j = 0;
                    SetChangeLength(tested, 1);
                }
            }
            if(head.Equals(currentApple) && !tested.Collide)
            {
                // eat it
                tested.Ate = EatenApple.normal;
                SetChangeLength(tested, 1);
            }
        }
    }

    void CheckShooters()
    {
        // każdy z węży
        foreach (var wonsz in all_wonsz)
        {
            // jeżeli strzela
            if (wonsz.ShootLaser)
            {
                wonsz.ChangeLength = -1;
                wonsz.ApplyChangeLength();
                // stwórz punkty trafienia lasera
                Vector2Int[] hitpoints = new Vector2Int[3];
                for (int j = 0; j < hitpoints.Length; j++)
                {
                    var hitpoint = ItemOnMap.KeepOnMap(wonsz.Positions[0].Position + wonsz.Direction.ToVector2Int()*(j+1), data.mapSize);
                    var hit = WhatIsHere(hitpoint);
                    hit?.LaserHit(this);
                }
                // sprawdź trafienia na wszystkich wężach (tak, na sobie też)
                for (int k = 0; k < all_wonsz.Length; k++)
                {
                    List<int> hit = all_wonsz[k].IndexHit(hitpoints);
                    // jeśli trafił, utnij węża
                    if (hit.Count>0)
                    {
                        var cut = all_wonsz[k].Cut(hit, Mathf.Max(hit[0], data.minLength));
                        all_wonsz[k].ShotHit = all_wonsz[k].ChangeLength;

                        if (cut.Length > data.lenStillApples)
                        {
                            for (int j = 0; j < cut.Length; j++)
                            {
                                Walls.Add(new LogicWall(cut[j]));
                            }
                        }
                        else
                        {
                            for (int j = 0; j < cut.Length; j++)
                            {
                                Apples.Add(new LogicApple(cut[j]));
                            }
                        }
                    }
                }
            }
        }
    }

    void SetChangeLength(LogicWonsz wonsz, int change)
    {
        wonsz.ChangeLength = Mathf.Max(wonsz.ChangeLength + change, data.minLength - wonsz.Positions.Length);
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
        return WhatIsHere(position) != null;
    }
    LogicItemOnMap WhatIsHere(Vector2Int position){
        foreach(var wonsz in all_wonsz)
        {
            foreach (var pos in wonsz.Positions)
            {
                if(pos.Position == position)
                {
                    return pos;
                }
            }
        }
        foreach (var apple in Apples)
        {
            if (apple.Position == position)
            {
                return apple;
            }
        }
        foreach (var wall in Walls)
        {
            if (wall.Position == position)
            {
                return wall;
            }
        }
        return null;
    }
    public NetResults[] GetAllResults()
    {
        NetResults[] result = new NetResults[all_wonsz.Length];
        for (int i = 0; i<all_wonsz.Length;i++)
        {
            result[i] = all_wonsz[i].Results;
        }
        return result;
    }
}
