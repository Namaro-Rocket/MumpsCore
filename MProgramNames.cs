namespace SapconCore.Mumps
{
	/// <summary>
	/// Перечисление с часто используемыми именами программ Mumps.
	/// </summary>
	public enum MProgramNames
	{
		/// <summary>
		/// Не определена.
		/// </summary>
		Undefined,

		/// <summary>
		/// Выполнить программу в том же разделе.
		/// </summary>
		MumpsDo,

		/// <summary>
		/// Выполнить программу на удаленном разделе.
		/// </summary>
		MumpsDoRemote,


		/// <summary>
		/// Выполнить строку.
		/// </summary>
		MumpsExecute,

		/// <summary>
		/// Вернуть значение переменной.
		/// </summary>
		MumpsReturn,

		/// <summary>
		/// Получить глобаль.
		/// </summary>
		MumpsGetGlobal,

		/// <summary>
		/// Получить все подписи для чертежа.
		/// </summary>
		getDrawingSigns,

		/// <summary>
		/// Получить информацию о пользователе.
		/// </summary>
		getUserData,

		/// <summary>
		/// Получить доступы пользователя.
		/// </summary>
		getUserSigns,
			
		/// <summary>
		/// Получить данные штампа бтд для чертежа.
		/// </summary>
		getBtdDrawingStamp,

		/// <summary>
		/// Получить все чертежи с пункта плана.
		/// </summary>
		getDrwsFromPpn,

		/// <summary>
		/// 
		/// </summary>
		getCurrentQueuedDrws,

		/// <summary>
		/// 
		/// </summary>
		SignDrawing,

		/// <summary>
		/// 
		/// </summary>
		DelDrwFromQueue,
	}
}
