using System;
using RobloxFiles;
using RobloxFiles.DataTypes;

namespace a_slice_of_homer_simpson
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("made by simul");
            if (args.Length < 2)
            {
                Console.WriteLine("Please open this folder in Command Prompt and run the following: \"convert.exe\" RBXMNAME REVIVALDOMAIN");
                Console.WriteLine("Here's an example: \"convert.exe\" thishat.rbxm kapish.fun");

                Console.ReadLine();
            } else
            {
                RobloxFile file = RobloxFile.Open(args[0]);
                string domain = args[1];

                if (file is BinaryRobloxFile)
                {
                    Console.WriteLine("Binary file; converting to XML...");
                    XmlRobloxFile xml = new XmlRobloxFile();
                    foreach(Instance instance in file.GetChildren())
                    {
                        instance.Parent = xml;
                    }
                    file = xml;
                    Console.WriteLine("Converted to XML!");
                }

                Console.WriteLine("Enumerating descendants...");

                foreach(Instance instance in file.GetDescendants())
                {
                    var instPath = instance.GetFullName();

                    foreach (var prop in instance.Properties)
                    {
                        Property binding = prop.Value;
                        Content content = binding.CastValue<Content>();

                        if(content != null)
                        {
                            string propName = prop.Key;
                            string url = content.Url.Trim();

                            if(url.Contains("roblox.com"))
                            {
                                Console.WriteLine($"Updating URL at {instPath}.{propName}...");
                                url = url.Replace("roblox.com", domain);
                                instance.props[propName].Value = url;
                                Console.WriteLine($"Updated URL at {instPath}.{propName}!");
                            }
                        }
                    }
                }

                file.Save("output.rbxm");

                Console.WriteLine("Enumerated descendants!");
                Console.WriteLine("Saved to output.rbxm (XML FILE)");
            }
        }
    }
}