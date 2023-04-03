using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Univ_Manage.Infrastructure.Models.Security;
using Univ_Manage.Infrastructure.SqlServer;

namespace Univ_Manage.Controllers
{
    public class UserSetsController : Controller
    {
        private readonly Univ_ManageDBContext _context;

        public UserSetsController(Univ_ManageDBContext context)
        {
            _context = context;
        }

        // GET: UserSets
        public async Task<IActionResult> Index()
        {
              return _context.Users != null ? 
                          View(await _context.Users.ToListAsync()) :
                          Problem("Entity set 'Univ_ManageDBContext.Users'  is null.");
        }

        // GET: UserSets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userSet = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userSet == null)
            {
                return NotFound();
            }

            return View(userSet);
        }

        // GET: UserSets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserSets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,IsDeleted,DepartmentId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] UserSet userSet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userSet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userSet);
        }

        // GET: UserSets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userSet = await _context.Users.FindAsync(id);
            if (userSet == null)
            {
                return NotFound();
            }
            return View(userSet);
        }

        // POST: UserSets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,IsDeleted,DepartmentId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] UserSet userSet)
        {
            if (id != userSet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserSetExists(userSet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userSet);
        }

        // GET: UserSets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userSet = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userSet == null)
            {
                return NotFound();
            }

            return View(userSet);
        }

        // POST: UserSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'Univ_ManageDBContext.Users'  is null.");
            }
            var userSet = await _context.Users.FindAsync(id);
            if (userSet != null)
            {
                _context.Users.Remove(userSet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserSetExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
