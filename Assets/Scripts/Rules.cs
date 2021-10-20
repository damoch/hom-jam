using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class Rules
    {
        public Rules()
        {
            LoadValues();
        }
        private const string _filename = "rules.csv";
        private string[] lines;

        public int PlayerStartHealth { get; set; }

        public void LoadValues()
        {
            var path = Application.dataPath + Path.DirectorySeparatorChar + _filename;
            if (!File.Exists(path)) 
            {
                File.Create(path);
                return;
            }
            lines = File.ReadAllLines(Application.dataPath + Path.DirectorySeparatorChar + _filename);
        }
    }
}
