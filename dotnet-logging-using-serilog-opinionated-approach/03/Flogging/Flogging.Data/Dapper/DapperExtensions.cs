﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flogging.Data.Dapper
{
    public static class DapperExtensions
    {
        public static int DapperProcNonQuery(this IDbConnection db, string procName,
            object paramList = null, IDbTransaction trans = null, int? timeoutSeconds = null)
        {
            try
            {
                return db.Execute(procName, paramList, trans, timeoutSeconds, CommandType.StoredProcedure);
            }
            catch (Exception orig)
            {
                var ex = new Exception($"Dapper proc execution failed!", orig);
                AddDetailsToException(ex, procName, paramList);
                throw ex;
            }
        }
        private static void AddDetailsToException(Exception ex, string procName, object paramList)
        {
            ex.Data.Add("Procedure", procName);
            if (paramList is DynamicParameters dynParams)  //C# 7 syntax
            {
                foreach (var p in dynParams.ParameterNames)
                {
                    ex.Data.Add(p, dynParams.Get<object>(p).ToString());
                }
            }
            else
            {
                var props = paramList.GetType().GetProperties();
                foreach (var prop in props)
                {
                    ex.Data.Add(prop.Name, prop.GetValue(paramList).ToString());
                }
            }
        }
    }
}
