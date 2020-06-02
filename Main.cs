using AviFav_;
using AviFav_.Config;
using MelonLoader;
using System;
using System.Linq;
using UnityEngine;
using VRC.Core;

namespace AviFavsPlus
{
    public static class BuildInfo
    {
        public const string Name = "Avatar Favorites Plus"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "404#0004"; // Author of the Mod.  (Set as null if none)
        public const string Company = "I am not a company -Kappa-"; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.4.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = "https://github.com/l-404-l"; // Download Link for the Mod.  (Set as null if none)
    }

    public class AviFavPlus : MelonMod
    {
        
        public static AvatarListApi CustomList;
        public static AviPButton FavoriteButton;
        public override void OnApplicationStart()
        {
            Config.LoadConfig(); //Needs to load the configs.
            var txt_path = new FileInfo(Path.Combine(Assembly.GetExecutingAssembly().Location, "avatars.txt"));
            if (txt_path.Exists)
            {
                var txt_avis = File.ReadAllLines().ToList().Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Trim()).ToList();
                foreach (var avi in txt_avis)
                {
                    var hasAvi = AviFav_.Config.Config.DAvatars.Select(a => a.AvatarID == avi).FirstOrDefault();
                    if (!hasAvi)
                    {
                        AviFav_.Config.Config.DAvatars.Add(new AviFav_.Config.Config.SavedAvi() { Name = avi, AvatarID = avi, ThumbnailImageUrl = "" });
                    }
                }
            }
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnGUI()
        {

        }
        
        public override void OnLateUpdate()
        {

        }

        
        public override void OnModSettingsApplied()
        {

        }
        
        public override void OnUpdate()
        {

        }

        //string , GameObject , VRCSDK2.Validation.Performance.Stats.AvatarPerformanceStats

        public override void VRChat_OnUiManagerInit()
        {
            if (Config.CFG.Custom)
            {
                CustomList = AvatarListApi.Create(Config.CFG.CustomName + " / " + Config.DAvatars.Count, 1);
                CustomList.AList.FirstLoad(Config.DAvatars);


                CustomList.AList.avatarPedestal.field_Internal_Action_3_String_GameObject_AvatarPerformanceStats_0 = new Action<string, GameObject, VRCSDK2.Validation.Performance.Stats.AvatarPerformanceStats>((x, y, z) =>
                {
                    if (Config.DAvatars.Any(v => v.AvatarID == CustomList.AList.avatarPedestal.field_Internal_ApiAvatar_0.id))
                    {
                        FavoriteButton.Title.text = Config.CFG.RemoveFavoriteTXT;
                        CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
                    }
                    else
                    {
                        FavoriteButton.Title.text = Config.CFG.AddFavoriteTXT;
                        CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
                    }

                });
                
                //Add-Remove Favorite Button
                FavoriteButton = AviPButton.Create(Config.CFG.AddFavoriteTXT, 0f, 9.6f);
                FavoriteButton.SetAction(() =>
                {
                    var avatar = CustomList.AList.avatarPedestal.field_Internal_ApiAvatar_0;
                    if (avatar.releaseStatus != "private")
                    {
                        if (!Config.DAvatars.Any(v => v.AvatarID == avatar.id))
                        {
                            AvatarListHelper.AvatarListPassthru(avatar);
                            CustomList.AList.Refresh(Config.DAvatars.Select(x => x.AvatarID).Reverse());
                            FavoriteButton.Title.text = Config.CFG.RemoveFavoriteTXT;
                            CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
                        }
                        else
                        {

                            AvatarListHelper.AvatarListPassthru(avatar);
                            CustomList.AList.Refresh(Config.DAvatars.Select(x => x.AvatarID).Reverse());
                            FavoriteButton.Title.text = Config.CFG.AddFavoriteTXT;
                            CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
                        }
                    }
                });

                //Author Button
                var t = AviPButton.Create("Show Author", 320f, 9.6f);
                t.SetScale(-.1f);
                t.SetAction(() =>
                {
                    VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_0(true);
                    APIUser.FetchUser(CustomList.AList.avatarPedestal.field_Internal_ApiAvatar_0.authorId, new Action<APIUser>(x =>
                    {

                        QuickMenu.prop_QuickMenu_0.prop_APIUser_0 = x;
                        QuickMenu.prop_QuickMenu_0.Method_Public_Void_Int32_Boolean_0(4, false);


                    }), null);
                });
            }
        }
    }
}
