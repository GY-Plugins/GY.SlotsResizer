using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Permissions;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.Android;

namespace GY.SlotsResizer
{
    public class SlotsResizer : RocketPlugin<Config>
    {
        public static SlotsResizer Instance;
        public static Config Cfg;
        
        protected override void Load()
        {
            Instance = this;
            Cfg = Configuration.Instance;
            
            U.Events.OnPlayerConnected += EventOnPlayerConnected;
        }

        private void EventOnPlayerConnected(UnturnedPlayer player) => StartCoroutine(nameof(ChangeSlots), player);

        private IEnumerator ChangeSlots(UnturnedPlayer player)
        {
            var permissions = R.Permissions.GetPermissions(player);
            
            if (permissions.Exists(p => p.Name == Cfg.BypassPermission)) yield break;
            
            var data = permissions.Where(p => p.Name.StartsWith("gy.slot.")).ToArray();
            
            yield return new WaitForSeconds(2);
            
            if (!data.Any())
            {
                player.Inventory.tellSize(Provider.server, PlayerInventory.SLOTS, Cfg.DefaultWidth, Cfg.DefaultHeigth);
                yield break;
            }

            var best = data.Select(s => s.Name.Split('.').Last().Split('x').Select(byte.Parse).ToArray())
                .OrderBy(i => i[0] * i[1])
                .Last();
            
            player.Inventory.tellSize(Provider.server, PlayerInventory.SLOTS, best[0], best[1]);
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= EventOnPlayerConnected;
        }
    }
}