using System.Collections.Generic;

using Sandbox;

using TTTReborn.Globals;
using TTTReborn.Player;

namespace TTTReborn.Rounds
{
    public class PostRound : BaseRound
    {
        public override string RoundName => "Post";
        public override int RoundDuration => Gamemode.Game.TTTPostRoundTime;

        private readonly List<TTTPlayer> _spectators = new();

        protected override void OnTimeUp()
        {
            base.OnTimeUp();

            // TODO: Allow users to close the menu themselves using mouse cursor.
            RPCs.ClientClosePostRoundMenu();

            Gamemode.Game.Instance.ChangeRound(new PreRound());
        }

        public override void OnPlayerSpawn(TTTPlayer player)
        {
            if (Players.Contains(player))
            {
                return;
            }

            player.MakeSpectator(false);

            AddPlayer(player);

            base.OnPlayerSpawn(player);
        }

        public override void OnPlayerKilled(TTTPlayer player)
        {
            Players.Remove(player);
            _spectators.Add(player);

            player.MakeSpectator();
        }

        protected override void OnStart()
        {
            if (Host.IsServer)
            {
                using (Prediction.Off())
                {
                    foreach (TTTPlayer player in Utils.GetPlayers())
                    {
                        RPCs.ClientSetRole(player, player.Role.Name);

                        // TODO move this to a method called after OnKilled() and use LifeState instead of Health
                        player.GetClientOwner()?.SetScore("alive", player.Health > 0);
                    }
                }
            }
        }
    }
}
