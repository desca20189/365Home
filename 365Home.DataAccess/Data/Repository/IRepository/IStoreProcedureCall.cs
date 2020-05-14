using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace _365Home.DataAccess.Data.Repository.IRepository
{
    public interface IStoreProcedureCall : IDisposable
    {
        IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param = null);

        void ExecuteWithoutReturn(string procedureName, DynamicParameters param = null);

        //RETURN WITH 1 VALUE
        T ExecuceReturnScaler<T>(string procedureName, DynamicParameters param = null);

    }
}
