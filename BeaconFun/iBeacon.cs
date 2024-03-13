using System;
namespace BeaconFun
{
	public class iBeacon
	{
		public iBeacon()
		{
		}

		public int DataLength { get; set; }
        public Guid UUID { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
		public int RSSI { get; set; }
        public DateTime LastSeenDateTime { get; set; }

        public Color RssiColor
        { 
            get
            {
                //-26 (a few inches) to -100 (40–50 m distance)
                switch (RSSI)
                {
                    case > -6:
                        return new Color(0, 66, 0); //004200; dark green
                    case > -16:
                        return new Color(0, 107, 0);//#006b00;
                    case > -26:
                        return new Color(0, 148, 0);//#009400;
                    case > -36:
                        return new Color(0, 189, 0);//#00bd00;
                    case > -46:
                        return new Color(0, 230, 0);//#00e600;
                    case > -56:
                        return new Color(15, 255, 15);//#0fff0f;
                    case > -66:
                        return new Color(56, 255, 56);//#38ff38;
                    case > -76:
                        return new Color(97, 255, 97);//#61ff61;
                    case > -86:
                        return new Color(138, 255, 138);//#8aff8a;
                    case > -96:
                        return new Color(179, 255, 179);//#b3ffb3; light green
                    default:
                        return new Color(255, 255, 255);
                }
            }
        }
    }
}

