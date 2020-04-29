using System.Collections.Generic;

namespace CFLFramework.Levels
{
    public class Level
    {
        #region PROPERTIES

        public int Id { get; set; } = 0;
        public string FileName { get; set; } = "Level";
        public string Name { get; set; } = "Default name";
        public string Tag { get; set; } = "Default";
        public string Description { get; set; } = "This is a default description";
        public string Difficulty { get; set; } = "This is a default difficulty";
        public string Owner { get; set; } = "Create for Life";
        public Dictionary<string, object> Objectives { get; set; }
        public byte[] Icon { get; set; }
        public byte[] Sprite { get; set; }
        public byte[] Video { get; set; }
        public byte[] Data { get; set; }

        #endregion

        #region BEHAVIORS

        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }

        #endregion
    }
}
