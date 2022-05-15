#nullable enable

using System;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace JdSharp.Core.Utils
{
    public static class ReaderUtils
    {
        public static void ReadLineFromStream(Stream dataStream, Expression<Action<string>> readLinePredicate)
        {
            if (dataStream.CanRead)
            {
                using StreamReader streamReader = new StreamReader(dataStream, Encoding.UTF8);
                string? line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    var action = readLinePredicate.Compile();
                    action.Invoke(line);
                }
            }
        }
    }
}