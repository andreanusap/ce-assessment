using CE.Assessment.BusinessLogic.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace CE.Assessment.BusinessLogic.Services
{
    public interface IProductService
    {
        public Task<PatchResponse> UpdateProduct(string merchantProductNo, JsonPatchDocument patchDoc);
    }
}
