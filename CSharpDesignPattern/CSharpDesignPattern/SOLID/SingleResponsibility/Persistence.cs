using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSharpDesignPattern.SOLID.SingleResponsibility
{
    /// <summary>
    /// This class is responsible for saving the journal to a file
    /// </summary>
    public class Persistence
    {
        public void SaveToFile(Journal j, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
            {
                File.WriteAllText(filename, j.ToString());
            }
        }
    }
}
