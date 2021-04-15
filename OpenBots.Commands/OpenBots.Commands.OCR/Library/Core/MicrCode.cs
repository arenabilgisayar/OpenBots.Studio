using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    public class MicrCode
    {
        public MicrCode()
        {
            AccountNumber = "";
            CheckNumber = "";
            RoutingNumber = "";
        }

        public string AccountNumber { get; set; }
        public string CheckNumber { get; set; }
        public string RoutingNumber { get; set; }

    }
}
