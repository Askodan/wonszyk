using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Net;
using System.Net.Sockets;
using System;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine.SceneManagement;

public enum GameStates
{
    Connection = 0,
    Starting = 1,
    Play = 2,
    Over = 3
}

public class GameLogic : GameLogicBehavior
{
    GameStates state;
    public static GameLogic Instance;
    public Map map;
    public WonszykServerData data;
    bool play = false;
    int frame = 0;
    public LogMachine logMachine;
    [SerializeField] ResultsMachine resultsMachine;
    [SerializeField] StatsMachine statsMachine;
    ItemOnMap currentApple;
    [SerializeField] ArrayOnMap playerApples;
    [SerializeField] ArrayOnMap walls;
    public Button laser;
    [SerializeField] Button StarterButton;
    AbstractMap tempMap = null;
    public override void GameStart(RpcArgs args)
    {
        MainThreadManager.Run(() =>
        {
            if(state!=GameStates.Starting )
                StartCoroutine(StartSequence());
        });
    }
    
    public override void PlayerPoints(RpcArgs args)
    {
        resultsMachine.Log(args.GetNext<uint>(), args.GetNext<int>());
    }

    public override void WonszPosition(RpcArgs args)
    {
        byte[] all_data = args.GetNext<byte[]>();
        NetWonsz[] wonsze = DivideAllWonszPacket(all_data);
        foreach (var wonsz in wonsze)
        {
            var wonszyk = Player.ActivePlayers[wonsz.playerId];
            wonszyk.mywonsz.FromNetWonsz(wonsz);
            if (wonsz.ate) logMachine.Log(TextBank.Say(wonszyk.name + " ", Texts.meal, wonszyk.data.WonszGender, ""));
            if (wonsz.shot) { logMachine.Log(TextBank.Say(wonszyk.name + " ", Texts.shot, wonszyk.data.WonszGender, ""));  wonszyk.ShootLaser(); }
            if (wonsz.collide) logMachine.Log(TextBank.Say(wonszyk.name + " ", Texts.hit, wonszyk.data.WonszGender, ""));
            resultsMachine.Log(wonsz.playerId, wonsz.points);
        }
        var w = args.GetNext<byte[]>();
        walls.UpdatePositions(ItemOnMap.Bytes2Points(w));
        var a = args.GetNext<byte[]>();
        playerApples.UpdatePositions(ItemOnMap.Bytes2Points(a));
    }

    public override void Message(RpcArgs args)
    {
        logMachine.Log(args.GetNext<string>());
    }

    public override void MakeNewApple(RpcArgs args)
    {
        Vector2Int pos = new Vector2Int(args.GetNext<int>(), args.GetNext<int>());
        if (currentApple == null)
        {
            currentApple = Instantiate(data.applePrefab);
        }
        currentApple.Position = pos;
    }

    public override void SetMatchSettings(RpcArgs args)
    {
        data.mapSize = args.GetNext<int>();
        data.minLength = args.GetNext<int>();
        data.startLength = args.GetNext<int>();

        map.Size = data.mapSize;
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();
        if (networkObject.IsServer)
        {
            networkObject.SendRpc(RPC_SET_MATCH_SETTINGS, Receivers.AllBuffered, 
                data.mapSize, 
                data.minLength, 
                data.startLength);
            LocalIPAddress();
        }

    }
    
    public override void CreatePlayer(RpcArgs args)
    {
        NetworkManager.Instance.InstantiatePlayer();
    }

    private void PlayerConnected(NetworkingPlayer player, NetWorker sender)
    {
        MainThreadManager.Run(() =>
        {
            if (state == GameStates.Connection)
            {
                networkObject.SendRpc(player, RPC_CREATE_PLAYER);
                //var result = NetworkManager.Instance.InstantiatePlayer();
                //result.networkObject.AssignOwnership(player);
            }
        });
    }
    private void Awake()
    {
        Instance = this;
        data = FindObjectOfType<WonszykServerData>();
        state = GameStates.Connection;
    }

    private void Start()
    {
        if (NetworkManager.Instance.Networker is IServer)
        {
            PlayerConnected(networkObject.Owner, networkObject.Networker);
            NetworkManager.Instance.Networker.playerConnected += PlayerConnected;
        }
        else
        {
            StarterButton.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (networkObject.IsServer)
        {
            if (state == GameStates.Connection)
            {
                //if (Player.ActivePlayers.Count == data.PlayersCountToStart)
                {
                    state = GameStates.Play;
                    networkObject.SendRpc(RPC_GAME_START, Receivers.AllBuffered);
                    foreach (var player in Player.ActivePlayers)
                    {
                        player.Value.mywonsz.SetLength(data.startLength, map.GetRandomFreePlace());
                    }
                }
            }
        }
    }

    IEnumerator GameFlow()
    {
        while (play)
        {
            yield return new WaitForSeconds(1f / data.gameSpeed);
            frame++;
            // stop shooters if they don't have a fuel
            List<uint> shooters = new List<uint>();
            foreach(var a_wonsz in tempMap.All_wonsz)
            {
                a_wonsz.Direction = Player.ActivePlayers[a_wonsz.PlayerId].mywonsz.Head.Direction;
            }
            foreach (var player in  Player.ActivePlayers)
            {
                if (player.Value.networkObject.Shoot)
                {
                    if (player.Value.mywonsz.GetLength() == data.minLength)
                    {
                        player.Value.networkObject.Shoot = false;
                    }
                    else
                    {
                        shooters.Add(player.Key);
                    }
                }
            }
            tempMap.Simulate(shooters);

            // iteracja dzieje się po intach, ale idą do tablicy, a nie słownika
            for (int i=0; i<Player.ActivePlayers.Count; i++)
            {
                AbstractWonsz a_wonszyk = tempMap.All_wonsz[i];
                Player wonszyk = Player.ActivePlayers[a_wonszyk.PlayerId];
                int PointsChange = a_wonszyk.ShotHit;
                if (a_wonszyk.Ate!=EatenApple.none) {
                    if (a_wonszyk.Ate == EatenApple.normal)
                    {
                        networkObject.SendRpc(RPC_MAKE_NEW_APPLE, Receivers.All, tempMap.CurrentApple.x, tempMap.CurrentApple.y);
                    }
                    PointsChange += data.appleEatenPoints;
                    a_wonszyk.Results.meals += 1;
                }
                if (a_wonszyk.Collide)
                {
                    PointsChange += data.collidePoints;
                    a_wonszyk.Results.hits += 1;
                }
                if (wonszyk.networkObject.Shoot)
                {
                    PointsChange += data.shotPoints;
                    a_wonszyk.Results.shots += 1;
                    wonszyk.networkObject.Shoot = false;
                }
                if (PointsChange != 0)
                {
                    a_wonszyk.Results.points = Mathf.Max(0, a_wonszyk.Results.points + PointsChange);
                }
                if (a_wonszyk.Results.points >= data.pointsToEnd)
                {
                    networkObject.SendRpc(RPC_GAME_OVER, Receivers.AllBuffered, NetResults.ArrayToByteArray(tempMap.GetAllResults()));
                    play = false;
                }
            }
            networkObject.SendRpc(RPC_WONSZ_POSITION, Receivers.All,
                    CreateAllWonszPacket(tempMap.All_wonsz),
                    ItemOnMap.Points2Bytes(tempMap.Walls1.ToArray()),
                    ItemOnMap.Points2Bytes(tempMap.Apples1.ToArray()));
        }
    }

    IEnumerator StartSequence()
    {
        state = GameStates.Starting;
        yield return new WaitForSeconds(0.5f);
        // przed odliczaniem
        if (networkObject.IsServer)
        {
            tempMap = new AbstractMap(data, Player.ActivePlayers);
            networkObject.SendRpc(RPC_WONSZ_POSITION, Receivers.All,
                    CreateAllWonszPacket(tempMap.All_wonsz),
                    ItemOnMap.Points2Bytes(tempMap.Walls1.ToArray()),
                    ItemOnMap.Points2Bytes(tempMap.Apples1.ToArray()));
        }
        for (int i = 0; i < 4; i++)
        {
            logMachine.Log((3-i).ToString());
            yield return new WaitForSeconds(1f);
        }

        foreach(var player in Player.ActivePlayers){
            resultsMachine.Log(player.Key, 0);
            player.Value.networkObject.AuthorityUpdateMode = true;
        }

        logMachine.Log("START!");
        play = true;
        state = GameStates.Play;
        if (networkObject.IsServer)
        {
            networkObject.SendRpc(RPC_MAKE_NEW_APPLE, Receivers.AllBuffered, tempMap.CurrentApple.x, tempMap.CurrentApple.y);
            StartCoroutine(GameFlow());
        }
    }

    public override void GameOver(RpcArgs args)
    {
        statsMachine.gameObject.SetActive(true);

        var a = args.GetNext<byte[]>();
        statsMachine.Log(NetResults.ArrayFromByteArray(a));
    }
    public void MainMenu()
    {
        NetworkManager.Instance.Disconnect();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                logMachine.Log(localIP);
            }
        }
        return localIP;
    }
    byte[] CreateAllWonszPacket(AbstractWonsz[] all_wonsz)
    {
        if (all_wonsz.Length == 0)
        {
            return new byte[0];
        }
        List<byte> result = new List<byte>();
        foreach (var wonsz in all_wonsz)
        {
            result.AddRange(BitConverter.GetBytes(wonsz.PlayerId));
            result.AddRange(CreateWonszDataPacket(wonsz));
            result.Add(255);
        }
        result.RemoveAt(result.Count - 1);
        return result.ToArray();
    }
    NetWonsz[] DivideAllWonszPacket(byte[] input)
    {
        if (input.Length == 0)
        {
            return new NetWonsz[0];
        }
        void Add_wonsz (List<NetWonsz> all_wonsz, List<byte> tempData)
        {
            List<byte> id = tempData.GetRange(0, sizeof(uint));
            tempData.RemoveRange(0, sizeof(uint));
            all_wonsz.Add(ReadWonszDataPacket(tempData.ToArray()));
            all_wonsz[all_wonsz.Count - 1].playerId = BitConverter.ToUInt32(id.ToArray(), 0);
        }
        List<NetWonsz> wonsze = new List<NetWonsz>();
        int d = 0;
        int lastDivider = 0;
        List<byte> temp = new List<byte>();
        foreach(var piece in input)
        {
            temp.Add(piece);
            if (d - lastDivider > 2 + sizeof(uint))
            {
                if (piece == 255)
                {
                    temp.RemoveAt(temp.Count - 1);
                    lastDivider = d + 1;

                    Add_wonsz(wonsze, temp);
                    temp.Clear();
                }
            }
            d++;
        }
        Add_wonsz(wonsze, temp);
        return wonsze.ToArray();
    }
    byte[] CreateWonszDataPacket(AbstractWonsz wonsz)
    {
        byte[] tail = Wonszyk.ToTailString(wonsz.Positions);
        bool[] flags = { wonsz.ShootLaser, wonsz.Collide, wonsz.Ate != EatenApple.none};
        List<byte> result = new List<byte>() { (byte)wonsz.Positions[0].x, (byte)wonsz.Positions[0].y, (byte)wonsz.Results.points, PackFlags(flags)};
        result.AddRange(tail);
        return result.ToArray();
    }
    NetWonsz ReadWonszDataPacket(byte[] input)
    {
        int num_byte_front = 4;
        NetWonsz wonsz = new NetWonsz();
        byte[] tail = new byte[input.Length - num_byte_front];
        for (int i = 0; i < tail.Length; i++)
        {
            tail[i] = input[i + num_byte_front];
        }
        List<PlayerDirection> arrayFromTail = Wonszyk.PosesFromTailString(tail);
        wonsz.directions = arrayFromTail.ToArray();
        wonsz.positions = new Vector2Int[arrayFromTail.Count+1];
        wonsz.positions[0] = new Vector2Int(input[0], input[1]);

        wonsz.points = input[2];

        bool[] flags = UnpackFlags(input[3]);
        wonsz.shot = flags[0];
        wonsz.collide = flags[1];
        wonsz.ate = flags[2];

        for (int i = 1; i < wonsz.positions.Length; i++)
        {
            wonsz.positions[i] = wonsz.positions[i - 1] + arrayFromTail[i - 1].ToVector2Int();
        }

        return wonsz;
    }
    byte PackFlags(bool[] flags)
    {
        BitArray bits = new BitArray(flags);
        byte[] Bytes = new byte[1];
        bits.CopyTo(Bytes, 0);
        return Bytes[0];
    }
    bool[] UnpackFlags(byte input)
    {
        BitArray bits = new BitArray(new byte[]{ input });
        bool[] result = new bool[bits.Count];
        int i = 0;
        foreach (var bit in bits)
        {
            result[i] = (bool)bit;
            i++;
        }
        return result;
    }
    void TestPackingWOnsz()
    {
        AbstractWonsz wonsz = new AbstractWonsz();
        wonsz.Positions = new Vector2Int[] {
            new Vector2Int (7, 1),
            new Vector2Int (7, 2),
            new Vector2Int (7, 3),
            new Vector2Int (6, 3),
            new Vector2Int (5, 3),
            new Vector2Int (5, 2),
            new Vector2Int (5, 1)
        };
        wonsz.ShootLaser = false;
        wonsz.Ate = EatenApple.normal;
        wonsz.Collide = false;
        Debug.Log(wonsz);
        byte[] bytes = CreateWonszDataPacket(wonsz);
        NetWonsz readWonsz = ReadWonszDataPacket(bytes);
        Debug.Log(readWonsz);
    }
}
