using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AviFav_.Config
{
    public class Config
    {
        public static string MainFolder = "404Mods\\AviFavorites";
        public static string ModConfig = "config.json";
        public static string AvatarLists = "avatars.json";

        public static Config CFG;

        public string CustomName = "Extended Favorites";
        public string AddFavoriteTXT = "Add Favorites";
        public string RemoveFavoriteTXT = "Remove Favorites";
        public bool Public = true;
        public bool Custom = true;

        public static List<SavedAvi> DAvatars = new List<SavedAvi> {

            new SavedAvi()
            {
                Name = "Example",
                AvatarID = "avtr_ff9f140f-99e8-419f-817e-b88028b0bff0", //Ider what avatar this is LOL 
                ThumbnailImageUrl = "https://d348imysud55la.cloudfront.net/thumbnails/3665376166.thumbnail-500.png",
            }
        };

        public static void SaveConfig()
        {
            if (CFG != null)
                File.WriteAllText(MainFolder + "//" + ModConfig, JsonConvert.SerializeObject(CFG, Formatting.Indented));
        }
        public static void UpdateAvatars()
        {
            if (CFG != null)
                File.WriteAllText(MainFolder + "//" + AvatarLists, JsonConvert.SerializeObject(DAvatars, Formatting.Indented));
        }

        public static void LoadConfig()
        {
            Directory.CreateDirectory(MainFolder);

            if (!File.Exists(MainFolder + "//" + ModConfig))
            {
                CFG = new Config();
                SaveConfig();

            }
            else
            {
                CFG = JsonConvert.DeserializeObject<Config>(File.ReadAllText(MainFolder + "//" + ModConfig));
                if (CFG == null)
                    CFG = new Config();
                SaveConfig();
            }
            
            if (!File.Exists(MainFolder + "//" + AvatarLists))
            {
                File.WriteAllText(MainFolder + "//" + AvatarLists, JsonConvert.SerializeObject(DAvatars, Formatting.Indented));
            
            }
            else
            {
                DAvatars = JsonConvert.DeserializeObject<List<SavedAvi>>(File.ReadAllText(MainFolder + "//" + AvatarLists));
            }
        }
    }
}
