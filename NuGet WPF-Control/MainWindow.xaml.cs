﻿using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace NuGet_WPF_Control
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private const string LoadSqlQuery =
            "SELECT id, pod, location, hostname, severity, timestamp, message FROM v_logentries";

        private const string ClearSqlQuery = "CALL `inventar_sql_live`.`ClearLog`({0})";
        private const string AddSqlQuery = "CALL `inventar_sql_live`.`LogMessageAdd`({0}, {1}, {2}, {3})";
        private string _connectionString;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                if (_connectionString == value)
                    return;

                _connectionString = value;
                OnPropertyChanged();
            }
        }

        public DataTable DataTable { get; } = new DataTable();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnClickLoad(object sender, RoutedEventArgs e)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(LoadSqlQuery, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                var adapter = new SqlDataAdapter(command);
                adapter.Fill(DataTable);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Connection string could not be used to connect to a database!\n" + exc.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void OnClickConfirm(object sender, RoutedEventArgs e)
        {
            var selectedIndex = DataGrid.SelectedIndex;
            var clearQuery = string.Format(ClearSqlQuery, selectedIndex);
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(clearQuery, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Database operation failed!", exc.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void OnClickAdd(object sender, RoutedEventArgs e)
        {
            var deviceId = DeviceId.Text;
            var deviceName = DeviceName.Text;
            var logLevel = LogLevel.Text;
            var logMessage = LogMessage.Text;
            var addQuery = string.Format(AddSqlQuery, deviceId, deviceName, logLevel, logMessage);
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(addQuery, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Database operation failed!", exc.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}