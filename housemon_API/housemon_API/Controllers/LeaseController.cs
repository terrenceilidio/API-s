using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using housemon_API.Models;
using Microsoft.AspNetCore.Cors;

namespace housemon_API.Controllers
{
    public class LeaseController
    {

        private readonly PropertyMonitorDbContext dbcontext;
        public LeaseController(PropertyMonitorDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [Route("create/Lease")]
        [HttpPost]
        public ResponseModel<Lease> createLease([FromBody] Lease lease)
        {
            try
            {
                dbcontext.leases.Add(lease);
                dbcontext.SaveChanges();
                return new ResponseModel<Lease>
                {
                    data = null,
                    message = "",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Lease>
                {
                    data = null,
                    message = "unable to add lease " + ex.Message,
                    status = true
                };
            }
        }

    }
}
