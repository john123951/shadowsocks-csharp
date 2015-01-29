using System;

namespace Sweet.LoveWinne.Model
{
	public class ServerDto
	{
		public long Id { get; set; }

		public string Server { set; get; }

		public int ServerPort { set; get; }

		public int LocalPort { set; get; }

		public string Password { set; get; }

		public string Method { set; get; }

		public string Remarks { set; get; }
	}
}

