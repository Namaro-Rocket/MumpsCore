namespace SapconCore.Mumps
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Newtonsoft.Json;

	/// <summary>
	/// Пресеты используемых объектов Mumps (программы + глобали).
	/// </summary>
	public class MumpsObjectSet
	{
		/// <summary>
		/// Список программ.
		/// </summary>
		public List<MProgram> ProgramList { get; } = new List<MProgram>();
		
		/// <summary>
		/// Список глобалей.
		/// </summary>
		public List<MGlobal> GlobalList { get; } = new List<MGlobal>();

		/// <summary>
		/// Проверка на существование объекта.
		/// </summary>
		public bool Exist(MumpsObject obj)
		{
			if (obj is MProgram)
				return ProgramList.Contains(obj);
			else if (obj is MGlobal)
				return ProgramList.Contains(obj);
			else throw new NotImplementedException("Не обслуживаемый тип объекта.");
		}

		/// <summary>
		/// Удалить из пресета объект.
		/// </summary>
		public void Remove(MumpsObject obj)
		{
			if (obj is MProgram)
			{
				var prog = obj as MProgram;
				ProgramList.Remove(prog);
			}
			else if (obj is MGlobal)
			{
				var glob = obj as MGlobal;
				GlobalList.Remove(glob);
			} 
			else throw new NotImplementedException("Не обслуживаемый тип объекта.");
		}

		/// <summary>
		/// Удалить все элементы из пресета.
		/// </summary>
		public void Clear()
		{
			ProgramList.Clear();
			GlobalList.Clear();
		}

		/// <summary>
		/// Добавить в пресет mumps объект.
		/// Если объект с таким же именем существует, новый его заменяет.
		/// </summary>
		public void Add(MumpsObject obj)
		{
			if (Exist(obj))
				Remove(obj);

			if (obj is MProgram)
			{
				var prog = obj as MProgram;
				ProgramList.Add(prog);
			}
			else if (obj is MGlobal)
			{
				var glob = obj as MGlobal;
				GlobalList.Add(glob);
			} 
			else throw new NotImplementedException("Не обслуживаемый тип объекта.");
		}

		/// <summary>
		/// Объединить элементы с пресетом.
		/// </summary>
		public void Append(IEnumerable<MumpsObject> objs)
		{
			foreach (var obj in objs)
				Add(obj);
		}

		/// <summary>
		/// Объединить элементы с пресетом.
		/// </summary>
		public void Append(MumpsObjectSet objSet)
		{
			foreach (var prog in objSet.ProgramList)
				Add(prog);

			foreach (var glob in objSet.GlobalList)
				Add(glob);
		}

		/// <summary>
		/// Найти программу в пресете по имени.
		/// </summary>
		/// <param name="name">Строковое название программы.</param>
		public MProgram FindProgram(string name)
		{
			return ProgramList.First(prog => prog.Name == name);
		}

		/// <summary>
		/// Найти программу в пресете по заданным спискам имен программ.
		/// </summary>
		public MProgram FindProgram(MProgramNames name)
		{
			return FindProgram(name.ToString());
		}

		/// <summary>
		/// Найти глобаль в пресете по имени.
		/// </summary>
		/// <param name="name">Строковое название глобаля.</param>
		public MGlobal FindGlobal(string name)
		{
			return GlobalList.First(glob => glob.Name == name);
		}

		/// <summary>
		/// Найти глобаль в пресете по заданным спискам имен глобалей.
		/// </summary>
		public MGlobal FindGlobal(MGlobalNames name)
		{
			return GlobalList.First(glob => glob.EnumName == name);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public static class MumpsObjectSetManager
	{
		/// <summary>
		/// Загружает пресет из файла
		/// </summary>
		public static MumpsObjectSet LoadFromFile(string pathToFile)
		{
			using (var fileStream     = new FileStream(pathToFile, FileMode.Open))
			using (var streamReader   = new StreamReader(fileStream))
			using (var jsonTextReader = new JsonTextReader(streamReader))
			{
				var serializer = new JsonSerializer();
				return serializer.Deserialize<MumpsObjectSet>(jsonTextReader);
			}
		}

		/// <summary>
		/// Сохраняет пресет в файл.
		/// </summary>
		public static void SaveToFile(string pathToFile, MumpsObjectSet set)
		{
			using (var fileStream = new FileStream(pathToFile, FileMode.OpenOrCreate))
			using (var streamWriter = new StreamWriter(fileStream))
			using (var jsonTextWriter = new JsonTextWriter(streamWriter))
			{
				var serializer = new JsonSerializer();
				serializer.Serialize(jsonTextWriter, set);
			}
		}
	}
}
