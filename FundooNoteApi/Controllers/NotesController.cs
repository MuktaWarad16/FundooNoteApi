using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLayer;
using CommonLayer.Models;
using ManagerLayer;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using MSMQ.Messaging;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;


namespace FundooNotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : Controller
    {

        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;
        private readonly INoteManager manager;
        private readonly IDistributedCache distributedCache;

        public NotesController(INoteManager manager, FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
            this.manager = manager;
            this.distributedCache = distributedCache;
        }



        [Authorize]
        [HttpPost("AddNote")]
        public IActionResult AddNotes(NotesModel model)
        {
            string data = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;

            if (string.IsNullOrEmpty(data))
            {
                return Unauthorized(new ResponseModel<NotesEntity> { success = false, message = "User not authorized" });
            }

            var response = manager.AddNotes(Convert.ToInt32(data), model);
            if (response != null)
            {
                return Ok(new ResponseModel<NotesEntity> { success = true, message = "notes added", Data = response });
            }
            else
            {
                return BadRequest(new ResponseModel<NotesEntity> { success = false, message = "notes not added" });
            }


        }


        [Authorize]
        [HttpPost("GetNotes")]
        public IActionResult GetNotes(int Userid)
        {
            string data = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            var response = manager.GetNotes(Convert.ToInt32(data));
            if (response != null)
            {
                return Ok(new ResponseModel<List<NotesEntity>> { success = true, message = "notes retrieved" ,Data=response});

            }
            else
            {
                return BadRequest(new ResponseModel<List<NotesEntity>> { success = false, message = "unable to retrieve" });
            }

        }

        [Authorize]
        [HttpPut("UpdateNotes")]
        public IActionResult UpdateNotes(UpdateNotesModel model)
        {
            var response = User.Claims.FirstOrDefault(c => c.Type == "UserId");

            if (response == null)
            {
                return Unauthorized(new ResponseModel<NotesEntity> { success = false, message = "invalid user authentication"});
            }
            int userId = Convert.ToInt32(response.Value);
            var allNotes = manager.GetNotes(userId);
            var updateNote = manager.UpdateNotes(Convert.ToInt32(userId), model.NotesId, model);


            if (updateNote != null)
            {
                return Ok(new ResponseModel< NotesEntity>{ success = true, message = "notes updated succefully" ,Data=updateNote});
            }
            else
            {
                return BadRequest(new ResponseModel<NotesEntity> { success = false, message = "note not found or unauthorized" });
            }
        }


        [Authorize]
        [HttpDelete("DeleteNotes")]
        public IActionResult DeleteNotes(int Notesid)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");

            if (userClaim == null)
            {
                return Unauthorized(new ResponseModel<bool> { success = false, message = "invalid user authentication" });
            }

            int userId = Convert.ToInt32(userClaim.Value);
            var result = manager.DeleteNotes(Notesid,userId);
            if (result != null)
            {
                return Ok(new ResponseModel<bool> { success = true, message = "notes deleted successfully",Data=result });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { success = false, message = "note not found or unauthorized" });
            }

        }


        [Authorize]
        [HttpPut("ispinunpin")]
        public IActionResult IsPinUnPin(int userId, int NotesId)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");

            if (userClaim == null)
            {
                return Unauthorized(new ResponseModel<bool> { success = false, message = "invalid user authentication" });
            }
            userId = Convert.ToInt32(userClaim.Value);
            var result = manager.IsPinUnpin(userId, NotesId);
            if (result != null)
            {
                return Ok(new ResponseModel<bool> { success = true, message = "notes pin successfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { success = false, message = "notes not pinned" });
            }
        }
        //  -----------------------------------------
        //--------------------------------
        //[Authorize]
        //[HttpGet("GetNotesByTitleOrDescription")]
        //public IActionResult GetNotesByTitleOrDescription(string Title, string Description)
        //{
        //    var filteredNotes = context.NotesEntity.Where(n =>
        //        n.Title.Contains(Title) ||
        //        n.Description.Contains(Description)).ToList();

        //    if (filteredNotes.Any())
        //    {
        //        return Ok(new ResponseModel<List<NotesEntity>> { success = true, message = "get notes by title or description" });
        //    }
        //    else
        //    {
        //        return BadRequest(new ResponseModel<List<NotesEntity>> { success = false, message = "Notes not found" });
        //    }
        //}



        //[Authorize]
        //[HttpGet("GetNotesCount")]
        //public IActionResult GetNotesCount()
        //{

        //    var notescount = context.NotesEntity.Count();

        //    if (notescount != 0)
        //    {
        //        return Ok(new ResponseModel<int> { success = true, message = "Total notes count", Data = notescount });
        //    }
        //    else
        //    {
        //        return BadRequest(new ResponseModel<int> { success = false, message = "No notes found for this user" });
        //    }
        //}

        //[Authorize]
        //[HttpGet("GetNotesCount")]
        //public IActionResult Image()
        //{
        //    string path = "@C:\\Users\\Hp\\Pictures\\admin.jpg";
        //    var img = context.NotesEntity.Count();

        //    if (img != 0)
        //    {
        //        return Ok(new ResponseModel<int> { success = true, message = "color", Data = img });
        //    }
        //    else
        //    {
        //        return BadRequest(new ResponseModel<int> { success = false, message = "No image found" });
        //    }
        //}

        //public async Task<IActionResult> GetAllNotesUsingRedisCache()
        //{
        //    var cacheKey = "NotesList";
        //    string SerializeNotesList;
        //    var NoteList = new List<NotesEntity>();
        //    var RedisNotesList = await distributedCache.GetAsync(cacheKey);
        //    if (RedisNotesList != null)
        //    {
        //        SerializeNotesList = Encoding.UTF8.GetString(RedisNotesList);
        //        NoteList = JsonConvert.DeserializeObject<List<NotesEntity>>(SerializeNotesList);

        //    }
        //    else
        //    {
        //        NoteList = FundooDBContext.NoteEntity.ToList();
        //        SerializeNotesList = JsonConvert.SerializeObject(NoteList);
        //        RedisNotesList = Encoding.UTF8.GetBytes(SerializeNotesList);
        //        var options = new DistributedCacheEntryOptions()
        //            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(20))
        //            .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        //        await DistributedCacheEntryExtensions.SetAsync(cacheKey, RedisNotesList, options);

        //    }
        //    return Ok(NoteList);
        //}



    }
}
