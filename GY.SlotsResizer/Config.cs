using Rocket.API;
using UnityEngine;

namespace GY.SlotsResizer
{
    public class Config : IRocketPluginConfiguration
    {
        public byte DefaultWidth;
        public byte DefaultHeigth;
        public string BypassPermission;

        public void LoadDefaults()
        {
            DefaultHeigth = 6;
            DefaultWidth = 6;
            BypassPermission = "gy.no-resize";
        }
    }
}