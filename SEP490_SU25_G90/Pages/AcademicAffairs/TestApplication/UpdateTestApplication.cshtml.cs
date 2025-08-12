using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using SEP490_SU25_G90.vn.edu.fpt.Services.LicenseTypeService;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestScoreStandardService;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.TestApplication
{
    [Authorize(Roles = "academic affairs, instructor")]
    public class UpdateTestApplication : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public List<SelectListItem> LearningApplicationList { get; set; }
        public List<(int id, string type)> LicenseTypes { get; set; } = new();
        [BindProperty]
        public CreatUpdateTestApplicationRequest RequestModel { get; set; } = new();
        private readonly ILicenseTypeService licenseTypeService;

        private readonly ITestApplicationService _testApplicationService;
        private readonly ITestScoreStandardService testScoreStandardService;
        public UpdateTestApplication(ITestApplicationService testApplicationService,
            ITestScoreStandardService testScoreStandardService,
            ILicenseTypeService licenseTypeService
            )
        {
            _testApplicationService = testApplicationService;
            this.testScoreStandardService = testScoreStandardService;

        }
        public async Task<IActionResult> OnGet()
        {
            RequestModel = await _testApplicationService.FindById(Id);
            LearningApplicationList = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Selected = true,
                    Text = $"{RequestModel.FullName} - {RequestModel.DateOfBirth} - Hạng {RequestModel.LicenseType}",
                    Value = Id.ToString()
                }
            };
            LicenseTypes = licenseTypeService.GetKeyValues();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var rawTest = await _testApplicationService.FindById(Id);
            var testScoreStandards = testScoreStandardService.FindByLearningApplication(RequestModel.LearningApplicationId!.Value);
            bool haveError = false;
            bool? status = null;
            if (RequestModel.TheoryScore != null
                || RequestModel.ObstacleScore != null
                || RequestModel.PracticalScore != null
                || RequestModel.SimulationScore != null
                )
            {
                if (!RequestModel.TheoryScore.HasValue)
                {
                    ModelState.AddModelError($"" +
                        $"{nameof(RequestModel)}.{nameof(RequestModel.TheoryScore)}",
                         $"Vui lòng nhập điểm"
                        );
                    haveError = true;
                }
                else if (!RequestModel.ObstacleScore.HasValue)
                {
                    ModelState.AddModelError($"" +
                        $"{nameof(RequestModel)}.{nameof(RequestModel.ObstacleScore)}",
                        $"Vui lòng nhập điểm"
                        );
                    haveError = true;
                }
                else if (!RequestModel.PracticalScore.HasValue)
                {
                    ModelState.AddModelError($"" +
                        $"{nameof(RequestModel)}.{nameof(RequestModel.PracticalScore)}",
                         $"Vui lòng nhập điểm"
                        );
                    haveError = true;
                }
                else if (!RequestModel.SimulationScore.HasValue)
                {
                    ModelState.AddModelError($"" +
                        $"{nameof(RequestModel)}.{nameof(RequestModel.SimulationScore)}",
                        $"Vui lòng nhập điểm"
                        );
                    haveError = true;
                }
                else
                {
                    status = false;
                    bool pass1 = false, pass2 = false, pass3 = false, pass4 = false;
                    foreach (var testScoreStandard in testScoreStandards)
                    {
                        if (testScoreStandard.PartName == "Theory")
                        {
                            if (RequestModel.TheoryScore.Value > testScoreStandard.MaxScore)
                            {

                                ModelState.AddModelError($"" +
                                    $"{nameof(RequestModel)}.{nameof(RequestModel.TheoryScore)}",
                                    $"Điểm lý thuyết không được phép vượt quá {testScoreStandard.MaxScore}"
                                    );
                                haveError = true;
                            }
                            else if (RequestModel.TheoryScore.Value < 0)
                            {
                                ModelState.AddModelError($"" +
                               $"{nameof(RequestModel)}.{nameof(RequestModel.TheoryScore)}",
                               $"Điểm lý thuyết không được phép nhỏ hơn 0"
                               );
                                haveError = true;
                            }
                            else
                            {
                                pass1 = RequestModel.TheoryScore.Value >= testScoreStandard.PassScore;
                            }
                        }
                        else if (testScoreStandard.PartName == "Simulation")
                        {
                            if (RequestModel.SimulationScore.Value > testScoreStandard.MaxScore)
                            {
                                ModelState.AddModelError($"" +
                                $"{nameof(RequestModel)}.{nameof(RequestModel.SimulationScore)}",
                                $"Điểm mô phỏng không được phép vượt quá {testScoreStandard.MaxScore}"
                                );
                                haveError = true;
                            }
                            else if (RequestModel.SimulationScore.Value < 0)
                            {
                                ModelState.AddModelError($"" +
                               $"{nameof(RequestModel)}.{nameof(RequestModel.SimulationScore)}",
                               $"Điểm mô phỏng không được phép nhỏ hơn 0"
                               );
                                haveError = true;
                            }
                            else
                            {
                                pass2 = RequestModel.SimulationScore.Value >= testScoreStandard.PassScore;
                            }

                        }
                        else if (testScoreStandard.PartName == "Obstacle")
                        {
                            if (RequestModel.ObstacleScore.Value > testScoreStandard.MaxScore)
                            {
                                ModelState.AddModelError($"" +
                                    $"{nameof(RequestModel)}.{nameof(RequestModel.ObstacleScore)}",
                                    $"Điểm sa hình không được phép vượt quá {testScoreStandard.MaxScore}"
                                );
                                haveError = true;
                            }
                            else if (RequestModel.ObstacleScore.Value < 0)
                            {
                                ModelState.AddModelError($"" +
                               $"{nameof(RequestModel)}.{nameof(RequestModel.ObstacleScore)}",
                               $"Điểm sa hình không được phép nhỏ hơn 0"
                               );
                                haveError = true;
                            }
                            else
                            {
                                pass3 = RequestModel.ObstacleScore.Value >= testScoreStandard.PassScore;
                            }

                        }
                        else if (testScoreStandard.PartName == "Practical")
                        {
                            if (RequestModel.PracticalScore.Value > testScoreStandard.MaxScore)
                            {
                                ModelState.AddModelError($"" +
                               $"{nameof(RequestModel)}.{nameof(RequestModel.PracticalScore)}",
                               $"Điểm đường trường không được phép vượt quá {testScoreStandard.MaxScore}"
                               );
                                haveError = true;
                            }
                            else if (RequestModel.PracticalScore.Value < 0)
                            {
                                ModelState.AddModelError($"" +
                               $"{nameof(RequestModel)}.{nameof(RequestModel.PracticalScore)}",
                               $"Điểm đường trường không được phép nhỏ hơn 0"
                               );
                                haveError = true;
                            }
                            else
                            {
                                pass4 = RequestModel.PracticalScore.Value >= testScoreStandard.PassScore;
                            }

                        }
                    }
                    status = pass1 && pass2 && pass3 && pass4;
                }
                if (rawTest.ResultImageUrl == null)
                {
                    ModelState.AddModelError($"" +
                       $"{nameof(RequestModel)}.{nameof(RequestModel.Attachment)}",
                       $"Vui lòng tải lên tệp đính kèm"
                       );
                    haveError = true;
                }
            }
            if (rawTest.SubmitProfileDate != RequestModel.SubmitProfileDate && RequestModel.SubmitProfileDate > DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError($"" +
                       $"{nameof(RequestModel)}.{nameof(RequestModel.ExamDate)}",
                       $"Ngày nộp hồ sơ không được vượt quá ngày hôm nay"
                       );
                haveError = true;
            }

            if ((rawTest.ExamDate != RequestModel.ExamDate || rawTest.SubmitProfileDate != RequestModel.SubmitProfileDate)
                && RequestModel.ExamDate < RequestModel.SubmitProfileDate)
            {
                ModelState.AddModelError($"" +
                       $"{nameof(RequestModel)}.{nameof(RequestModel.ExamDate)}",
                       $"Ngày thi phải lớn hơn ngày nộp hồ sơ"
                       );
                haveError = true;
            }

            if (haveError) return await OnGet();

            await _testApplicationService.UpdateTestApplication(Id, RequestModel, status);
            RequestModel = new();
            return RedirectToPage("TestApplicationList");
        }
    }
}
