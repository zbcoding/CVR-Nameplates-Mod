using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using UnityEngine;

namespace CVRNameplates
{

    public class Json
    {
        public int[] DefaultColor { get; set; }
        public int[] FriendsColor { get; set; }
        public int[] LegendColor { get; set; }
        public int[] DevColor { get; set; }
        public int[] ModColor { get; set; }
        public int[] GuideColor { get; set; }

        public string Background { get; set; }
        public string Icon { get; set; }
        public string MicIconOn { get; set; }
        public string MicIconOff { get; set; }
        public string Friend { get; set; }
    }

    static class UserRanks
    {
        public const string Mod = "Moderator";
        public const string Dev = "Developer";
        public const string Guide = "Community Guide";
        public const string Legend = "Legend";
    }

    internal class Config
    {
        public Json Js { get; set; }
        public Color DefaultColor { get; set; }
        public Color FriendsColor { get; set; }
        public Color DevColor { get; set; }
        public Color ModColor { get; set; }
        public Color GuideColor { get; set; }
        public Color LegendColor { get; set; }

        public Config()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "//CVR-Nameplates"))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "//CVR-Nameplates");

            if (!File.Exists(Directory.GetCurrentDirectory() + "//CVR-Nameplates//PlatesManagerConfig.Json"))
            {
                using (WebClient wc = new WebClient())
                {
                    File.WriteAllText(Directory.GetCurrentDirectory() + "//CVR-Nameplates//PlatesManagerConfig.Json", JsonConvert.SerializeObject(new Json()
                    {
                        DefaultColor = new int[] { 0, 128, 255 }, //blue
                        FriendsColor = new int[] { 0, 255, 128 }, //green
                        ModColor = new int[] { 255, 0, 0 }, //red
                        DevColor = new int[] { 229, 17, 15 }, //cvr orange
                        GuideColor = new int[] { 102, 0, 204 }, //purple
                        LegendColor = new int[] { 255, 153, 51 }, //orange
                        Background = Convert.ToBase64String(wc.DownloadData("https://raw.githubusercontent.com/zbcoding/CVR-Nameplates-Mod/main/Icons/namepalte%20(1).png")),
                        Icon = Convert.ToBase64String(wc.DownloadData("https://raw.githubusercontent.com/zbcoding/CVR-Nameplates-Mod/main/Icons/iconbackground.png")),
                        MicIconOn = Convert.ToBase64String(wc.DownloadData("https://raw.githubusercontent.com/zbcoding/CVR-Nameplates-Mod/main/Icons/Mic%20On.png")),
                        MicIconOff = Convert.ToBase64String(wc.DownloadData("https://raw.githubusercontent.com/zbcoding/CVR-Nameplates-Mod/main/Icons/micoff.png")),
                        Friend = Convert.ToBase64String(wc.DownloadData("https://raw.githubusercontent.com/zbcoding/CVR-Nameplates-Mod/main/Icons/friendIcon.png")),
                    }));
                    wc.Dispose();
                }
            }
            Js = JsonConvert.DeserializeObject<Json>(File.ReadAllText(Directory.GetCurrentDirectory() + "//CVR-Nameplates//PlatesManagerConfig.Json"));
            DefaultColor  = new Color32(byte.Parse(Js.DefaultColor[0].ToString()), byte.Parse(Js.DefaultColor[1].ToString()), byte.Parse(Js.DefaultColor[2].ToString()), 170);
            FriendsColor = new Color32(byte.Parse(Js.FriendsColor[0].ToString()), byte.Parse(Js.FriendsColor[1].ToString()), byte.Parse(Js.FriendsColor[2].ToString()), 170);

        }

    }
}
