namespace SapconCore.Mumps
{
	/// <summary>
	/// Статус подключения к Mumps.
	/// </summary>
	public enum ConnectionStatus
	{
		/// <summary>
		/// Соединение с Mumps не было проинициализированно.
		/// </summary>
		UnInitialized,

		/// <summary>
		/// Connection to MSM currently.
		/// </summary>
		Connected,

		/// <summary>
		/// Was disconected from MSM.
		/// </summary>
		Disconnected,

		/// <summary>
		/// Connection not established.
		/// </summary>
		Failed,
	}
}
