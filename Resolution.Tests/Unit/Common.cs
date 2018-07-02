using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Resolution.Tests.Unit
{
    public static class Common
    {
        public static byte[] ReadFixture(params string[] paths)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream($"Resolution.Tests.Unit.Fixtures.{paths[0]}.{paths[1]}");

            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static T[] GetArray<T>(params T[] parameters)
        {
            return parameters;
        }

        public static IList<T> GetList<T>(params T[] parameters)
        {
            return new List<T>(parameters);
        }
    }
}
