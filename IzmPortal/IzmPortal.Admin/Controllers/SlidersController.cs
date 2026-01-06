using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.Slider;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

public class SlidersController : BaseAdminController
{
    public SlidersController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    // =======================
    // LIST
    // =======================
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var response = await Api.GetAsync("/api/sliders", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Sliderlar alınamadı.");

        if (failure != null)
            return failure;

        var items = await response
            .ReadContentAsync<List<SliderDto>>() ?? new();

        return View(items);
    }

    // =======================
    // UPLOAD (GET)
    // =======================
    public IActionResult Upload()
    {
        return View();
    }

    // =======================
    // UPLOAD (POST)
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(
        CreateSliderDto dto,
        IFormFile file,
        CancellationToken ct)
    {
        if (!ModelState.IsValid || file == null || file.Length == 0)
        {
            ModelState.AddModelError(
                nameof(file),
                "Resim seçilmedi.");

            return View(dto);
        }

        using var content = new MultipartFormDataContent();

        content.Add(
            new StringContent(dto.Title ?? string.Empty),
            nameof(CreateSliderDto.Title));

        content.Add(
            new StreamContent(file.OpenReadStream()),
            nameof(file),
            file.FileName);

        var response = await Api.PostAsync(
            "/api/sliders",
            content,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Slider eklenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Slider başarıyla eklendi.");
    }

    // =======================
    // DELETE
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var response = await Api.DeleteAsync(
            $"/api/sliders/{id}",
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Slider silinemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Slider silindi.");
    }
}
