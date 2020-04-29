#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("gDKxkoC9trmaNvg2R72xsbG1sLNfeU9p/xgjK68IyiQNjlczI+9giLA64+CrsY6H0TbZQQNNzv/Vyw+WkxIgSwfAkfFDa0L38teZ4SB82KIysb+wgDKxurIysbGwAb1+2oY/lozIM4UJIFicl6LUrDmi+BQg0TWuNefC1S/E9mzAgEIgp4d+XSufoXX0OWOBvs4DfF4l8j868zP7ezEWMB8Jy0LkdJ0snKVYJ5tQsTWlh2iUe6rLOvWErZpRPJwYhFWlZ/lGuhbJqhnsW+7I/4bhg3ZYNB0l2wnexsFt/rpDgGN0D8vkQlEENlgIc3kYB5kfDf4a4Qwl+d/Uqufhi+gcBZWRnwtFgMJjf82rDJIiBUOATSzKJHg0ova6+0vTrbKzsbCx");
        private static int[] order = new int[] { 0,2,3,9,9,10,11,9,9,10,12,12,12,13,14 };
        private static int key = 176;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
