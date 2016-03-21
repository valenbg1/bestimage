using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace BestImage
{
    public static class PreferencesSaver
    {
        public static void savePreferences(string path, int heightRef, int widthRef)
        {
            using (XmlWriter prefWriter = XmlWriter.Create("preferences.xml"))
            {
                prefWriter.WriteStartDocument();
                prefWriter.WriteStartElement("preferences");

                prefWriter.WriteElementString("path", path);
                prefWriter.WriteElementString("heightRef", heightRef.ToString());
                prefWriter.WriteElementString("widthRef", widthRef.ToString());

                prefWriter.WriteEndElement();
                prefWriter.WriteEndDocument();
            }
        }

        public static void loadPreferences(out string path, out int heightRef, out int widthRef)
        {
            path = "";
            heightRef = 0;
            widthRef = 0;

            try
            {
                using (XmlReader prefReader = XmlReader.Create("preferences.xml"))
                {
                    while (prefReader.Read())
                    {
                        if (prefReader.IsStartElement())
                        {
                            switch (prefReader.Name)
                            {
                                case "path":
                                    prefReader.Read();
                                    path = prefReader.Value;
                                    break;

                                case "heightRef":
                                    prefReader.Read();
                                    heightRef = UInt16.Parse(prefReader.Value);
                                    break;

                                case "widthRef":
                                    prefReader.Read();
                                    widthRef = UInt16.Parse(prefReader.Value);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {

            }
        }
    }
}
