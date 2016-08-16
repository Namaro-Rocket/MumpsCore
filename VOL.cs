namespace SapconCore.Mumps
{
	/// <summary>
	/// Представляет собой физические тома в Mumps (корень).
	/// Provides the physycal volumes list members in mumps.
	/// </summary>
	public enum VOL
	{
		/// <summary>
		/// DWA - MUMPS main system VOL.
		/// </summary>
		DWA,

		/// <summary>
		/// MTC - main storage VOL.
		/// </summary>
		MTC,

		/// <summary>
		/// OLD - outdated VOL. To support prev. programms.
		/// </summary>
		OLD,

		/// <summary>
		/// STM - ??? VOL.
		/// </summary>
		STM,

		/// <summary>
		/// EML - ??? VOL.
		/// </summary>
		EML,

		/// <summary>
		/// ARH - archive VOL.
		/// </summary>
		ARH,

		/// <summary>
		/// RZK - ??? VOL.
		/// </summary>
		RZK,

		/// <summary>
		/// TTT - tech documentation VOL.
		/// </summary>
		TTT,

		/// <summary>
		/// DDD - Personal VOL used by Bashkirov Dmitriy.
		/// </summary>
		DDD,

		/// <summary>
		/// PDF - pdf documentation VOL.
		/// </summary>
		PDF
	}
}
