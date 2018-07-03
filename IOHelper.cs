using System;
using System.IO;

namespace Zdd.Utility
{
    /// <summary>
    /// IOHelper
    /// </summary>
    public static class IOHelper
    {

        /// <summary>
        /// Gets the files in directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        public static FileInfo[] GetFilesInDirectory(string directory)
        {
            if ((directory == null) || (directory.Length < 1))
                throw new ArgumentException("Directory supplied is either null or empty");

            DirectoryInfo dirInfo = new DirectoryInfo(directory);

            if (!dirInfo.Exists)
                throw new ArgumentException("Directory '" + directory + "' does not exist.");

            return dirInfo.GetFiles();
        }
    }
}
