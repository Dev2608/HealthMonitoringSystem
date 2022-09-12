using HealthMonitoringSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace HealthMonitoringSystem.Controllers
{
    [RoutePrefix("api/AndroidAppApis")]
    public class AndroidAppApisController : ApiController
    {
        HealthMonitoringSystemEntities db = new HealthMonitoringSystemEntities();

        [Route("UserRegistration")]
        [HttpPost]
        public IHttpActionResult UserRegistration([FromBody] AndroidApp userObj)
        {
            if (db.AndroidApps.Any(alias => alias.UserName.Equals(userObj.UserName)))
            {
                return BadRequest("UserName already exists.");
            }
            else
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Membership.CreateUser(userObj.UserName, userObj.Password);
                        db.AndroidApps.Add(userObj);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest(ex.Message);
                    }
                }
            }
            return Ok();
        }
    }
}
