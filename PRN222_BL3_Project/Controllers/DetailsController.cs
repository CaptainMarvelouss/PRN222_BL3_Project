using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using Repositories;

public class DetailsController : Controller
{
    private readonly IFootballFieldRepository _fieldRepo;

    public DetailsController(IFootballFieldRepository fieldRepo)
    {
        _fieldRepo = fieldRepo;
    }

    public async Task<IActionResult> Details(int id)
    {
        var field = _fieldRepo.GetFootballFieldById(id);
        if (field == null)
        {
            return NotFound();
        }

        var allFields = _fieldRepo.GetFootballFields();

        var viewModel = new FieldDetailsViewModel
        {
            Field = field,
            AllFields = allFields
        };

        return View(viewModel);
    }

}
