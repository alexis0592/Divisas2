using System;
using Taller2Divisas.Data;
using System.Collections.Generic;
using System.Linq;

namespace Taller2Divisas.Services
                        
{
    public class DataService
    {


		#region Methods
		public bool DeleteAll<T>() where T : class
		{
			try
			{
				using (var da = new DataAccess())
				{
					var oldRecords = da.GetList<T>(false);
					foreach (var oldRecord in oldRecords)
					{
						da.Delete(oldRecord);
					}

					//da.Insert(model);

					return true;
				}
			}
			catch (Exception ex)
			{
				ex.ToString();
                return false;
			}
		}

		public List<T> Get<T>(bool withChildren) where T : class
		{
			using (var da = new DataAccess())
			{
				return da.GetList<T>(withChildren).ToList();
			}
		}

		public void Save<T>(List<T> list) where T : class
		{
			using (var da = new DataAccess())
			{
				foreach (var record in list)
				{
					InsertOrUpdate(record);
				}
			}
		}

		public T InsertOrUpdate<T>(T model) where T : class
		{
			try
			{
				using (var da = new DataAccess())
				{
					var oldRecord = da.Find<T>(model.GetHashCode(), false);
					if (oldRecord != null)
					{
						da.Update(model);
					}
					else
					{
						da.Insert(model);
					}

					return model;
				}
			}
			catch (Exception ex)
			{
				ex.ToString();
				return model;
			}
		}

		public T Insert<T>(T model)
		{
			using (var da = new DataAccess())
			{
				da.Insert(model);
				return model;
			}
		}

		public T Find<T>(int pk, bool withChildren) where T : class
		{
			using (var da = new DataAccess())
			{
				return da.Find<T>(pk, withChildren);
			}
		}
        #endregion
    }
}
