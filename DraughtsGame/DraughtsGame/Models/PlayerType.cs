using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Models
{
    public enum PlayerType : int
    {

        PlayerOne = 1,
        PlayerTwo = 2

       
    }
    public static class PlayerTypeToString
    {
        public static string Description(this PlayerType type)
        {
            switch (type)
            {
                case PlayerType.PlayerOne: return "Player One";
                case PlayerType.PlayerTwo: return "Player Two";
            }
            return "";
        }
    }

}
