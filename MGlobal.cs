namespace SapconCore.Mumps
{
	public class MGlobal : MumpsObject
	{
		/// <summary>
		/// Адрес глобаля в Mumps.
		/// </summary>
		public string MumpsAdress { get; }

		/// <summary>
		/// Перечисление, для лучшего поиска глобалей.
		/// </summary>
		public MGlobalNames EnumName { get; }

		public string MumpsFullName
		{
			get { return $"{MumpsAdress}"; }
		}

		private MGlobal(UCI uci, VOL vol, string mumpsAdress, string description = "")
		{
			UCI = uci;
			VOL = vol;
			MumpsAdress = mumpsAdress;
			Description = description;
		}

		/// <summary>
		/// Создает экземпляр программы.
		/// </summary>
		/// <param name="uci">Кип в котором находится программа</param>
		/// <param name="vol">Том на котором находится программа</param>
		/// <param name="mumpsAdress">Имя программы в Mumps</param>
		/// <param name="name">Уникальное имя программы</param>
		/// <param name="description">Описание назначения программы</param>
		public MGlobal(UCI uci, VOL vol, string mumpsAdress, string name, string description = "")
			:this(uci, vol, mumpsAdress, description)
		{
			Name = name;
			EnumName = MGlobalNames.Undefined;
		}

		/// <summary>
		/// Создает экземпляр программы.
		/// </summary>
		/// <param name="uci">Кип в котором находится программа</param>
		/// <param name="vol">Том на котором находится программа</param>
		/// <param name="mumpsAdress">Имя программы в Mumps</param>
		/// <param name="name">Уникальное имя программы в виде перечисления для лучшего поиска</param>
		/// <param name="description">Описание назначения программы</param>
		private MGlobal(UCI uci, VOL vol, string mumpsAdress, MGlobalNames name, string description = "")
			: this(uci, vol, mumpsAdress, description)
		{
			Name = name.ToString();
			EnumName = name;
		}

		public override bool Equals(object obj)
		{
			var glob = obj as MGlobal;

			if (glob != null)
				return this.Name == glob.Name;
			return false;
		}

		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				var hash = (int)2166136261;
				hash = (hash * 16777619) ^ Name.GetHashCode();
				return hash;
			}
		}

		public override string ToString()
		{
			return $"[{UCI},{VOL}]{MumpsAdress}";
		}
	}
}
