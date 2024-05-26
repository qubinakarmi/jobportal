using JobPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        JobPortalDbContext db_context = new JobPortalDbContext();

        // GET: JobController
        public ActionResult Index(int CurrentPage = 1, int PageSize = 10)
        {
            int userId = Convert.ToInt32(HttpContext.User.Claims.ElementAt(1).Value);
            var job_list = db_context.Jobs.Where(j => j.UserId == userId )
                .Select(x => new JobViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                Title = x.Title,
                OrganizationId = x.OrganizationId, 
                OrganizationName = x.Organization.Name,
                CreatedBy = x.User.UserName
            });
            
            if(job_list != null)
            {
                var paged_list = job_list.OrderBy(x => x.Id)
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize).ToList();
                var new_list = new PaginatedJobViewModel()
                {
                    Count = job_list.Count(),
                    CurrentPage = CurrentPage,
                    PageSize = PageSize,
                    Data = paged_list
                };
                return View(new_list);
            }
            else
            {
                return View(Enumerable.Empty<PaginatedJobViewModel>());
            }
        }

        // GET: JobController/Details/5
        public ActionResult Details(int id)
        {
            var job_detail = db_context.Jobs.Include(x => 
            x.Organization).Where(x => x.Id == id).FirstOrDefault();
            var mapped_job = new JobViewModel()
            {
                Id = job_detail.Id, 
                Title = job_detail.Title,
                Description = job_detail.Description,
                OrganizationName = job_detail.Organization.Name,
            };
            return View(mapped_job);
        }

        // GET: JobController/Create
        public ActionResult Create()
        {
            var org_list = db_context.Organizations.Select( x => new OrganizationViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            ViewBag.Organizations = org_list;
            return View();
        }

        // POST: JobController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobViewModel jvm)
        {
            
            try
            {
                var entity = new Job() 
                {
                    Title = jvm.Title,
                    Description = jvm.Description,
                    UserId = Convert.ToInt32(HttpContext.User.Claims.ElementAt(1).Value),
                    OrganizationId = jvm.OrganizationId,
                };
                db_context.Jobs.Add(entity);
                db_context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: JobController/Edit/5
        public ActionResult Edit(int id)
        {
            var org_list = db_context.Organizations.Select(x => new OrganizationViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            ViewBag.Organizations = org_list;

            var job_detail = db_context.Jobs.Include(x =>
            x.Organization).Where(x => x.Id == id).FirstOrDefault();
            var mapped_job = new JobViewModel()
            {
                Id = job_detail.Id,
                Title = job_detail.Title,
                Description = job_detail.Description,
                OrganizationName = job_detail.Organization.Name,
            };
            return View(mapped_job);
        }

        // POST: JobController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,JobViewModel jvm)
        {
            try
            {
                var entity = new Job()
                {
                    Id = jvm.Id,
                    Title = jvm.Title,
                    Description = jvm.Description,
                    OrganizationId = jvm.OrganizationId,
                };
                db_context.Jobs.Update(entity);
                db_context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: JobController/Delete/5
        public ActionResult Delete(int id)
        {
            var job_detail = db_context.Jobs.Include(x =>
            x.Organization).Where(x => x.Id == id).FirstOrDefault();
            var delete_job = new JobViewModel()
            {
                Id = job_detail.Id,
                Title = job_detail.Title,
                Description = job_detail.Description,
                OrganizationName = job_detail.Organization.Name,
            };
            return View(delete_job);
        }

        // POST: JobController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,IFormCollection collection)
        {
            try
            {
                var entity = db_context.Jobs.Where(x => x.Id == id).FirstOrDefault();
                db_context.Jobs.Remove(entity);
                db_context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
