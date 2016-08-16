namespace SapconCore.Mumps
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;

	/// <summary>
	/// Провайдер доступа к соединениям с Mumps.
	/// </summary>
	public sealed class MumpsConnectionList
	{
		public const int DEFAULT_MAX_CONNECTION = 3;

		private List<MsmConnection> _connections = new List<MsmConnection>();
		private int _maxConnections;

		/// <summary>
		/// 
		/// </summary>
		public MsmConnection MainConnection
		{
			get { return Find(UCI.MGR, VOL.DWA); }
		}

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<MsmConnection> Connections
		{
			get { return _connections; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int MaxConnections
		{
			get { return _maxConnections; }
		}

		public MumpsConnectionList(int maxConnections)
		{
			//Contract.Requires<ArgumentOutOfRangeException>(maxConnections > 0 && maxConnections < 10, $"Не верное количество соединений:{maxConnections}. \nЧисло соединений должно находится в пределах 1 и 9");

			_maxConnections = maxConnections;
		}

		public bool ConnectionExist(MsmConnection connection)
		{
			Contract.Requires<ArgumentNullException>(connection != null, "Аргумент connection равен null");

			return Connections.Contains(connection);
		}

		public MsmConnection Find(string connectionName)
		{
			return Connections.FirstOrDefault(con => con.Name == connectionName);
		}

		public MsmConnection Find(UCI uci, VOL vol)
		{
			return Connections.FirstOrDefault(con => con.UCI == uci && con.VOL == vol);
		}

		public void Add(MsmConnection connection)
		{
			//Contract.Requires<MsmConnectionException>(_connections.Count() >= _maxConnections, $"Максимальное количество соединений уже установленно({MaxConnections})! Используйте рабочие соединения.");
			//Contract.Requires<MsmConnectionException>(ConnectionExist(connection), $"Соединение с именем:{connection.Name} уже существует.");

			_connections.Add(connection);
		}
	}
}