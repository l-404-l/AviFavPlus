using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AviFav_.Config
{
    public class Config
    {
        public static string MainFolder = "404Mods/AviFavorites";
        public static string ModConfig = MainFolder + "/config.json";
        public static string AvatarLists = MainFolder + "/avatars.json";

        public static Config CFG;

        public string CustomName = "Extended Favorites";
        public string AddFavoriteTXT = "+Favorite";
        public string RemoveFavoriteTXT = "-Favorite";
        public bool Public = true;
        public bool Custom = true;

        public static List<SavedAvi> DAvatars = new List<SavedAvi> {

            new SavedAvi()
            {
                Name = "Robot",
                AvatarID = "avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11",
                ThumbnailImageUrl = "https://d348imysud55la.cloudfront.net/Avatar-Robot-Image-2017415f1_3_a.file_0e8c4e32-7444-44ea-ade4-313c010d4bae.1.png",
            }
        };

        public static void SaveConfig()
        {
            if (CFG != null)
                File.WriteAllText(ModConfig, JsonConvert.SerializeObject(CFG, Formatting.Indented));
        }
        public static void UpdateAvatars()
        {
            if (CFG != null)
                File.WriteAllText(AvatarLists, JsonConvert.SerializeObject(DAvatars, Formatting.Indented));
        }

        public static void LoadConfig()
        {
            Directory.CreateDirectory(MainFolder);

            if (!File.Exists(ModConfig))
            {
                CFG = new Config();
                SaveConfig();

            }
            else
            {
                CFG = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ModConfig));
                if (CFG == null)
                    CFG = new Config();
                SaveConfig();
            }
            
            if (!File.Exists(AvatarLists))
                File.WriteAllText(AvatarLists, JsonConvert.SerializeObject(DAvatars, Formatting.Indented));
            else
                DAvatars = JsonConvert.DeserializeObject<List<SavedAvi>>(File.ReadAllText(AvatarLists));
        }
    }
}
