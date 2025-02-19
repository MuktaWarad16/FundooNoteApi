using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CommonLayer;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;

namespace FundooWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorsController : ControllerBase
    {
        private readonly ICollaboratorManager manager;
        private readonly FundooDBContext context;
        private readonly IBus bus;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<CollaboratorsController> logger;

        public CollaboratorsController(ICollaboratorManager manager, FundooDBContext context, IBus bus, IDistributedCache distributedCache, ILogger<CollaboratorsController> logger)
        {
            this.manager = manager;
            this.context = context;
            this.bus = bus;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        [Authorize]
        [HttpPost("addcollaborators")]
        public IActionResult AddCollaborator(CollaboratorModel model)
        {
            try
            {
                if (manager.MailExists(model.Email))
                {
                    string data = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                    string email = User.Claims.FirstOrDefault(c => c.Type == "Email").Value;
                    CollaboratorEntity response = manager.AddCollaborator(model, Convert.ToInt32(data));
                    SendCollabModel send = new SendCollabModel();
                    send.SendMessage(model.Email, model.NotesId,model.CollaboratorId);
                   // Uri uri = new Uri("rabbitmq://localhost/FundooNotesEmailQueue");
                    //var endPoint = await bus.GetSendEndpoint(uri);
                    //await endPoint.Send(model);

                    return Ok(new ResponseModel<string> { success = true, message = "Collaborator add successfully" });
                }
                return Ok(new ResponseModel<string> { success = false, message = "Failed to add Collaborator" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("getCollaborators")]
        public IActionResult GetAllCollaborators(int userId)
        {
            try
            {
                var response = manager.GetAllCollaborators(userId);
                logger.LogInformation("fetched {Count} collaborators", response.Count);

                return Ok(new ResponseModel<List<CollaboratorEntity>> { success = true, message = "fetched all collaborators", Data = response });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<List<CollaboratorEntity>> { success = true, message = "failed to fetch" });
            }
            //catch (Exception ex)
            //{
            //    logger.LogError("Unexpected error: {Error}", ex.ToString());
            //    return StatusCode(500, new ResponseModel<List<CollaboratorEntity>> { success = false, message = "An unexpected error occured. Please try again later" });
            //}
        }

        [Authorize]
        [HttpDelete("deletecollaborators")]
        public IActionResult DeleteCollaborator(int collaboratorId, int notesId)
        {
            try
            {
                string data = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var response = manager.DeleteCollaborator(Convert.ToInt32(data), collaboratorId, notesId);

                logger.LogInformation(response.ToString());
                return Ok(new ResponseModel<bool> { success = true, message = "Delete successful", Data = response });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<bool> { success = false, message = "failed to delete" });
            }
        }


        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllCollaboratorsUsingRedisCache()
        {
            var cacheKey = "CollabList";
            string SerializedCollabLst;
            var CollabList = new List<CollaboratorEntity>();
            var RedisCollabList = await distributedCache.GetAsync(cacheKey);
            if (RedisCollabList != null)
            {
                SerializedCollabLst = Encoding.UTF8.GetString(RedisCollabList);
                CollabList = JsonConvert.DeserializeObject<List<CollaboratorEntity>>(SerializedCollabLst);
            }
            else
            {
                CollabList = context.CollaboratorEntity.ToList();
                SerializedCollabLst = JsonConvert.SerializeObject(CollabList);
                RedisCollabList = Encoding.UTF8.GetBytes(SerializedCollabLst);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(20))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                await distributedCache.SetAsync(cacheKey, RedisCollabList, options);
            }
            return Ok(CollabList);
        }

    }
}