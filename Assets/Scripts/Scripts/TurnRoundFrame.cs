using System;
using System.Collections;
using System.Collections.Generic;

public class Pass
{
    //阶段
    RoundEnum order;
    Pass next;

    public RoundEnum Order { get => order;}
    internal Pass Next { get => next; set => next = value; }

    public Pass(RoundEnum order)
    {
        this.order = order;
        Next = null;
    }
}
public class Round
{
    //回合
    Pass cur_pass;//该回合的当前阶段
    Pass head_pass;//头阶段

    public Round()
    {
        head_pass = null;
        foreach (RoundEnum item in Enum.GetValues(typeof(RoundEnum)))
        {
            if (head_pass == null)
            {
                head_pass = new Pass(item);
                cur_pass = head_pass;
            }
            else
            {
                cur_pass.Next = new Pass(item);
                cur_pass = cur_pass.Next;
            }
        }
        cur_pass = head_pass;
    }
}

public class TurnRoundManager
{
    //单例模式
    private static TurnRoundManager instance;

    public static TurnRoundManager Instance
    {
        get
        {
            if (instance == null)
                instance = new TurnRoundManager();
            return instance;
        }
        set { instance = value; }
    }
    public TurnRoundManager() { Init(); }

    List<Round> roundLoop;
    int cur_round_index;//当前回合数
    int cur_player_index;//当前轮到的玩家

    private void Init()
    {
        roundLoop = new List<Round>();
        cur_round_index = 0;
        cur_player_index = 0;
    }

    public void AddPlayer(int pos)
    {

    }
}
