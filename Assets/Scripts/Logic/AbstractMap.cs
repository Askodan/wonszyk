using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractMap
{
    WonszykServerData data;
    // ile musi zostać odcięte węża żeby stać się jabłkiem, a nie ścianą
    int frame = 0;
    AbstractWonsz[] all_wonsz;
    // ściana z pozycją (-1, -1) nie istnieje
    List<Vector2Int> Walls;
    List<Vector2Int> Apples;
    Vector2Int currentApple;

    public AbstractMap(WonszykServerData data_, Dictionary<uint, Player> wonszyki)
    {
        Apples = new List<Vector2Int>();
        Walls = new List<Vector2Int>();
        data = data_;
        PlaceWonsz(wonszyki);
        currentApple = GetRandomFreePlace();
    }

    public AbstractWonsz[] All_wonsz
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

    public List<Vector2Int> Walls1
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

    public List<Vector2Int> Apples1
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

    public Vector2Int CurrentApple
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

    void PlaceWonsz(Dictionary<uint, Player> wonszyki)
    {
        all_wonsz = new AbstractWonsz[wonszyki.Count];
        int i = 0;
        foreach(var wonsz in wonszyki)
        {
            all_wonsz[i] = wonsz.Value.mywonsz.ToAbstract();
            all_wonsz[i].PlayerId = wonsz.Key;
            all_wonsz[i].Results.playerId = wonsz.Key;
            i++;
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
                if (wonsz.ShootLaser)
                {
                    wonsz.ChangeLength = -1;
                    wonsz.ApplyChangeLength();
                }
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
                currentApple = GetRandomFreePlace();
            }
        }
    }

    void CheckCollisions()
    {
        // Check all wonszes
        for (int i=0; i<all_wonsz.Length; i++)
        {
            AbstractWonsz tested = all_wonsz[i];
            // if not moving not colliding
            if (tested.Stopped)
            {
                continue;
            }
            // if head
            Vector2Int head = tested.Positions[0];
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
                    Vector2Int current = all_wonsz[j].Positions[k];
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
        for (int i = 0; i < all_wonsz.Length; i++)
        {
            // jeżeli strzela
            if (all_wonsz[i].ShootLaser)
            {
                // stwórz punkty trafienia lasera
                Vector2Int[] hitpoints = new Vector2Int[3];
                for (int j = 0; j < hitpoints.Length; j++)
                {
                    hitpoints[j] = ItemOnMap.KeepOnMap(all_wonsz[i].Positions[0] + all_wonsz[i].Direction.ToVector2Int()*(j+1), data.mapSize);
                }
                // sprawdź trafienia na wszystkich wężach (tak, na sobie też)
                for (int k = 0; k < all_wonsz.Length; k++)
                {
                    List<int> hit = all_wonsz[k].IndexHit(hitpoints);
                    // jeśli trafił, utnij węża
                    if (hit.Count>0)
                    {
                        all_wonsz[k].Cut(hit, Mathf.Max(hit[0], data.minLength));
                        all_wonsz[k].ShotHit = all_wonsz[k].ChangeLength;

                        if (all_wonsz[k].Free_positions.Length > data.lenStillApples)
                        {
                            for (int j = 0; j < all_wonsz[k].Free_positions.Length; j++)
                            {
                                Walls.Add(all_wonsz[k].Free_positions[j]);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < all_wonsz[k].Free_positions.Length; j++)
                            {
                                Apples.Add(all_wonsz[k].Free_positions[j]);
                            }
                        }
                    }
                }
                // sprawdź trafienia w ściany
                for (int k = Walls.Count-1; k >= 0; k--)
                {
                    for(int j = 0; j<hitpoints.Length; j++)
                    {
                        if (Walls[k].Equals(hitpoints[j]))
                        {
                            Walls.RemoveAt(k);
                            k = 0;
                        }
                    }
                }
            }
        }
    }

    void SetChangeLength(AbstractWonsz wonsz, int change)
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
        foreach(var wonsz in all_wonsz)
        {
            foreach (var pos in wonsz.Positions)
            {
                if(pos == position)
                {
                    return false;
                }
            }
        }
        foreach (var apple in Apples)
        {
            if (apple == position)
            {
                return false;
            }
        }
        foreach (var wall in Walls)
        {
            if (wall == position)
            {
                return false;
            }
        }
        return true;
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
