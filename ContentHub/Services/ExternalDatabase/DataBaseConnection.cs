//using ContentHub.Models;
//using HtmlAgilityPack;
//using Npgsql;
//using System;
//using System.Data;

//namespace ContentHub.Services.Database
//{
//    public class DataBaseConnection : IDataBaseConnection
//    {
//        private readonly string _connectionString = "";
//        public string GetContentFromDatabase()
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<DateTime> GetLastUpdatedFromDatabase()
//        {


//        }

//        public async Task UpdateDatabase(string url, DateTime? lastModified, List<ImageAttributes> images, List<WordCounts> wordFrequencies)
//        {
//            using (var connection = new NpgsqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();
//            }
//        }
//    }

//}

