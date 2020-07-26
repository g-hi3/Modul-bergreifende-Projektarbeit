using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Repository {
    public abstract class RepositoryBase<M> : IRepositoryBase<M> {

        protected RepositoryBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public long Count()
        {
            using var connection = new MySqlConnection(ConnectionString);
            using var command = PrepareCommand(CountAllCommandText, connection);
            connection.Open();
            return (long) command.ExecuteScalar();
        }

        public long Count(string whereCondition, Dictionary<string, object> parameterValues)
        {
            var preparedString = CountAllCommandText + ConvertWhereCondition(parameterValues);
            using var connection = new MySqlConnection(ConnectionString);
            using var command = PrepareCommand(preparedString, connection);
            return (long) command.ExecuteScalar();
        }

        protected MySqlCommand PrepareCommand(string commandText, MySqlConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        protected string ConvertWhereCondition(Dictionary<string, object> parameterValues)
        {
            var whereCondition = new StringBuilder("WHERE ");
            foreach (var (key, value) in parameterValues)
            {
                whereCondition.Append($"{key} = ");
                
                if (value is string || value is DateTime)
                    whereCondition.Append($"'{value}'");
                else
                    whereCondition.Append(value);

                whereCondition.Append(" AND ");
            }

            whereCondition.Remove(whereCondition.Length - 6, 5);
            return whereCondition.ToString();
        }

        public abstract M GetSingle<P>(P pkValue);

        public abstract void Add(M entity);

        public abstract void Delete(M entity);

        public abstract void Update(M entity);

        public abstract List<M> GetAll(string whereCondition, Dictionary<string, object> parameterValues);

        public abstract List<M> GetAll();


        public abstract string TableName { get; }

        private string CountAllCommandText => $"select count(*) from {TableName}";
        
        protected string ConnectionString { get; }
        
    }

}