using System;
using SQLite.Net.Interop;

namespace Taller2Divisas.Interfaces
{
    public interface IConfig
    {
		string DirectoryDB { get; }

		ISQLitePlatform Platform { get; }
    }
}
