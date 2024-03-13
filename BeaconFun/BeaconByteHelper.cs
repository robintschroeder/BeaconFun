using System;
using System.Diagnostics;

namespace BeaconFun
{
	public static class BeaconByteHelper
	{
		public static iBeacon ParseiBeaconBytes(byte[] bytes)
		{
			//entire message length is 25
			if (bytes[3] == 0x15 && bytes.Length == 25)
			{
                byte[] binaryUUID = bytes.Skip(4).Take(16).ToArray();
                string strHex = BitConverter.ToString(binaryUUID);
				Guid uuid = new Guid(strHex.Replace("-", ""));

                var major_bytes = bytes.Skip(20).Take(2).Reverse().ToArray(); //they send this in big endian
                var minor_bytes = bytes.Skip(22).Take(2).Reverse().ToArray();

                var iBeacon = new iBeacon()
				{
					DataLength = bytes[3],
					UUID = uuid,
					Major = BitConverter.ToInt16(major_bytes),
					Minor = BitConverter.ToInt16(minor_bytes),
					RSSI = bytes[24],
					LastSeenDateTime = DateTime.UtcNow,
				};

				Debug.WriteLine($"iBeacon: {iBeacon.UUID.ToString()} {iBeacon.Major} {iBeacon.Minor} {iBeacon.RSSI}");
                Debug.WriteLine($"{BeaconByteHelper.ByteArrayToHexString(bytes.Skip(20).Take(2).ToArray())}");
                Debug.WriteLine($"{BeaconByteHelper.ByteArrayToHexString(bytes.Skip(22).Take(2).ToArray())}");


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

