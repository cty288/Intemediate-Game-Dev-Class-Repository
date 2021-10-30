using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Week4
{
    public static class SimpleEventSystem {
        /// <summary>
        ///   event triggered when player state changed. Pass old and new state to the callback function
        /// </summary>
        public static Action<PlayerState, PlayerState> OnPlayerStateUpdate;

        public static Action<int,int> OnLifeChange;

        public static Action<int, int> OnDiamondChange;

        public static Action<int, int> OnKeyChange;

        //Pass life added and current level number
        public static Action<int,int> OnGameEnds;

        public static Action OnPlayerRespawn;

        public static Action<ItemType> OnPlayerPickItem;

        public static Action<string> OnShot;

        public static Action OnGameStart;

        public static Action<int, int> OnPlayerFloorChange;

        public static Action OnBossDie;

        public static Action OnEntireGameEnds;
    }
}
