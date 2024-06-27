using DAPManSWebReports.Domain.CommonService.CommonInterface;
using DAPManSWebReports.Domain.FolderService;
using DAPManSWebReports.Domain.MappingService;
using DAPManSWebReports.Entities.Models;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAPManSWebReports.API.Controllers
{
    [Route("api/folders")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private IMappingService<FolderModel> _baseRepo;
        private IMenuTreeService<FolderModel> _folderDtoService;

        public FolderController(IMappingService<FolderModel> baseRepo, IMenuTreeService<FolderModel> folderDtoService)
        {
            _baseRepo = baseRepo;
            _folderDtoService = folderDtoService;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<FolderModel>> GetFolders()
        {
            var folders = await _baseRepo.GetDtoList();
            return folders;
            
        }

        [HttpGet("parents")]
        public async Task<IEnumerable<FolderModel>> GetParentFolders()
        {
            var folders = await _folderDtoService.GetParentDtos();
            return folders;

        }

        [HttpGet("childrens/{parentid}")]
        public async Task<IActionResult> GetChildFoldersById(int parentid)
        {
            IEnumerable<FolderModel> foldersById = _folderDtoService.GetChildDtos(parentid);
            if (foldersById != null)
                return Ok(foldersById);
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFolderById(int id)
        {
            FolderModel folderById = await _folderDtoService.GetDtoById(id);
            if (folderById != null)
                return Ok(folderById);
            else
            {
                return BadRequest();
            }
        }

        // POST api/<FolderController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FolderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FolderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
