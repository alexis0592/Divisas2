using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using Taller2Divisas.Interfaces;
using Taller2Divisas.Models;
using Xamarin.Forms;

namespace Taller2Divisas.Data
{
    public class DataAccess : IDisposable
    {
        #region Attribute
        private SQLiteConnection connection;
        #endregion

        #region Constructor
        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            connection = new SQLiteConnection(config.Platform,
                                              System.IO.Path.Combine(config.DirectoryDB,
                                                                     "Divisas2.db3"));
            connection.CreateTable<Rate>();
            connection.CreateTable<LastSearch>();
        }
		#endregion

		#region Methods
		public void Insert<T>(T model)
		{
			connection.Insert(model);
		}

		public void Update<T>(T model)
		{
			connection.Update(model);
		}

		public void Delete<T>(T model)
		{
			connection.Delete(model);
		}

		public T First<T>(bool WithChildren) where T : class
		{
			if (WithChildren)
			{
				return connection.GetAllWithChildren<T>().FirstOrDefault();
			}
			else
			{
				return connection.Table<T>().FirstOrDefault();
			}
		}

		public List<T> GetList<T>(bool WithChildren) where T : class
		{
			if (WithChildren)
			{
				return connection.GetAllWithChildren<T>().ToList();
			}
			else
			{
				return connection.Table<T>().ToList();
			}
		}

		public T Find<T>(int pk, bool WithChildren) where T : class
		{
			if (WithChildren)
			{
				return connection.GetAllWithChildren<T>().FirstOrDefault(m => m.GetHashCode() == pk);
			}
			else
			{
				return connection.Table<T>().FirstOrDefault(m => m.GetHashCode() == pk);
			}
		}

		#endregion

		public void Dispose()
        {
            connection.Dispose();
        }
    }
}
