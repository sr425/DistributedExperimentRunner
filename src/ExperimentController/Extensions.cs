using System;

namespace ExperimentController
{
    public static class Extensions
    {
        public static string ToHexString(this byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "");
        }
    }
}