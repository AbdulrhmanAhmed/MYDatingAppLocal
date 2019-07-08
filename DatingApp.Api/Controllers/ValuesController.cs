using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _DataContext;
        public ValuesController(DataContext DataContext)
        {
            _DataContext = DataContext;

        }
        // GET api/values
        [HttpGet]
        public async Task< IActionResult> GetValues()
        {
            var Values=await _DataContext.Values.ToListAsync();
            return Ok(Values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetValueByID(int id)
        {
            var Values =_DataContext.Values.FirstOrDefault(x=>x.Id==id);
            return Ok(Values); 
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
