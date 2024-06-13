using DAPManSWebReports.Domain.Entities;
using DAPManSWebReports.Domain.Interfaces;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAPManSWebReports.API.Controllers
{
    [Route("api/dataview")]
    [ApiController]
    public class DataViewController : ControllerBase
    {
        private IMappingService<DataViewModel> _dataViewRepository;
        private IMenuTreeService<DataViewModel> _dataviewDtoService;
        private IDataViewService<DataViewModel> _dataViewService;
        public DataViewController(IMappingService<DataViewModel> dataViewRepository, IMenuTreeService<DataViewModel> dataviewDtoService, IDataViewService<DataViewModel> dataViewService)
        {
            _dataViewRepository = dataViewRepository;
            _dataviewDtoService = dataviewDtoService;
            _dataViewService = dataViewService;
        }

        [HttpGet]
        public async Task<IEnumerable<DataViewModel>> Get()
        {
            var dataViews = await _dataViewRepository.GetDtoList();
            return dataViews;
        }

        [HttpGet("parents")]
        public async Task<IEnumerable<DataViewModel>> GetParentFolders()
        {
            var dataviews = await _dataviewDtoService.GetParentDtos();
            return dataviews;

        }

        [HttpGet("childrens/{parentid}")]
        public IActionResult GetChildFoldersById(int parentid)
        {
            IEnumerable<DataViewModel> viewsById =  _dataviewDtoService.GetChildDtos(parentid);
            if (viewsById.Count() != 0)
                return Ok(viewsById);
            else
            {
                return BadRequest();
            }
        }

        // GET api/<DataViewController>/5
        [HttpGet("{dataviewid}")]
        public async Task<IActionResult> GetDataViewById(int dataviewid)
        {
            DataViewModel dataView = await _dataviewDtoService.GetDtoById(dataviewid);
            if(dataView != null)
                return Ok(dataView);
            else
            {
                return NotFound(); 
            }
        }

        // POST api/<DataViewController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DataViewController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DataViewController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
