namespace SapconCore.Mumps
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using MSMOLE;
	using NLog;

	/// <summary>
	/// Оболочка над библиотекой MSMOLE пакета MsmActivate
	/// </summary>
	public class MsmActivate
	{
		#region Fields
		private const int MSM_CONNECTION_TIMEOUT       = 7200;
		private const char MUMPS_INDEX_DEVIDER         = '\u0000';
		private const char MUMPS_DATA_DEVIDER          = '\u0001';
		private const string MSM_EXEC_ERR_CODE         = "MUMPS_EXCEPTION:";
		private Object thisLock                        = new object();
		private static Logger logger                   = LogManager.GetCurrentClassLogger();
		#endregion Fields

		#region Properties
		private MCommand MCommand { get; set; }

		public int Connected { get { return MCommand.Connected; } }
		#endregion Properties

		#region Constructors
		public MsmActivate()
		{
			MCommand = new MCommand();
		}
		#endregion Constructors

		#region Methods
		public void Init(string server, short port, UCI uci, VOL vol, string userName)
		{
			InitMsmConnection(server, port, uci, vol, userName, MSM_CONNECTION_TIMEOUT);
		}

		public void Init(string server, short port, UCI uci, VOL vol, string userName, short partsize)
		{
			InitMsmConnection(server, port, uci, vol, userName, partsize);
		}

		private void InitMsmConnection(string server, short port, UCI uci, VOL vol, string userName, short? partsize)
		{
			Contract.Requires<ArgumentOutOfRangeException>(port > 0, $"Порт MSM не может быть:{port}");
			Contract.Requires<ArgumentOutOfRangeException>(partsize > 0, $"размер буфера MSM не может быть:{partsize}");
			Contract.Requires<ArgumentNullException>(server != null, "MSM сервер не задан.");
			Contract.Requires<ArgumentNullException>(userName != null, "MSM пользователь не задан.");

			MCommand.Server = server;
			MCommand.Port = port;
			MCommand.Username = userName;
			MCommand.UCI = uci.ToString();
			MCommand.Volgrp = vol.ToString();
			MCommand.Timeout = MSM_CONNECTION_TIMEOUT;
			MCommand.LocalTimeout = MSM_CONNECTION_TIMEOUT;
			MCommand.Partsize = 7200;
		}

		public void Login(string password)
		{
			try
			{
				MCommand.Login(password);
			}
			catch
			{
				throw new MsmConnectionException("Ошибка подключения к Msm!");
			}
		}

		public void Logout()
		{
			try
			{
				MCommand.Logout();
			}
			catch
			{
				throw new MsmConnectionException("Ошибка отключения от Msm!");
			}
		}

		public object Do(MProgram doMProg, MProgram mProg, params object[] param)
		{
			Contract.Requires<MsmConnectionException>(MCommand.Connected > 0, "Соединение с MSM разорванно.");
			Contract.Requires<MumpsExecutionException>(MCommand.UCI == mProg.UCI.ToString(), "Запускаемая программа должна находится в том же разделе (используйте DoRemote).");
			Contract.Requires<MumpsExecutionException>(MCommand.Volgrp == mProg.VOL.ToString(), "Запускаемая программа должна находится в том же томе (используйте DoRemote).");

			object ret = MCommand.Do(doMProg.MumpsFullName, mProg.MumpsFullName, param);
			CheckReturnedError(ret.ToString(), mProg);
			return ret;
		}

		public object DoRemote(MProgram doMProg, MProgram mProg, params object[] param)
		{
			Contract.Requires<MsmConnectionException>(MCommand.Connected > 0, "Соединение с MSM разорванно.");

			object ret = MCommand.Do(doMProg.MumpsFullName, mProg.UCI.ToString(), mProg.VOL.ToString(), mProg.MumpsFullName, param);
			CheckReturnedError(ret.ToString(), mProg);
			return ret;
		}

		public object Execute(MProgram execMProg, UCI uci, VOL vol, string execStr)
		{
			Contract.Requires<MsmConnectionException>(MCommand.Connected > 0, "Соединение с MSM разорванно.");

			object ret = MCommand.Do(execMProg.MumpsFullName, uci.ToString(), vol.ToString(), execStr);
			CheckReturnedError(ret.ToString(), execMProg);
			return ret;
		}

		public Dictionary<string, string> GetGlobal(MProgram getGlobMProg, MGlobal mumpsGlobal, bool kill, bool encode)
		{
			Contract.Requires<MsmConnectionException>(MCommand.Connected > 0, "Соединение с MSM разорванно.");

			var global = new Dictionary<string, string>();
			string lastIndex = "", retStr = "";

			while (true)
			{
				var ret = MCommand.Do(getGlobMProg.MumpsFullName, mumpsGlobal.UCI.ToString(), mumpsGlobal.VOL.ToString(), mumpsGlobal.MumpsFullName, lastIndex, kill);

				//Если следующий индекс пустой - конец глобаля.
				if (ret.ToString().Equals(string.Empty)) break;

				CheckReturnedError(ret.ToString(), getGlobMProg);

				if (encode)
					retStr = MsmEncoding.ConvertToWin(ret.ToString());
				else
					retStr = ret;

				var recivedDictionary = retStr
								.Split(MUMPS_INDEX_DEVIDER)
								.Select(x => x.Split(MUMPS_DATA_DEVIDER))
								.ToDictionary(s => s[0], s => s[1]);

				//Заполняем словарь
				foreach (var dictionaryElement in recivedDictionary)
					global.Add(dictionaryElement.Key, dictionaryElement.Value);

				//Определяем последний индекс
				lastIndex = recivedDictionary.Last().Key;
			}
			return global;
		}

		private void CheckReturnedError(string mumpsString, MProgram mumpsProg)
		{
			if (mumpsString.StartsWith(MSM_EXEC_ERR_CODE, StringComparison.CurrentCultureIgnoreCase))
			{
				var exCode = MumpsErrCodeHelper.Parse(mumpsString);
				var error = new MumpsExecutionException($"Ошибка выполнения Mumps программы:{mumpsProg.MumpsFullName}. \nКод ошибки:{exCode}");
				logger.Error(error);
				throw error;
			}
		}
		#endregion Methods
	}
}
