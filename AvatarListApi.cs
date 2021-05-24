using AviFav_.Config;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using static AviFav_.Config.Config;

namespace AviFav_
{
    public static class AvatarListHelper
    {
        // Will try and fix the bugs soon

        //Main List Mehtod
        public static void Refresh(this UiAvatarList value, IEnumerable<string> list)
        {

            value.field_Private_Dictionary_2_String_ApiAvatar_0.Clear();
            foreach (var t in list)
            {
                if (!value.field_Private_Dictionary_2_String_ApiAvatar_0.ContainsKey(t))
                    value.field_Private_Dictionary_2_String_ApiAvatar_0.Add(t, null);
            }
            value.field_Public_ArrayOf_0 = list.ToArray();
            value.Method_Protected_Virtual_Void_Int32_0(0);
        }

        public static void FirstLoad(this UiAvatarList value, List<SavedAvi> list)
        {
            int deleted = 0;
            value.field_Private_Dictionary_2_String_ApiAvatar_0.Clear();
            for (int i = 0; i < list.Count(); i++)
            {
                var t = list[i];
                var avatar = new ApiAvatar() { id = t.AvatarID, name = t.Name, thumbnailImageUrl = t.ThumbnailImageUrl };
                avatar.Get(new Action<ApiContainer>(x =>
                {
                    var avi = x.Model as ApiAvatar;
                    if (avatar.releaseStatus == "private")
                    {
                        deleted++;
                        list.Remove(t);
                        return;
                    }
                    else
                    {
                        if (!value.field_Private_Dictionary_2_String_ApiAvatar_0.ContainsKey(t.AvatarID))
                            value.field_Private_Dictionary_2_String_ApiAvatar_0.Add(t.AvatarID, avatar);
                    }
                }));
            }
            if (deleted > 0)
            {
                MelonLogger.Msg($"Deleted {deleted} private avatars.");
                DAvatars = list;
                UpdateAvatars();
            }
            value.field_Public_ArrayOf_0 = list.Select(x => x.AvatarID).ToArray();
            //value.Method_Protected_Virtual_Void_Int32_0(0);
            value.Method_Protected_Virtual_Void_Int32_0(0);

        }

        public static bool AvatarListPassthru(ApiAvatar avi)
        {
            if (avi.releaseStatus == "private" || avi == null)
            {
                return false;
            }
            if (!DAvatars.Any(v => v.AvatarID == avi.id))
            {
                DAvatars.Add(new SavedAvi()
                {
                    AvatarID = avi.id,
                    Name = avi.name,
                    ThumbnailImageUrl = avi.thumbnailImageUrl,
                });
            }
            else
            {
                DAvatars.RemoveAll(v => v.AvatarID == avi.id);
            }

            UpdateAvatars();
            return true;
        }

        //For other lists if needed.
        public static void Refresh(this UiAvatarList value, List<string> list)
        {
            value.field_Private_Dictionary_2_String_ApiAvatar_0.Clear();
            foreach (var t in list)
            {
                value.field_Private_Dictionary_2_String_ApiAvatar_0.Add(t, null);
            }
            value.field_Public_ArrayOf_0 = list.ToArray();
            value.Method_Protected_Virtual_Void_Int32_0(0);
        }
    }
    public class AvatarListApi
    {
        private static UiAvatarList aviList = null;
        public GameObject GameObj;
        public UiAvatarList AList;
        public Button ListBtn;
        public Text ListTitle;


        public static UiAvatarList AviList
        {
            get
            {
                if (aviList == null)
                {
                    var pageAvatar = GameObject.Find("/UserInterface/MenuContent/Screens/Avatar");
                    var vlist = pageAvatar.transform.Find("Vertical Scroll View/Viewport/Content");
                    var updatethis = vlist.transform.Find("Favorite Avatar List").gameObject;
                    updatethis = UnityEngine.Object.Instantiate(updatethis, updatethis.transform.parent);
                    var avText = updatethis.transform.Find("Button");                                   // I make a invis list because 1 doesn't activate or have anything 
                    avText.GetComponentInChildren<Text>().text = "New List";                            // running and its just easier to copy / less todo on copy.
                    var UpdateValue = updatethis.GetComponent<UiAvatarList>();
                    UpdateValue.field_Public_EnumNPublicSealedvaInPuMiFaSpClPuLiCrUnique_0 = UiAvatarList.EnumNPublicSealedvaInPuMiFaSpClPuLiCrUnique.SpecificList;
                    UpdateValue.StopAllCoroutines();
                    updatethis.SetActive(false);
                    aviList = UpdateValue;
                }
                return aviList;
            }
        }

        public static AvatarListApi Create(string listname, int index)
        {
            var list = new AvatarListApi();
            list.GameObj = UnityEngine.Object.Instantiate(AviList.gameObject, AviList.transform.parent);
            list.GameObj.transform.SetSiblingIndex(index);
            list.AList = list.GameObj.GetComponent<UiAvatarList>();
            list.ListBtn = list.AList.GetComponentInChildren<Button>();
            list.ListTitle = list.AList.GetComponentInChildren<Text>();
            list.ListTitle.text = listname;
            list.AList.hideWhenEmpty = true;
            list.AList.clearUnseenListOnCollapse = true;
            list.GameObj.SetActive(true);
            return list;
        }

        public void SetAction(Action v)
        {
            ListBtn.onClick = new Button.ButtonClickedEvent();
            ListBtn.onClick.AddListener(v);
        }
    }


    public class AviPButton
    {
        private static GameObject avipbtn = null;
        public GameObject GameObj;
        public Button Btn;
        public Text Title;


        public static GameObject AviPBTN
        {
            get
            {
                if (avipbtn == null)
                {
                    var button = GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Favorite Button");
                    var NewFavPageBTN = UnityEngine.Object.Instantiate(button, button.transform.parent);
                    NewFavPageBTN.GetComponent<Button>().onClick.RemoveAllListeners();
                    NewFavPageBTN.SetActive(false);
                    var pos = NewFavPageBTN.transform.localPosition;
                    NewFavPageBTN.transform.localPosition = new Vector3(pos.x, pos.y + 150f);
                    avipbtn = NewFavPageBTN;
                }
                return avipbtn;
            }
        }

        public static AviPButton Create(string ButtonTitle, float x, float y, bool shownew = false)
        {
            var NBtn = new AviPButton();
            NBtn.GameObj = UnityEngine.Object.Instantiate(AviPBTN.gameObject, AviPBTN.transform.parent);
            NBtn.Btn = NBtn.GameObj.GetComponentInChildren<Button>();
            NBtn.Btn.onClick.RemoveAllListeners();
            var pos = NBtn.GameObj.transform.localPosition;
            NBtn.GameObj.transform.localPosition = new Vector3(pos.x + x, pos.y + (80f * y));
            NBtn.Title = NBtn.GameObj.GetComponentInChildren<Text>();
            NBtn.Title.text = ButtonTitle;
            if (!shownew)
            {
                var t = NBtn.GameObj.GetComponentsInChildren(Il2CppType.Of<Image>()); // I gotta remember to change this back when 4 drops. Image.Il2CppType
                foreach (var pics in t)
                {
                    if (pics.name == "Icon_New")
                        UnityEngine.Object.DestroyImmediate(pics);

                }
            }
            NBtn.GameObj.SetActive(true);
            return NBtn;
        }

        public void SetScale(float size)
        {
            var scale = GameObj.transform.localScale;
            GameObj.transform.localScale = new Vector3(scale.x + size, scale.y + size, scale.z + size);
        }

        public void SetAction(Action v)
        {
            Btn.onClick = new Button.ButtonClickedEvent();
            Btn.onClick.AddListener(v);
        }
    }
}
