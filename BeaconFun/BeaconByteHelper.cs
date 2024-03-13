using System;
using System.Diagnostics;

namespace BeaconFun
{
	public static class BeaconByteHelper
	{
		public static iBeacon ParseiBeaconBytes(byte[] bytes, int rssi)
		{
			//entire message length is 25
			if (bytes[3] == 0x15 && bytes.Length == 25)
			{
                byte[] binaryUUID = bytes.Skip(4).Take(16).ToArray();
                string strHex = BitConverter.ToString(binaryUUID);
				Guid uuid = new Guid(strHex.Replace("-", ""));

                var major_bytes = bytes.Skip(20).Take(2).Reverse().ToArray(); //they send this in big endian
                var minor_bytes = bytes.Skip(22).Take(2).Reverse().ToArray();

                /* The final byte is the packet is used to calculate distance from the iBeacon. 
                 * It represents RSSI value (Received Signal Strength Indication) measured at 1 meter from the iBeacon. 
                 * The value of this byte changes automatically if the user changes the transmission power (Tx power) 
                 * for the iBeacon, but it's also configurable if a user wants custom calibration for each beacon and 
                 * Tx power level.*/

                var iBeacon = new iBeacon()
				{
					DataLength = bytes[3],
					UUID = uuid,
					Major = BitConverter.ToInt16(major_bytes),
					Minor = BitConverter.ToInt16(minor_bytes),
					RSSI_1_Meter = bytes[24],
                    RSSI = rssi,
					LastSeenDateTime = DateTime.UtcNow,
				};

				Debug.WriteLine($"iBeacon: {iBeacon.UUID.ToString()} {iBeacon.Major} {iBeacon.Minor} {iBeacon.RSSI} {iBeacon.DistanceInMeters}");
                //Debug.WriteLine($"{BeaconByteHelper.ByteArrayToHexString(bytes.Skip(20).Take(2).ToArray())}");
                //Debug.WriteLine($"{BeaconByteHelper.ByteArrayToHexString(bytes.Skip(22).Take(2).ToArray())}");


                return iBeacon;
			}
			else 
			{
				//remaining data length is not 21, as expected
				return null;
			}
		}

        public static string ByteArrayToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
		}

        public static bool IsIBeaconAdvertisement(byte[] data)
        {
            if (data == null)
                return false;

            if (data.Length != 25)
                return false;

            if (data[2] != 0x02)
                return false;

            return true;
        }
    }
}

