namespace SapconCore.Mumps
{
	using Newtonsoft.Json;

	/// <summary>
	/// Отображение объекта из системы Mumps.
	/// </summary>
	public abstract class MumpsObject
	{
		/// <summary>
		/// Физический раздел на котором находится объект (кип).
		/// </summary>
		[JsonProperty(PropertyName = "uci")]
		public UCI UCI { get; protected set; }

		/// <summary>
		/// Логический раздел на котором находится объект (том).
		/// </summary>
		[JsonProperty(PropertyName = "vol")]
		public VOL VOL { get; protected set; }

		/// <summary>
		/// Описание действий или назначения объекта.
		/// </summary>
		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		/// <summary>
		/// Уникальное имя объекта.
		/// </summary>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; protected set; }
	}
}
