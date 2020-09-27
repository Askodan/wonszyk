﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;

[RequireComponent(typeof(Steering))]
public class Player : PlayerBehavior
{
    public Wonszyk mywonsz;
    public WonszykPlayerData data;
    //private int points = 0;
    public static Dictionary<uint, Player> ActivePlayers = new Dictionary<uint, Player>();
    public Laser laser;
    //static List<Player> list_players = new List<Player>();
    Steering steer;

    //public int Points { get => points; set { points = value;
    //        GameLogic.Instance.networkObject.SendRpc(GameLogic.RPC_PLAYER_POINTS, Receivers.AllBuffered, networkObject.NetworkId, points);
    //    } }

    protected virtual void Awake()
    {
        data = GetComponent<WonszykPlayerData>();
        foreach (WonszykPlayerData wpd in FindObjectsOfType<WonszykPlayerData>())
        {
            if (!wpd.transform.parent)
            {
                wpd.CopyTo(data);
            }
        }
        mywonsz.data = data;
        Steering[] steers = GetComponents<Steering>();
        steer = FindSteeringInArray(steers, data.WonszSteering);
        steer.is_local = data.WonszLocalSteering;
        steer.Init();
    }

    protected virtual void OnDestroy()
    {
        ActivePlayers.Remove(networkObject.NetworkId);
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();
        if (!networkObject.IsOwner)
        {
        }
        else
        {
            // Assign the name when this object is setup on the network
            networkObject.SendRpc(RPC_SET_CUSTOMIZATIONS, Receivers.AllBuffered, data.WonszName, data.WonszColor, (int)(data.WonszGender));
        }
        ActivePlayers.Add(networkObject.NetworkId, this);
        laser.SetDuration(1f / GameLogic.Instance.data.gameSpeed);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (mywonsz.Head)
        {
            mywonsz.Head.Direction = (PlayerDirection)networkObject.Direction;
            if (networkObject.Shoot && !laser.LoadEffect.isPlaying)
            {
                laser.SetPosition(mywonsz.Head);
                laser.LoadLaser();
            }
            if (networkObject.IsOwner)
            {
                PlayerDirection unallowed = mywonsz.GetUnallowedDirection();
                PlayerDirection newDirection = PlayerDirection.None;
                if (steer.is_local)
                {
                    newDirection = steer.Steer(PlayerDirection.None);
                    newDirection = steer.localize(newDirection, unallowed);
                }
                else
                {
                    newDirection = steer.Steer(unallowed);
                }
                if (newDirection != PlayerDirection.None)
                    networkObject.Direction = (int)newDirection;
                networkObject.Shoot = steer.ShootLaser() || networkObject.Shoot;
            }
        }
    }

    public override void SetCustomizations(RpcArgs args)
    {
        data.WonszName = args.GetNext<string>();
        data.WonszColor = args.GetNext<Color>();
        data.WonszGender = (Gender)args.GetNext<int>();
        mywonsz.UpdateColor();
        name = data.WonszName;
        GameLogic.Instance.logMachine.Log(TextBank.Say(name + " ", Texts.join, data.WonszGender, ""));
    }

    public void ShootLaser()
    {
        laser.SetPosition(mywonsz.Head);
        laser.ShootLaser();
    }

    Steering FindSteeringInArray(Steering[] steers, SteeringEnum whichOne)
    {
        foreach (var st in steers) {
            if(st.GetType() == Steering.TypeOfEnum[whichOne])
            {
                return st;
            }
        }
        Debug.LogError("Didn't found steering script of given type");
        return null;
    }
}
