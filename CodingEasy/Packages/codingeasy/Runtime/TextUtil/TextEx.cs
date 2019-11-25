using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using YimiTools.ColorUtil;

namespace YimiTools.TextUtil {
    public static class TextEx
    {

        public static void RichText(this Text _Text, string _Content, Color _Color)
        {
            _Text.text = (string.Format("<color=\"#{0}\">{1}</color>", _Color.ToHex(), _Content));
        }

        public static void RichText(this Text _Text, string _Content, float _R, float _G, float _B, float _A = 1f)
        {
            _Text.text = (string.Format("<color=\"#{0}\">{1}</color>", new Color(_R, _G, _B, _A).ToHex(), _Content));
        }
    }

}
