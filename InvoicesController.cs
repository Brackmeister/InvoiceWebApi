using Microsoft.AspNet.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceWebApi
{
    [Route("/api/invoices")]
    public class InvoicesController : Controller
    {
        [HttpGet]
        public IEnumerable<Invoice> Get()
        {
            using (var conn = new NpgsqlConnection("Host=pgdb;Username=postgres;Password=P@ssw0rd!"))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    // Insert some data
                    cmd.CommandText = "SELECT * FROM Invoices;";
                    var result = new List<Invoice>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Invoice
                            {
                                ID = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                Customer = reader.GetString(2)
                            });
                        }

                        return result;
                    }
                }
            }
        }
    }
}
