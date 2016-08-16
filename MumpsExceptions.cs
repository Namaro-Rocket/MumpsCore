namespace SapconCore.Mumps
{
	using System;

	/// <summary>
	/// общая ошибка Mumps
	/// </summary>
	public class MumpsGeneralException : Exception
	{
		public MumpsGeneralException(string message)
				: base(message)
		{
		}

		public MumpsGeneralException(string message, Exception innerException)
				: base(message, innerException)
		{
		}
	}

	/// <summary>
	/// Ошибка исполнения Mumps
	/// </summary>
	public class MumpsExecutionException : Exception
	{
		public MumpsExecutionException(string message)
			 : base(message)
		{
		}

		public MumpsExecutionException(string message, Exception innerException)
			 : base(message, innerException)
		{
		}
	}

	/// <summary>
	/// Ошибка подключения к Mumps
	/// </summary>
	public class MsmConnectionException : Exception
	{
		public MsmConnectionException(string message)
			 : base(message)
		{
		}

		public MsmConnectionException(string message, Exception innerException)
			 : base(message, innerException)
		{
		}
	}
}
