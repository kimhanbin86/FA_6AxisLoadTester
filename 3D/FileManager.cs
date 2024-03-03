using System;
using System.IO;
using System.Text;

namespace _3D
{
	#region sample file
	/*
	d7273b92c28076c7f473949dbc85632f
	3,30.00,20.00,11.00,0.00,0.00,0.00,0.00,0.00,0.00,3.00,-2.00,1.00,128,128,128,Gray,-4.00,-2.00,-1.00,128,128,128,Gray,None,False,False,False,False,True
	*/
	#endregion

	public static class FileManager
	{
		public static bool WriteFile(String settings, String fileName, out String errorMsg)
		{
			errorMsg = String.Empty;
			using (StreamWriter streamWriter = new StreamWriter(fileName))
			{
				try
				{
					byte[] array = Encoding.ASCII.GetBytes(settings);
					byte[] hash = HashUtil.ComputeHash(array);
					String hashString = HashUtil.GetHashString(hash);
					streamWriter.WriteLine(hashString);
					streamWriter.Write(settings);
				}
				catch (Exception ex)
				{
					// todo log
					errorMsg = ex.Message;
					return false;
				}
			}

			return true;
		}

		public static bool ReadFile(out String settings, String fileName, out String errorMsg)
		{
			settings = String.Empty;
			errorMsg = String.Empty;
			using (StreamReader streamReader = new StreamReader(fileName))
			{
				String line;
				int lineCount = 0;
				string hashStringOriginal = String.Empty;

				while ((line = streamReader.ReadLine()) != null)
				{
					try
					{
						if (lineCount == 0)
						{
							hashStringOriginal = line;
						}
						else
						{
							settings = line;

							String[] settingsArray = line.Split(new char[] { ',' });
							if (settingsArray.Length != 30)
							{
								errorMsg = errorMsg = String.Format("Invalid settings file");
								return false;
							}
						}
					}
					catch(Exception ex)
					{
						// todo log
						errorMsg = ex.Message;
						return false;
					}
					
					lineCount++;
				}

				byte[] array = Encoding.ASCII.GetBytes(settings);
				byte[] hash = HashUtil.ComputeHash(array);
				String hashStringNew = HashUtil.GetHashString(hash);

				if(!hashStringOriginal.Equals(hashStringNew))
				{
					errorMsg = String.Format("Md5 hash mismatch (file is corrupted)");
					return false;
				}
			}

			return true;
		}
	}
}
