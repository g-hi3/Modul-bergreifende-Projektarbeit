using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Repository {
    
    public class LocationRepository : RepositoryBase<LocationModel>
    {

        private const string ColumnNameId = "LocationID";
        private const string ColumnNameAddressId = "LocAdresseId_FK";
        private const string ColumnNameDeviceId = "LocGeraetID_FK";
        private const string ColumnNameName = "LocName";
        
        public LocationRepository(string connectionString) : base(connectionString)
        {
        }

        public override LocationModel GetSingle<P>(P pkValue)
        {
            if (pkValue is LocationPrimaryKey locationPrimaryKey)
                return GetSingle(locationPrimaryKey);
            throw new Exception($"Primary key is not applicable to {TableName}!");
        }

        public LocationModel GetSingle(LocationPrimaryKey pkValue)
        {
            var preparedCommandText = SelectCommandText + " WHERE " + pkValue.ToWhereCondition();
            using var connection = new MySqlConnection(ConnectionString);
            using var command = PrepareCommand(preparedCommandText, connection);
            using var reader = command.ExecuteReader();
            if (!reader.NextResult())
                throw new Exception($"{TableName} Model could not be found for primary key {pkValue}"!);
            return ToLocationModel(reader);
        }

        public override void Add(LocationModel entity)
        {
            var id = entity.Id;
            var addressId = entity.AddressId;
            var deviceId = entity.DeviceId;
            var name = entity.Name;
            var preparedCommandText = string.Format(InsertCommandText, id, addressId, deviceId, name);
            using var connection = new MySqlConnection(ConnectionString);
            using var command = PrepareCommand(preparedCommandText, connection);
            command.ExecuteNonQuery();
        }

        public override void Delete(LocationModel entity)
        {
            var id = entity.Id;
            var preparedCommandText = string.Format(DeleteCommandText, id);
            using var connection = new MySqlConnection(ConnectionString);
            using var command = PrepareCommand(preparedCommandText, connection);
            command.ExecuteNonQuery();
        }

        public override void Update(LocationModel entity)
        {
            var id = entity.Id;
            var addressId = entity.AddressId;
            var deviceId = entity.DeviceId;
            var name = entity.Name;
            var preparedCommandText = string.Format(UpdateCommandText, id, addressId, deviceId, name);
            using var connection = new MySqlConnection(ConnectionString);
            using var command = PrepareCommand(preparedCommandText, connection);
            command.ExecuteNonQuery();
        }

        public override List<LocationModel> GetAll(string whereCondition, Dictionary<string, object> parameterValues)
        {
            var preparedCommandText = SelectCommandText + ConvertWhereCondition(parameterValues);
            using var connection = new MySqlConnection(ConnectionString);
            using var command = PrepareCommand(preparedCommandText, connection);
            using var reader = command.ExecuteReader();
            var all = new List<LocationModel>();
            while (reader.NextResult())
            {
                var location = ToLocationModel(reader);
                all.Add(location);
            }
            return all;
        }

        public override List<LocationModel> GetAll()
        {
            var preparedCommandText = SelectCommandText;
            using var connection = new MySqlConnection(ConnectionString);
            using var command = PrepareCommand(preparedCommandText, connection);
            using var reader = command.ExecuteReader();
            var all = new List<LocationModel>();
            while (reader.NextResult())
            {
                var location = ToLocationModel(reader);
                all.Add(location);
            }
            return all;
        }

        private static LocationModel ToLocationModel(MySqlDataReader reader)
        {
            var location = new LocationModel
            {
                Id = reader.GetString(ColumnNameId),
                AddressId = reader.GetString(ColumnNameAddressId),
                DeviceId = reader.GetString(ColumnNameDeviceId),
                Name = reader.GetString(ColumnNameName)
            };
            return location;
        }

        private static string InsertCommandText => @"INSERT INTO `inventar_sql_live`.`location`
(`LocationID`, `LocAdresseID_FK`, `LocGeraetID_FK`, `LocName`)
VALUES (<{0}>, <{1}>, <{2}>, <{3}>)";

        private static string SelectCommandText => @"SELECT `location`.`LocationID`,
`location`.`LocAdresseID_FK`,
`location`.`LocGeraetID_FK`,
`location`.`LocName`
FROM `inventar_sql_live`.`location`";

        private static string UpdateCommandText => @"UPDATE `inventar_sql_live`.`location`
SET `LocAdresseID_FK` = <{1}>, `LocGeraetID_FK` = <{2}>, `LocName` = <{3}>
WHERE `LocationID` = <{0}>";

        private static string DeleteCommandText => @"DELETE FROM `inventar_sql_live`.`location`
WHERE Id = {0}";

        public override string TableName => "`inventar_sql_live`.`location`";
    }
    
}