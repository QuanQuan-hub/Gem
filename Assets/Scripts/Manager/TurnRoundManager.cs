using System;
using System.Collections;
using System.Collections.Generic;

public class Pass
{
    //阶段
    RoundEnum order;
    Pass nextPass;

    public RoundEnum Order { get => order;}
    internal Pass Next { get => nextPass; set => nextPass = value; }

    public Pass(RoundEnum order)
    {
        this.order = order;
        Next = null;
    }
}
public class Round:IComparer<Round>
{
    int order;//回合的优先级，玩家初始化回合时赋值
    //回合
    Pass cur_pass;//该回合的当前阶段
    Pass head_pass;//头阶段
    Round nextRound;

    public Round NextRound { get => nextRound; set => nextRound = value; }
    public int Order { get => order;}

    public Round(int order)
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
        this.order = order;
    }

    public int Compare(Round x, Round y)
    {
        return x.Order > y.Order ? 1 : -1;//在这修改排序规则
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

    List<Round> roundList;//存玩家回合的头指针
    Round cur_round;

    private void Init()
    {
        roundList = new List<Round>();
    }

    /// <summary>
    /// 添加玩家的回合，支持动态添加
    /// </summary>
    /// <param name="new_Round"></param>
    public void AddPlayer(Round new_Round)
    {
        roundList.Add(new_Round);
        roundList.Sort();
        for (int i = 0; i < roundList.Count; i++)
        {
            if (i != roundList.Count - 1)
            {
                roundList[i].NextRound = roundList[i + 1];
            }
            else
            {
                roundList[i].NextRound = roundList[0];
            }
        }
    }

    /// <summary>
    /// 删除一玩家的回合，一般是该玩家退出回合轮（死亡、逃跑等）
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool DeletePlayer(Round target)
    {
        if (roundList.Contains(target))
        {
            if (target == cur_round)
            {
                cur_round = target.NextRound;
            }
            roundList.Remove(target);
            return true;
        }
        else
        {
            return false;
        }
    }
}
