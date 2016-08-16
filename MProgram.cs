namespace SapconCore.Mumps
{
	using Newtonsoft.Json;

	/// <summary>
	/// Интерфейс программы в системе Mumps
	/// </summary>
	public class MProgram : MumpsObject
	{
		/// <summary>
		/// Имя программы в Mumps.
		/// </summary>
		[JsonProperty(PropertyName = "mumpsName")]
		public string MumpsName { get; }

		/// <summary>
		/// Имя метки в программе.
		/// </summary>
		[JsonProperty(PropertyName = "mumpsLabel")]
		public string MumpsLabel { get; }

		/// <summary>
		/// Полный путь программы.
		/// </summary>
		[JsonIgnore]
		public string MumpsFullName
		{
			get { return $"{MumpsLabel}^{MumpsName}";	}
		}

		private MProgram(UCI uci, VOL vol, string mumpsName, string mumpsLabel, string description = "")
		{
			UCI         = uci;
			VOL         = vol;
			MumpsLabel  = mumpsLabel;
			MumpsName   = mumpsName;
			Description = description;
		}

		/// <summary>
		/// Создает экземпляр программы.
		/// </summary>
		/// <param name="uci">Кип в котором находится программа</param>
		/// <param name="vol">Том на котором находится программа</param>
		/// <param name="mumpsName">Имя программы в Mumps</param>
		/// <param name="name">Уникальное имя программы</param>
		/// <param name="description">Описание назначения программы</param>
		[JsonConstructor]
		public MProgram(UCI uci, VOL vol, string mumpsName, string mumpsLabel, string name, string description = "")
			:this(uci, vol, mumpsName, mumpsLabel, description)
		{
			Name     = name;
		}

		/// <summary>
		/// Создает экземпляр программы.
		/// </summary>
		/// <param name="uci">Кип в котором находится программа</param>
		/// <param name="vol">Том на котором находится программа</param>
		/// <param name="mumpsName">Имя программы в Mumps</param>
		/// <param name="name">Уникальное имя программы в виде перечисления для лучшего поиска</param>
		/// <param name="description">Описание назначения программы</param>
		public MProgram(UCI uci, VOL vol, string mumpsName, string mumpsLabel, MProgramNames name, string description = "")
			: this(uci, vol, mumpsName, mumpsLabel, description)
		{
			Name     = name.ToString();
		}

		public override bool Equals(object obj)
		{
			var prog = obj as MProgram;

			if (prog != null)
				return this.Name == prog.Name;
			return false;
		}

		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = (int)2166136261;
				hash = (hash * 16777619) ^ Name.GetHashCode();
				return hash;
			}
		}

		public override string ToString()
		{
			return $"[{UCI},{VOL}]{MumpsLabel}^{MumpsName}";
		}
	}
}
