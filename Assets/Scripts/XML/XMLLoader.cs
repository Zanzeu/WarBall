using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace WarBall.XML
{
    public class XMLLoader
    {
        public static T LoadFromXML<T>(string xmlContent) where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var reader = new StringReader(xmlContent))
                {
                    return serializer.Deserialize(reader) as T;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"XML∑¥–Ú¡–ªØ ß∞‹: {e.Message}");
                return null;
            }
        }
    }
}
