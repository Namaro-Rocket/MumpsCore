namespace SapconCore.Mumps
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Оболочка над MsmActivate
	/// </summary>
	public class MsmConnection
	{
		private MumpsConnectionList _conList;
		private MumpsObjectSet _objSet;

		const string MUMPS_CONTROL_USER = "SA";
		object currentLock = new object();
		MsmActivate msmActivate;
		bool established;

		#region Parametrs
		/// <summary>
		/// Название соединения.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Адрес сервера Msm соединения.
		/// </summary>
		public string Server { get; }

		/// <summary>
		/// Номер порта Msm соединения.
		/// </summary>
		public short Port { get; }

		/// <summary>
		/// Физический раздел к которому подключенно соединение (кип).
		/// </summary>
		public UCI UCI { get; }

		/// <summary>
		/// Логический раздел к которому подключенно соединение (том).
		/// </summary>
		public VOL VOL { get; }

		/// <summary>
		/// Статус подключения.
		/// </summary>
		public ConnectionStatus Status
		{
			get
			{
				if (!established)
					return ConnectionStatus.UnInitialized;
				if (established && !Online)
					return ConnectionStatus.Disconnected;
				if (established && Online)
					return ConnectionStatus.Connected;

				else throw new MumpsGeneralException("Возвращен неожиданный статус подключения.");
			}
		}

		/// <summary>
		/// Номер задания на Msm сервере.
		/// </summary>
		public int Job { get { return msmActivate.Connected; } }

		/// <summary>
		/// Статус подключения.
		/// </summary>
		public bool Online { get { return msmActivate.Connected != 0; } }
		#endregion

		#region Constructors
		/// <summary>
		/// Создает экземпляр подключения к Msm серверу.
		/// </summary>
		/// <param name="connectionName">Уникальное имя соединения</param>
		public MsmConnection(MumpsConnectionList conList, MumpsObjectSet objSet, string connectionName, string server, short port, UCI uci, VOL vol)
		{
			Server = server;
			Port = port;
			UCI = uci;
			VOL = vol;


			_conList = conList;
			_objSet  = objSet;

			conList.Add(this);

			//Костыли-велосипеды. иначе идет lock на основной поток с ui.
			msmActivate = Task.Factory.StartNew(() => { return new MsmActivate(); }).Result;

			established = false;

			Name   = connectionName;

			msmActivate.Init(
				 this.Server,
				 this.Port,
				 this.UCI,
				 this.VOL,
				 MUMPS_CONTROL_USER);
		}
		#endregion

		/// <summary>
		/// Устанавливает соединение с Msm сервером по проинициализированным параметрам.
		/// </summary>
		public void Establish()
		{
			this.msmActivate.Login(MUMPS_CONTROL_USER);
		}

		/// <summary>
		/// Разрывает текущее соединение с Msm
		/// </summary>
		public void Terminate()
		{
			this.msmActivate.Logout();
		}

		public object Execute(MProgram mProg, string[] mParams)
		{
			var doProg       = _objSet.FindProgram(MProgramNames.MumpsDo);
			var doRemoteProg = _objSet.FindProgram(MProgramNames.MumpsDoRemote);

			lock (currentLock)
			{
				return (mProg.UCI == UCI && mProg.VOL == VOL) ?
					msmActivate.Do(doProg, mProg, mParams) :
					msmActivate.DoRemote(doRemoteProg, mProg, mParams);
			}
		}

		public object Execute(UCI uci, VOL vol, string execStr)
		{
			var execProg = _objSet.FindProgram(MProgramNames.MumpsExecute);

			lock (currentLock)
			{
				var ret = msmActivate.Execute(execProg, uci, vol, execStr);
				return ret;
			}
		}

		public object Execute(string execStr)
		{
			var execProg = _objSet.FindProgram(MProgramNames.MumpsExecute);

			lock (currentLock)
			{
				var ret = msmActivate.Execute(execProg, UCI, VOL, execStr);
				return ret;
			}
		}

		public async Task<object> ExecuteAsync(MProgram mProg, string[] mParams)
		{
			var execProg = _objSet.FindProgram(MProgramNames.MumpsExecute);

			return await TaskEx.Run(() =>
			{
				return Execute(mProg, mParams);
			}).ConfigureAwait(false);
		}

		public async Task<object> ExecuteAsync(string execStr)
		{
			return await TaskEx.Run(() =>	{ return Execute(execStr);	}).ConfigureAwait(false);
		}

		public async Task<object> ExecuteAsync(UCI uci, VOL vol, string execStr)
		{
			return await TaskEx.Run(() =>	{ return Execute(uci, vol, execStr); }).ConfigureAwait(false);
		}

		public Dictionary<string, string> GetGlobal(MGlobal mGlob, bool kill, bool encode)
		{
			var getGlobProg = _objSet.FindProgram(MProgramNames.MumpsGetGlobal);

			lock (currentLock) { return msmActivate.GetGlobal(getGlobProg, mGlob, kill, encode);	}
		}

		public async Task<Dictionary<string, string>> GetGlobalAsync(MGlobal mGlob, bool kill, bool encode)
		{
			return await TaskEx.Run(() =>	{ lock (currentLock) { return GetGlobal(mGlob, kill, encode); }}).ConfigureAwait(false);
		}

		public override bool Equals(object obj)
		{
			var con = obj as MsmConnection;

			if (con != null)
				return this.Name == con.Name;
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
	}
}
