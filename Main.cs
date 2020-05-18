using AviFav_;
using AviFav_.Config;
using Harmony;
using MelonLoader;
using NET_SDK.Harmony;
using NET_SDK.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.UI;

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
