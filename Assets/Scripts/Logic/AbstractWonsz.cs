using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EatenApple
{
    none,
    normal,
    players
}
public class AbstractWonsz
{
    //input
    uint playerId;
    NetResults results =  new NetResults();
    Vector2Int[] positions;
    Vector2Int[] old_positions;
    Vector2Int[] free_positions;
    PlayerDirection direction;
    bool shootLaser = false;
    int changeLength = 0;
    
    //output
    int shotHit = 0;
    bool stopped = false;
    bool collide = false;
    EatenApple ate = EatenApple.none; // index of eaten apple

    // state
    public int stoppedTill = 0;


    public void Reset()
    {
        shotHit = 0;
        stopped = false;
        collide = false;
        ate = EatenApple.none;
        changeLength = 0;
        old_positions = new Vector2Int[positions.Length];
        for(int i = 0; i < old_positions.Length; i++)
        {
            old_positions[i] = new Vector2Int(positions[i].x, positions[i].y);
        }
    }
    public Vector2Int[] Positions
    {
        get
        {
            return positions;
        }

        set
        {
            positions = value;
        }
    }

    public PlayerDirection Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }

    public bool Collide
    {
        get
        {
            return collide;
        }

        set
        {
            collide = value;
        }
    }

    public uint PlayerId
    {
        get
        {
            return playerId;
        }

        set
        {
            playerId = value;
        }
    }

    public bool Stopped
    {
        get
        {
            return stopped;
        }

        set
        {
            stopped = value;
        }
    }

    public int ChangeLength
    {
        get
        {
            return changeLength;
        }

        set
        {
            changeLength = value;
        }
    }

    public bool ShootLaser
    {
        get
        {
            return shootLaser;
        }

        set
        {
            shootLaser = value;
        }
    }

    public int ShotHit
    {
        get
        {
            return shotHit;
        }

        set
        {
            shotHit = value;
        }
    }

    public Vector2Int[] Free_positions
    {
        get
        {
            return free_positions;
        }

        set
        {
            free_positions = value;
        }
    }

    public EatenApple Ate
    {
        get
        {
            return ate;
        }

        set
        {
            ate = value;
        }
    }
    
    public NetResults Results
    {
        get
        {
            return results;
        }

        set
        {
            results = value;
        }
    }

    public void MoveForward(int MapSize)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            int index = positions.Length - i - 1;
            Vector2Int bodyPart = positions[index];
            if (index > 0)
            {
                positions[index] = positions[index - 1];
            }
        }
        
        positions[0] = ItemOnMap.KeepOnMap(positions[0] + direction.ToVector2Int(), MapSize);
    }
    public void BackOff()
    {
        positions = old_positions;
    }
    // zwraca pierwszy trafiony indeks przez tablice punktów
    public List<int> IndexHit(Vector2Int[] pos)
    {
        List<int> hits = new List<int>();
        for (int k = 0; k < Positions.Length; k++)
        {
            for (int i = 0; i < pos.Length; i++)
            { 
                if (pos[i].Equals(Positions[k]))
                {
                    hits.Add(k);
                }
            }
        }
        return hits;
    }
    public void Cut(List<int> indexOfCut, int newLength)
    {
        ChangeLength = newLength - Positions.Length;
        List<Vector2Int> stays = new List<Vector2Int>();
        List<Vector2Int> free = new List<Vector2Int>();
        for(int i = 0; i < positions.Length; i++)
        {
            if (i< newLength)
            {
                stays.Add(positions[i]);
            }else if (i > newLength)
            {
                if (!indexOfCut.Exists(p => p.Equals(i))) {
                    free.Add(positions[i]);
                }
            }
        }
        Free_positions = free.ToArray();
        positions = stays.ToArray();
    }
    public void ApplyChangeLength()
    {
        if (ChangeLength != 0) {
            //Debug.Log("zmiana długości " + old_positions.Length + " + " + ChangeLength);
            Vector2Int[] newpos = new Vector2Int[old_positions.Length+ChangeLength];
            // Copy same part
            int tocopy = Mathf.Min(newpos.Length, positions.Length);
            for (int i = 0; i < tocopy; i++)
            {
                newpos[i] = positions[i];
            }
            if (ChangeLength > 0)
            {
                for (int i = tocopy; i < newpos.Length; i++)
                {
                    if(i < old_positions.Length-1)
                    {
                        newpos[i] = old_positions[i-1];
                    }
                    else
                    {
                        newpos[i] = old_positions[old_positions.Length - 1];
                    }
                }
            }
            positions = newpos;
        }
    }
    public override string ToString()
    {
        string result = "";
        foreach(var pos in positions)
        {
            result += "(" + pos.x + "," + pos.y + ") ";
        }
        result += ShootLaser ? "shot " : "not shot ";
        result += Ate != EatenApple.none ? "ate " : "not ate ";
        result += Collide ? "hit " : "not hit ";
        return result;
    }
}
