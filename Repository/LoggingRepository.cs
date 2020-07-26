using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Repository {
    
    public class LoggingRepository : RepositoryBase<LoggingModel>
    {

        private const string ColumnNameId = "Id";
        private const string ColumnNamePod = "Pod";
        private const string ColumnNameLocation = "Location";
        private const string ColumnNameHostname = "Hostname";
        private const string ColumnNameSeverity = "Severity";
        private const string ColumnNameTimestamp = "Timestamp";
        private const string ColumnNameMessage = "Message";
        
        public LoggingRepository(string connectionString) : base(connectionString)
        {
        }

        public override LoggingModel GetSingle<P>(P pkValue)
        {
            if (pkValue is LoggingPrimaryKey locationPrimaryKey)
                return GetSingle(locationPrimaryKey);
            throw new Exception($"Primary key is not applicable to {TableName}!");
        }

        public LoggingModel GetSingle(LoggingPrimaryKey pkValue)
        {
            var preparedCommandText = SelectCommandText + " WHERE " + pkValue.ToWhereCondition();
            using var connection = new MySqlConnection(ConnectionString);
            using var command = PrepareCommand(preparedCommandText, connection);
            using var reader = command.ExecuteReader();
            if (!reader.NextResult())
                throw new Exception($"{TableName} Model could not be found for primary key {pkValue}"!);
            return ToLoggingModel(reader);
        }

        public override void Add(LoggingModel entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(LoggingModel entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(LoggingModel entity)
        {
            throw new NotImplementedException();
        }

        public override List<LoggingModel> GetAll(string whereCondition, Dictionary<string, object> parameterValues)
        {
            var preparedCommandText = SelectCommandText + ConvertWhereCondition(parameterValues);
            using var connection = new MySqlConnection(ConnectionString);
            using var command = PrepareCommand(preparedCommandText, connection);
            using var reader = command.ExecuteReader();
            var all = new List<LoggingModel>();
            while (reader.NextResult())
            {
                var logging = ToLoggingModel(reader);
                all.Add(logging);
            }
            return all;
        }

        public override List<LoggingModel> GetAll()
        {
            using var connection = new MySqlConnection(ConnectionString);
            using var command = PrepareCommand(SelectCommandText, connection);
            using var reader = command.ExecuteReader();
            var all = new List<LoggingModel>();
            while (reader.NextResult())
            {
                var logging = ToLoggingModel(reader);
                all.Add(logging);
            }
            return all;
        }

        private static LoggingModel ToLoggingModel(MySqlDataReader reader)
        {
            var logging = new LoggingModel
            {
                Id  = reader.GetString(ColumnNameId),
                Pod  = reader.GetString(ColumnNamePod),
                Location  = reader.GetString(ColumnNameLocation),
                Hostname  = reader.GetString(ColumnNameHostname),
                Severity  = reader.GetString(ColumnNameSeverity),
                Timestamp  = reader.GetDateTime(ColumnNameTimestamp),
                Message  = reader.GetString(ColumnNameMessage)
            };
            return logging;
        }

        private static string SelectCommandText => @"SELECT `v_logentries`.`id`,
    `v_logentries`.`pod`,
    `v_logentries`.`location`,
    `v_logentries`.`hostname`,
    `v_logentries`.`severity`,
    `v_logentries`.`timestamp`,
    `v_logentries`.`message`
FROM `inventar_sql_live`.`v_logentries`";

        public override string TableName => "`inventar_sql_live`.`v_logentries`";
    }
    
}