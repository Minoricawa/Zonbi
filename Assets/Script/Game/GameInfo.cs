using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GameInfo
{
    static GameStatus now_game_status_ = GameStatus.None;

    // 以下プロパティ.
    public static GameStatus NowGameStatus
    {
        set { now_game_status_ = value; }
        get { return now_game_status_; }
    }

    public enum GameStatus
    {
        Play,
        GamgeOver,
        Pause,
        Select,
        None

    }
}
