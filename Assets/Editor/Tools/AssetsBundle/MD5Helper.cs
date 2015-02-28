using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;


public class MD5Helper
{
	public static string md5(string source)
	{
		MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
		byte[] bytes = Encoding.UTF8.GetBytes(source);
		byte[] buffer2 = provider.ComputeHash(bytes, 0, bytes.Length);
		provider.Clear();
		string str = string.Empty;
		for (int i = 0; i < buffer2.Length; i++)
		{
			str = str + Convert.ToString(buffer2[i], 0x10).PadLeft(2, '0');
		}
		return str.PadLeft(0x20, '0');
	}
	
	public static string md5file(string file)
	{
		if(!File.Exists(file))
			return "";
		string str;
		try
		{
			FileStream inputStream = new FileStream(file, FileMode.Open);
			byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(inputStream);
			inputStream.Close();
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < buffer.Length; i++)
			{
				builder.Append(buffer[i].ToString("x2"));
			}
			str = builder.ToString();
		}
		catch (Exception exception)
		{
			throw new Exception("md5file() fail, error:" + exception.Message);
		}
		return str;
	}
}