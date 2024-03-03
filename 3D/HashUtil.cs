using System;
using System.Security.Cryptography;

namespace _3D
{
	public static class HashUtil
	{
		// cryptographic service provider
		private static System.Security.Cryptography.MD5 Md5 = MD5.Create();

		public static byte[] ComputeHash(byte[] array)
		{
			return Md5.ComputeHash(array);
		}

		// Return a hash code as a string (thanks again to Rod Stephens)
		public static string GetHashString(byte[] hash)
		{
			string result = String.Empty;
			foreach (byte b in hash) result += b.ToString("x2");
			return result;
		}
	}
}
