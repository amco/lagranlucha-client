using UnityEngine;

using EasyMobile;

namespace CFLFramework.Utilities.Extensions
{
    public static class SharingExtension
    {
        #region FIELDS

        private const string Filename = "Share";
        private const string EmptyString = "";

        #endregion

        #region BEHAVIORS

        public static void ShareTexture(Texture2D texture, string shareMessage = EmptyString)
        {
#if UNITY_IOS
            Sharing.ShareTexture2D(texture, Filename, EmptyString);
#else
            Sharing.ShareTexture2D(texture, Filename, shareMessage);
#endif
        }

        public static void ShareSprite(Sprite sprite, string shareMessage = EmptyString)
        {
            Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Rect spriteRect = new Rect(sprite.textureRect.x, sprite.textureRect.y, sprite.textureRect.width, sprite.textureRect.height);
            Color[] pixels = sprite.texture.GetPixels((int)spriteRect.x, (int)spriteRect.y, (int)spriteRect.width, (int)spriteRect.height);
            texture.SetPixels(pixels);
            texture.Apply();
            ShareTexture(texture, shareMessage);
        }

        public static void ShareRenderTexture(RenderTexture renderTexture, string shareMessage = EmptyString)
        {
            RenderTexture.active = renderTexture;
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();
            ShareTexture(texture, shareMessage);
            RenderTexture.active = null;
        }

        public static void ShareMessage(string shareMessage)
        {
            Sharing.ShareText(shareMessage);
        }

        public static void ShareURL(string url)
        {
            Sharing.ShareURL(url);
        }

        #endregion
    }
}
