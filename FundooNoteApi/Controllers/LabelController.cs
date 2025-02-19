using CommonLayer;
using System.Linq;
using System;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace FundooWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelManager manager;
        private readonly IDistributedCache distributedCache;
        private readonly FundooDBContext context;
        private readonly ILogger<LabelsController> logger;
       // private List<LabelEntity> labelList;
       // private object cachekey;

        public LabelsController(ILabelManager manager, FundooDBContext context, IDistributedCache distributedCache, ILogger<LabelsController> logger)
        {
            this.manager = manager;
            this.context = context;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("addLabel")]
        public IActionResult AddLabel(LabelModel model)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var label = manager.AddLabel(model, Convert.ToInt32(userId));
                logger.LogInformation(label.ToString());
                return Ok(new ResponseModel<LabelEntity> { success = true, message = "label added successfully", Data = label });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<LabelEntity> { success = false, message = "failed to add label" });
            }
        }

        [Authorize]
        [HttpDelete("updateLabel")]
        public IActionResult Update(int labelId, string labelName)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                bool result = manager.UpdateLabel(Convert.ToInt32(userId), labelId, labelName);
                logger.LogInformation(result.ToString());
                return Ok(new ResponseModel<bool> { success = true, message = "Updated successful", Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<bool> { success = false, message = "failed to Update" });
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult Delete(int labelId)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                bool result = manager.DeleteLabel(Convert.ToInt32(userId), labelId);
                logger.LogInformation(result.ToString());
                return Ok(new ResponseModel<bool> { success = true, message = "Deleted successful", Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<bool> { success = false, message = "failed to delete" });
            }
        }

        [Authorize]
        [HttpGet("getAllLabels")]
        public ActionResult GetLabels()
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                List<LabelEntity> labels = manager.GetAllLabels(Convert.ToInt32(userId));
                logger.LogInformation(labels.ToString());
                return Ok(new ResponseModel<List<LabelEntity>> { success = true, message = "fetch successful", Data = labels });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<List<LabelEntity>> { success = true, message = "failed to fetch" });
            }
        }

        [HttpGet]
        [Route("RedisGetallUsers")]
        public async Task<IActionResult> GetAllUsersUsingRedisCache()
        {
            var cacheKey = "UsersList";
            string serializedLabelList;
            var labelList = new List<LabelEntity>();
            var redisLabelList = await distributedCache.GetAsync(cacheKey);
            if (redisLabelList != null)
            {
                serializedLabelList = Encoding.UTF8.GetString(redisLabelList);
                labelList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelList);
            }
            else
            {
                labelList = context.LabelEntity.ToList();
                serializedLabelList = JsonConvert.SerializeObject(labelList);
                redisLabelList = Encoding.UTF8.GetBytes(serializedLabelList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(20))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                await distributedCache.SetAsync(cacheKey, redisLabelList, options);
            }
            return Ok(labelList);
        }
    }
}