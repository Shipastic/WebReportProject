using DAPManSWebReports.API.Services.QueryParamService;
using DAPManSWebReports.Domain.CommonService.CommonInterface;
using DAPManSWebReports.Domain.DataViewService;
using DAPManSWebReports.Domain.FolderService;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAPManSWebReports.API.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class FolderDetailController : ControllerBase
    {
        private IMenuTreeService<FolderModel> _folderDtoService;
        private IMenuTreeService<DataViewModel> _dataViewDtoService;
        private IMenuService<FolderDetail, FolderModel, DataViewModel> _menuService;
        private IDataViewService<DataViewModel> _dataViewService;
        public FolderDetailController(IMenuTreeService<FolderModel> folderDtoService,
                                  IMenuTreeService<DataViewModel> dataViewDtoService,
                                  IMenuService<FolderDetail, FolderModel, DataViewModel> menuService,
                                  IDataViewService<DataViewModel> dataViewService)
        {
            _folderDtoService = folderDtoService;
            _dataViewDtoService = dataViewDtoService;
            _menuService = menuService;
            _dataViewService = dataViewService;
        }

        [HttpGet("parents")]
        public async Task<FolderDetail> GetParentMenuItens()
        {
            IEnumerable<FolderModel> folderList   = await _folderDtoService.GetParentDtos();

            //List<int> folderIds                   = folderList.Select(folder => folder.id).ToList();
            IEnumerable<DataViewModel> viewList   = await _dataViewDtoService.GetParentDtos();

            FolderDetail items                    = await _menuService.GetListDtos(folderList, viewList);
            return items;
        }
        [HttpGet("childrens/{parentid}")]
        public async Task<IActionResult> GetChildFoldersViewsById(int parentid)
        {
            IEnumerable<FolderModel> foldersById     =  _folderDtoService.GetChildDtos(parentid);
            IEnumerable<DataViewModel> dataViewsById =  _dataViewDtoService.GetChildDtos(parentid);
            FolderDetail menuEntities = await _menuService.GetListDtos(foldersById, dataViewsById);
            if (menuEntities != null)
                return Ok(menuEntities);
            else
            {
                return BadRequest();
            }
        }
        // GET: api/<FolderDetailController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FolderDetailController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FolderDetailController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FolderDetailController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FolderDetailController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
