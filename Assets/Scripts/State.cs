using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {
    public static readonly int Walk_Left = -1;
    public static readonly int Walk_Right = 0;
    public static readonly int Idle = 1;
    public static readonly int Run = 2;
    public static readonly int Jump = 3;
    public static readonly int Squat = 4;
    public static readonly int JumpBack = 5;
    public static readonly int Attack = 6;
    public static readonly int Injured = 7;//受伤
    public static readonly int StandUp = 8;//起立
    public static readonly int Ready = 9;

    public static readonly int Injured_01 = 1;
    public static readonly int Injured_02 = 2;
    public static readonly int Injured_03 = 3;

    public static readonly int Attack_Punch_Ground = 0;//地面拳
    public static readonly int Attack_Kick_Ground = 1;//地面腿
    public static readonly int Attack_Punch_Sky = 2;//空中拳
    public static readonly int Attack_Kick_Sky = 3;//空中腿
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
