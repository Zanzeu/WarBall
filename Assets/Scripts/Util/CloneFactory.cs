using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WarBall.Util
{
    public static class CloneFactory
    {
        public static T CreateDeepCopy<T>(T original)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException($"{typeof(T).Name}必须标记为[Serializable]", nameof(original));
            }

            if (ReferenceEquals(original, null))
            {
                return default;
            }

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, original);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}