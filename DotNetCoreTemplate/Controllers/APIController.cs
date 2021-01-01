using DotNetCoreTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetCoreTemplate.Controllers
{
    [Route("api/[controller]s")]
    public class APIController : Controller
    {
        private static List<APIModel> _apiModel = new List<APIModel>
                                                  {
                                                      new APIModel
                                                      {
                                                          Id = 1,
                                                          Name = "test01"
                                                      },
                                                      new APIModel
                                                      {
                                                          Id = 2,
                                                          Name = "test02"
                                                      }
                                                  };

        [HttpGet]
        public APIResultModel Get(string q)
        {
            var result = new APIResultModel();
            result.Data = _apiModel.Where(c => string.IsNullOrEmpty(q)
                                         || Regex.IsMatch(c.Name, q, RegexOptions.IgnoreCase));
            result.IsSuccess = true;
            return result;
        }

        [HttpGet("{id}")]
        public APIResultModel Get(int id)
        {
            var result = new APIResultModel();
            result.Data = _apiModel.SingleOrDefault(c => c.Id == id);
            result.IsSuccess = true;
            return result;
        }

        [HttpPost]
        public APIResultModel Post([FromBody] APIModel apiModel)
        {
            var result = new APIResultModel();
            apiModel.Id = _apiModel.Count() == 0 ? 1 : _apiModel.Max(c => c.Id) + 1;
            _apiModel.Add(apiModel);
            result.Data = apiModel.Id;
            result.IsSuccess = true;
            return result;
        }

        [HttpPut("{id}")]
        public APIResultModel Put(int id, [FromBody] APIModel apiModel)
        {
            var result = new APIResultModel();
            int index;
            if ((index = _apiModel.FindIndex(c => c.Id == id)) != -1)
            {
                _apiModel[index] = apiModel;
                result.IsSuccess = true;
            }
            return result;
        }

        [HttpDelete("{id}")]
        public APIResultModel Delete(int id)
        {
            var result = new APIResultModel();
            int index;
            if ((index = _apiModel.FindIndex(c => c.Id == id)) != -1)
            {
                _apiModel.RemoveAt(index);
                result.IsSuccess = true;
            }
            return result;
        }
    }
}
