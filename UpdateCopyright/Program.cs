using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UpdateLicenseSourceCS
{
    class Program
    {
        static string[] excludedirs = { "\\obj\\", "\\bin\\", "\\lib\\", "\\Interop\\Swig\\", "\\.git", "\\.vs" };

        static void Main(string[] args)
        {
            var oldcright = new List<string>();

            foreach (var file in Directory.GetFiles(Environment.CurrentDirectory))
            {
                if (file.Contains("oldcr") && file.EndsWith(".txt"))
                    oldcright.Add(File.ReadAllText(file));
            }
            //oldcright = null;
            var cright = File.ReadAllText("copyrightText.txt");
            RecursiveTraversal("D:\\Workspace\\Pipeline\\pipeline-dotnet", oldcright.ToArray(), cright);
        }

        private static void RecursiveTraversal(string dir, string[] oldcopyright, string copyright)
        {
            var fns = Directory.EnumerateFileSystemEntries(dir);
            Console.WriteLine("listing " + dir);
            foreach(var fullfn in fns) {
                if (excludedirs.Where(e => fullfn.Contains(e)).Count() > 0)
                    continue;
                if (Directory.Exists(fullfn))
                    RecursiveTraversal(fullfn, oldcopyright, copyright);
                else if (fullfn.EndsWith(".cs"))
                    UpdateSource(fullfn, oldcopyright, copyright);
            }
        }

        private static void UpdateSource(string filename, string[] oldcopyrights, string copyright)
        {
            Console.WriteLine("reading " + filename);
            var fdata = File.ReadAllText(filename);

            if (oldcopyrights != null) {
                foreach (var oldcopyright in oldcopyrights) {
                    if (fdata.StartsWith(oldcopyright)) {
                        fdata = fdata.Substring(oldcopyright.Length).TrimStart();
                        break;
                    }
                }
            }

            if (fdata.StartsWith(copyright) == false) {
                Console.WriteLine("updating " + filename);
                fdata = copyright + fdata;
                File.WriteAllText(filename, fdata);
            }
        }
    }
}

    



