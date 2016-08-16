namespace SapconCore.Mumps
{
	public interface IMumpsReciveble<T>
	{
		string MumpsPattern { get; }

		T Parse(string mumpsStr);
	}
}
