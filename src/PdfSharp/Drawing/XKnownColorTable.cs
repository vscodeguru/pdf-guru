namespace PdfSharp.Drawing
{
    internal class XKnownColorTable
    {
        internal static uint[] ColorTable;

        public static uint KnownColorToArgb(XKnownColor color)
        {
            if (ColorTable == null)
                InitColorTable();
            if (color <= XKnownColor.YellowGreen)
                return ColorTable[(int)color];
            return 0;
        }

        public static bool IsKnownColor(uint argb)
        {
            for (int idx = 0; idx < ColorTable.Length; idx++)
            {
                if (ColorTable[idx] == argb)
                    return true;
            }
            return false;
        }

        public static XKnownColor GetKnownColor(uint argb)
        {
            for (int idx = 0; idx < ColorTable.Length; idx++)
            {
                if (ColorTable[idx] == argb)
                    return (XKnownColor)idx;
            }
            return (XKnownColor)(-1);
        }

        private static void InitColorTable()
        {
            uint[] colors = new uint[141];
            colors[0] = 0xFFF0F8FF;   
            colors[1] = 0xFFFAEBD7;   
            colors[2] = 0xFF00FFFF;   
            colors[3] = 0xFF7FFFD4;   
            colors[4] = 0xFFF0FFFF;   
            colors[5] = 0xFFF5F5DC;   
            colors[6] = 0xFFFFE4C4;   
            colors[7] = 0xFF000000;   
            colors[8] = 0xFFFFEBCD;   
            colors[9] = 0xFF0000FF;   
            colors[10] = 0xFF8A2BE2;   
            colors[11] = 0xFFA52A2A;   
            colors[12] = 0xFFDEB887;   
            colors[13] = 0xFF5F9EA0;   
            colors[14] = 0xFF7FFF00;   
            colors[15] = 0xFFD2691E;   
            colors[16] = 0xFFFF7F50;   
            colors[17] = 0xFF6495ED;   
            colors[18] = 0xFFFFF8DC;   
            colors[19] = 0xFFDC143C;   
            colors[20] = 0xFF00FFFF;   
            colors[21] = 0xFF00008B;   
            colors[22] = 0xFF008B8B;   
            colors[23] = 0xFFB8860B;   
            colors[24] = 0xFFA9A9A9;   
            colors[25] = 0xFF006400;   
            colors[26] = 0xFFBDB76B;   
            colors[27] = 0xFF8B008B;   
            colors[28] = 0xFF556B2F;   
            colors[29] = 0xFFFF8C00;   
            colors[30] = 0xFF9932CC;   
            colors[31] = 0xFF8B0000;   
            colors[32] = 0xFFE9967A;   
            colors[33] = 0xFF8FBC8B;   
            colors[34] = 0xFF483D8B;   
            colors[35] = 0xFF2F4F4F;   
            colors[36] = 0xFF00CED1;   
            colors[37] = 0xFF9400D3;   
            colors[38] = 0xFFFF1493;   
            colors[39] = 0xFF00BFFF;   
            colors[40] = 0xFF696969;   
            colors[41] = 0xFF1E90FF;   
            colors[42] = 0xFFB22222;   
            colors[43] = 0xFFFFFAF0;   
            colors[44] = 0xFF228B22;   
            colors[45] = 0xFFFF00FF;   
            colors[46] = 0xFFDCDCDC;   
            colors[47] = 0xFFF8F8FF;   
            colors[48] = 0xFFFFD700;   
            colors[49] = 0xFFDAA520;   
            colors[50] = 0xFF808080;   
            colors[51] = 0xFF008000;   
            colors[52] = 0xFFADFF2F;   
            colors[53] = 0xFFF0FFF0;   
            colors[54] = 0xFFFF69B4;   
            colors[55] = 0xFFCD5C5C;   
            colors[56] = 0xFF4B0082;   
            colors[57] = 0xFFFFFFF0;   
            colors[58] = 0xFFF0E68C;   
            colors[59] = 0xFFE6E6FA;   
            colors[60] = 0xFFFFF0F5;   
            colors[61] = 0xFF7CFC00;   
            colors[62] = 0xFFFFFACD;   
            colors[63] = 0xFFADD8E6;   
            colors[64] = 0xFFF08080;   
            colors[65] = 0xFFE0FFFF;   
            colors[66] = 0xFFFAFAD2;   
            colors[67] = 0xFFD3D3D3;   
            colors[68] = 0xFF90EE90;   
            colors[69] = 0xFFFFB6C1;   
            colors[70] = 0xFFFFA07A;   
            colors[71] = 0xFF20B2AA;   
            colors[72] = 0xFF87CEFA;   
            colors[73] = 0xFF778899;   
            colors[74] = 0xFFB0C4DE;   
            colors[75] = 0xFFFFFFE0;   
            colors[76] = 0xFF00FF00;   
            colors[77] = 0xFF32CD32;   
            colors[78] = 0xFFFAF0E6;   
            colors[79] = 0xFFFF00FF;   
            colors[80] = 0xFF800000;   
            colors[81] = 0xFF66CDAA;   
            colors[82] = 0xFF0000CD;   
            colors[83] = 0xFFBA55D3;   
            colors[84] = 0xFF9370DB;   
            colors[85] = 0xFF3CB371;   
            colors[86] = 0xFF7B68EE;   
            colors[87] = 0xFF00FA9A;   
            colors[88] = 0xFF48D1CC;   
            colors[89] = 0xFFC71585;   
            colors[90] = 0xFF191970;   
            colors[91] = 0xFFF5FFFA;   
            colors[92] = 0xFFFFE4E1;   
            colors[93] = 0xFFFFE4B5;   
            colors[94] = 0xFFFFDEAD;   
            colors[95] = 0xFF000080;   
            colors[96] = 0xFFFDF5E6;   
            colors[97] = 0xFF808000;   
            colors[98] = 0xFF6B8E23;   
            colors[99] = 0xFFFFA500;   
            colors[100] = 0xFFFF4500;   
            colors[101] = 0xFFDA70D6;   
            colors[102] = 0xFFEEE8AA;   
            colors[103] = 0xFF98FB98;   
            colors[104] = 0xFFAFEEEE;   
            colors[105] = 0xFFDB7093;   
            colors[106] = 0xFFFFEFD5;   
            colors[107] = 0xFFFFDAB9;   
            colors[108] = 0xFFCD853F;   
            colors[109] = 0xFFFFC0CB;   
            colors[110] = 0xFFDDA0DD;   
            colors[111] = 0xFFB0E0E6;   
            colors[112] = 0xFF800080;   
            colors[113] = 0xFFFF0000;   
            colors[114] = 0xFFBC8F8F;   
            colors[115] = 0xFF4169E1;   
            colors[116] = 0xFF8B4513;   
            colors[117] = 0xFFFA8072;   
            colors[118] = 0xFFF4A460;   
            colors[119] = 0xFF2E8B57;   
            colors[120] = 0xFFFFF5EE;   
            colors[121] = 0xFFA0522D;   
            colors[122] = 0xFFC0C0C0;   
            colors[123] = 0xFF87CEEB;   
            colors[124] = 0xFF6A5ACD;   
            colors[125] = 0xFF708090;   
            colors[126] = 0xFFFFFAFA;   
            colors[127] = 0xFF00FF7F;   
            colors[128] = 0xFF4682B4;   
            colors[129] = 0xFFD2B48C;   
            colors[130] = 0xFF008080;   
            colors[131] = 0xFFD8BFD8;   
            colors[132] = 0xFFFF6347;   
            colors[133] = 0x00FFFFFF;   
            colors[134] = 0xFF40E0D0;   
            colors[135] = 0xFFEE82EE;   
            colors[136] = 0xFFF5DEB3;   
            colors[137] = 0xFFFFFFFF;   
            colors[138] = 0xFFF5F5F5;   
            colors[139] = 0xFFFFFF00;   
            colors[140] = 0xFF9ACD32;   

            ColorTable = colors;
        }
    }
}
