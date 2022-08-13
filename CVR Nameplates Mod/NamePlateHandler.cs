using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABI_RC.Core.Player;
using ABI_RC.Core.Networking;
using ABI_RC.Core.Networking.IO.Social;
using UnityEngine;

namespace CVRNameplates
{
    internal class NamePlateHandler : MonoBehaviour, IDisposable
    {
        private GameObject _maskGameObject { get; set; }
        private UnityEngine.UI.Image _maskImageComp { get; set; }
        private GameObject _backgroundGameObject { get; set; }
        public UnityEngine.UI.Image BackgroundImageComp { get; set; }
        public UnityEngine.UI.Image BackgroundMask { get; set; }
        private GameObject _backgroundGameObj { get; set; }
        private GameObject _friendIcon { get; set; }
        public GameObject MicOn { get; set; }
        public GameObject MicOff { get; set; }
        public Color UserColor { get; set; }
        private Color _micOnColor { get; set; }
        private UnityEngine.UI.Image _micOffImage{ get; set; }
        private UnityEngine.UI.Image _micOnImage { get; set; }
        private UnityEngine.UI.Image _friend { get; set; }

        void Start() => InvokeRepeating(nameof(Setup), -1, 0.3f);
         private void Setup()
         {
            try
            {
                if (this.transform.Find("Canvas/Content/Image/ObjectMaskSlave/UserImageMask (1)") == null) return;
            }
            catch { return; }

            //set color of nameplate depending on user
            String userRank = transform.parent.gameObject.GetComponent<PlayerDescriptor>().userRank;
            if (userRank == UserRanks.Mod)
            {
                UserColor = Main.s_config.ModColor;
            }
            else if (userRank == UserRanks.Dev)
            {
                UserColor = Main.s_config.DevColor;                                         
            }
            else if (userRank == UserRanks.Guide)
            {
                UserColor = Main.s_config.GuideColor;
            }
            else if (userRank == UserRanks.Legend)
            {
                UserColor = Main.s_config.LegendColor;
            }
            //developers and other special ranks will keep their color
            //friend users will get friend color
            else if (Friends.FriendsWith(transform.parent.gameObject.GetComponent<PlayerDescriptor>().ownerId)
                    && transform.parent.gameObject.GetComponent<PlayerDescriptor>().userRank == UserRanks.User)
            {
                UserColor = Main.s_config.FriendsColor;
            }
            else UserColor = Main.s_config.DefaultColor;
      
            _backgroundGameObject = this.transform.Find("Canvas/Content/Image").gameObject;
            Component.DestroyImmediate(_backgroundGameObject.GetComponent<UnityEngine.UI.Image>());

            //nameplate outline
            BackgroundImageComp = _backgroundGameObject.AddComponent<UnityEngine.UI.Image>();
            BackgroundImageComp.type = UnityEngine.UI.Image.Type.Sliced;
            BackgroundImageComp.pixelsPerUnitMultiplier = 500;
            BackgroundImageComp.color = UserColor;
            BackgroundImageComp.ChangeSpriteFromString(Main.s_config.Js.Background, 200, new Vector4(255, 0, 255, 0));

            _maskGameObject = this.transform.Find("Canvas/Content/Image/ObjectMaskSlave/UserImageMask").gameObject;
            Component.DestroyImmediate(_maskGameObject.GetComponent<UnityEngine.UI.Image>());

            _maskImageComp = _maskGameObject.AddComponent<UnityEngine.UI.Image>();
            _maskImageComp.ChangeSpriteFromString(Main.s_config.Js.Icon);
            _maskGameObject.transform.localScale = new Vector3(1.25f, 1.1f, 1);
            Component.DestroyImmediate(_maskGameObject.GetComponent<UnityEngine.UI.Mask>());

            _maskGameObject.AddComponent<UnityEngine.UI.Mask>();
            _backgroundGameObj = this.transform.Find("Canvas/Content/Image/ObjectMaskSlave/UserImageMask (1)").gameObject;
            Component.DestroyImmediate(_backgroundGameObj.GetComponent<UnityEngine.UI.Image>());

            //outline around profile picture
            BackgroundMask = _backgroundGameObj.AddComponent<UnityEngine.UI.Image>();
            BackgroundMask.color = UserColor;
            BackgroundMask.ChangeSpriteFromString(Main.s_config.Js.Icon);
            BackgroundMask.transform.SetSiblingIndex(0);
            BackgroundMask.transform.localScale = new Vector3(1.45f, 1.25f, 1);
            _maskGameObject.transform.Find("UserImage").transform.localScale = new Vector3(1.05f, 1.05f, 1);

            _friendIcon = _backgroundGameObject.transform.Find("FriendsIndicator").gameObject;
            _friendIcon.transform.localScale = new Vector3(0.9f, 0.6f, 1);
            _friendIcon.transform.localPosition = new Vector3(0.60f, 0.39f, 0);
            _friend = _friendIcon.GetComponent<UnityEngine.UI.Image>();
            _friend.ChangeSpriteFromString(Main.s_config.Js.Friend).color = UserColor;
            _friend.enabled = true; //set to true for mic icons instantiation

            MicOff = GameObject.Instantiate(_friendIcon, _friendIcon.transform.parent.transform);
            _micOffImage = MicOff.GetComponent<UnityEngine.UI.Image>();
            _micOffImage.ChangeSpriteFromString(Main.s_config.Js.MicIconOff).color = UserColor;
            MicOff.transform.localPosition = new Vector3(0.944f, 0.39f, 0);

            MicOn = GameObject.Instantiate(MicOff, _friendIcon.transform.parent.transform);
            _micOnImage = MicOn.GetComponent<UnityEngine.UI.Image>();
            //change color tint/brightness of mic icon by +30% if player is speaking
            _micOnColor = new Color(UserColor.r * 1.3f, UserColor.g * 1.3f, UserColor.b * 1.3f);
            _micOnImage.material.color = _micOnColor; //review this line
            _micOnImage.ChangeSpriteFromString(Main.s_config.Js.MicIconOn).color = _micOnColor;
            MicOn.transform.localPosition = new Vector3(0.944f, 0.39f, 0);

            //Nametags start with mic off icon until updated by voice event
            MicOn.SetActive(false);
            MicOff.SetActive(true);

            //at this point, friend icon set to off unless account is friend
            if (Friends.FriendsWith(transform.parent.gameObject.GetComponent<PlayerDescriptor>().ownerId))
            {
                _friend.enabled = true;
            } 
            else
            {
                _friend.enabled = false;
            }

            Dispose();
         }
        public void Dispose()
        {
            _friend = null;
            _micOnImage = null;
            _micOffImage = null;
            _maskGameObject = null;
            _maskImageComp = null;
            _backgroundGameObject = null;
            _backgroundGameObj = null;
            _friendIcon = null;
            CancelInvoke(nameof(Setup));
        }
    }
}
