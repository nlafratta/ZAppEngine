﻿using System.Text;

namespace Zirpl.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendIfNotEmpty(this StringBuilder sb, object val)
        {
            if (sb.Length > 0
                && val != null)
            {
                sb.Append(val);
            }
            return sb;
        }
    }
}
