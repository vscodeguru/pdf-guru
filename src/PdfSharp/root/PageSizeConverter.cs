using System;

using PdfSharp.Drawing;

namespace PdfSharp
{
    public static class PageSizeConverter
    {
        public static XSize ToSize(PageSize value)
        {
            switch (value)
            {
                case PageSize.A0:
                    return new XSize(2384, 3370);

                case PageSize.A1:
                    return new XSize(1684, 2384);

                case PageSize.A2:
                    return new XSize(1191, 1684);

                case PageSize.A3:
                    return new XSize(842, 1191);

                case PageSize.A4:
                    return new XSize(595, 842);

                case PageSize.A5:
                    return new XSize(420, 595);


                case PageSize.RA0:
                    return new XSize(2438, 3458);

                case PageSize.RA1:
                    return new XSize(1729, 2438);

                case PageSize.RA2:
                    return new XSize(1219, 1729);

                case PageSize.RA3:
                    return new XSize(865, 1219);

                case PageSize.RA4:
                    return new XSize(609, 865);

                case PageSize.RA5:
                    return new XSize(433, 609);


                case PageSize.B0:
                    return new XSize(2835, 4008);

                case PageSize.B1:
                    return new XSize(2004, 2835);

                case PageSize.B2:
                    return new XSize(1417, 2004);

                case PageSize.B3:
                    return new XSize(1001, 1417);

                case PageSize.B4:
                    return new XSize(709, 1001);

                case PageSize.B5:
                    return new XSize(499, 709);

                case PageSize.Quarto:               
                    return new XSize(576, 720);

                case PageSize.Foolscap:             
                    return new XSize(576, 936);

                case PageSize.Executive:            
                    return new XSize(540, 720);

                case PageSize.GovernmentLetter:     
                    return new XSize(576, 756);

                case PageSize.Letter:               
                    return new XSize(612, 792);

                case PageSize.Legal:                
                    return new XSize(612, 1008);

                case PageSize.Ledger:               
                    return new XSize(1224, 792);

                case PageSize.Tabloid:              
                    return new XSize(792, 1224);

                case PageSize.Post:                 
                    return new XSize(1126, 1386);

                case PageSize.Crown:                
                    return new XSize(1440, 1080);

                case PageSize.LargePost:            
                    return new XSize(1188, 1512);

                case PageSize.Demy:                 
                    return new XSize(1260, 1584);

                case PageSize.Medium:               
                    return new XSize(1296, 1656);

                case PageSize.Royal:                
                    return new XSize(1440, 1800);

                case PageSize.Elephant:             
                    return new XSize(1565, 2016);

                case PageSize.DoubleDemy:           
                    return new XSize(1692, 2520);

                case PageSize.QuadDemy:             
                    return new XSize(2520, 3240);

                case PageSize.STMT:                 
                    return new XSize(396, 612);

                case PageSize.Folio:                
                    return new XSize(612, 936);

                case PageSize.Statement:            
                    return new XSize(396, 612);

                case PageSize.Size10x14:            
                    return new XSize(720, 1008);
            }
            throw new ArgumentException("Invalid PageSize.", "value");
        }
    }
}