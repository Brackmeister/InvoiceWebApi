﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Npgsql;
using Dapper;

namespace InvoiceWebApi
{
    [Route("/api/invoices")]
    public class InvoicesController : Controller
    {
        private const string ConnectionString = "Host=pgdb,Username=postgres;Password=P@ssw0rd!";

        [HttpGet]
        public async Task<IEnumerable<Invoice>> Get()
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                await conn.OpenAsync();
                return await conn.QueryAsync<Invoice>("select * from invoices;");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Invoice newInvoice)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "insert into invoices values (@Id, @Description, @Customer)";
                    cmd.Parameters.AddWithValue("@Id", newInvoice.Id);
                    cmd.Parameters.AddWithValue("@Description", newInvoice.Description);
                    cmd.Parameters.AddWithValue("@Customer", newInvoice.Customer);
                    await cmd.ExecuteNonQueryAsync();
                    return this.Ok();
                }
            }
        }
    }
}
