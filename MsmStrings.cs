namespace SapconCore.Mumps
{
	using System.Text;

	/// <summary>
	/// Класс помошник кодирования и декодирования данных в\из Mumps
	/// </summary>
	public static class MsmEncoding
	{
		public static string[] ConvertToWin(string[] strArr)
		{
			for (int i = 0; i < strArr.Length; i++)
				strArr[i] = ConvertFromMumps(strArr[i]);

			return strArr;
		}

		public static string ConvertToWin(string str)
		{
			return ConvertFromMumps(str);
		}

		public static string ConvertToMumps(string str)
		{
			return ConvertToMumpsEnc(str);
		}

		private static string ConvertFromMumps(string source)
		{
			var cp866 = Encoding.GetEncoding(866);
			var win1251 = Encoding.GetEncoding("Windows-1251");

			var win1251Bytes = win1251.GetBytes(source);
			var cp866String = cp866.GetString(win1251Bytes);

			return cp866String;
		}

		private static string ConvertToMumpsEnc(string source)
		{
			var cp866 = Encoding.GetEncoding(866);
			var win1251 = Encoding.GetEncoding("Windows-1251");

			var cp866Bytes = cp866.GetBytes(source);
			var win1251String = win1251.GetString(cp866Bytes);

			return win1251String;
		}
	}
}
