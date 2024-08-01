using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using Library.ViewModel;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        public IActionResult AddBookToShelf(int Id)
        {
            var Genre = _context.Book.FirstOrDefault(x => x.Id == Id).Genre;
            var GenreId = _context.Genre.FirstOrDefault(x => x.Name == Genre).Id;
            ViewData["Id"] = Id;
            ViewData["ShelvesId"] = new SelectList(_context.Shelf.Where(s => s.GenreId == GenreId), "Id", "Id");
            if (_context.Shelf.Where(s => s.GenreId == GenreId).ToList().Count == 0)
            {
                TempData["ErrorMessage"] = "אין מדפים זמינים";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBookToShelf(BookShelf bookShelf)
        {
            
            Book book = await _context.Book.FirstOrDefaultAsync(b => b.Id == bookShelf.BookId);
            Shelf shelf = await _context.Shelf.Include(s => s.Books).FirstOrDefaultAsync(s => s.Id == bookShelf.ShelfId);
            int freeWidth = shelf.Width - shelf.Books.Sum(b => b.Width);
            if (book.SetId == null)
            {
                if (freeWidth >= book.Width && shelf.Height > book.Height) 
                {
                    if (shelf.Height >= (book.Height + 10))
                    {
                        TempData["ErrorMessage"] = " הספר נוסף בהצלחה אבל המדף גבוה מדי לספר";
                        
                    }
                    book.ShelfId = bookShelf.ShelfId;
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    

                    
                }
                else
                {
                    TempData["ErrorMessage"] = "אין מקום מספיק במדף";
                }
            }
            else
            {
                int setWidth = shelf.Books.Where(b => b.SetId == book.SetId).Sum(b => b.Width);
                Set set = _context.Set.Include(s => s.Books).FirstOrDefault(s => s.Id == book.SetId);
                if (freeWidth >= setWidth && shelf.Height > set.Height)
                {
                    if (shelf.Height >= (set.Height + 10))
                    {
                        TempData["ErrorMessage"] = " הספר נוסף בהצלחה אבל המדף גבוה מדי לספר";

                    }

                    foreach (Book book1 in _context.Book.Where(b => b.SetId == book.SetId))
                    {
                         book1.ShelfId = bookShelf.ShelfId;
                         _context.Update(book1);
                         await _context.SaveChangesAsync();
                    }
                    
                }
                else 
                {
                    TempData["ErrorMessage"] = "אין מקום מספיק במדף";
                }
            }
            var Genre = _context.Book.FirstOrDefault(x => x.Id == book.Id).Genre;
            var GenreId = _context.Genre.FirstOrDefault(x => x.Name == Genre).Id;
            ViewData["Id"] = book.Id;
            ViewData["ShelvesId"] = new SelectList(_context.Shelf.Where(s => s.GenreId == GenreId), "Id", "Id");
            if (_context.Shelf.Where(s => s.GenreId == GenreId).ToList().Count == 0)
            {
                TempData["ErrorMessage"] = "אין מדפים זמינים";
            }
            return View();

        }

        




        // GET: Books
        public async Task<IActionResult> Index()
        {
            var libraryContext = _context.Book.Include(b => b.Set).Include(b => b.Shelf);
            return View(await libraryContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Set)
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult CreateSet()
        {
            return RedirectToAction("Create", "Sets");
        }
        public IActionResult AddToSet(int? Id, string? Genre)
        {
            ViewData["Genres"] = new SelectList(_context.Genre.Where(g => g.Name == Genre), "Name", "Name");
            ViewData["SetName"] = new SelectList(_context.Set<Set>().Where(s => s.Id == Id), "Id", "NameSet"); 
            return View();
        }
        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["Genres"] = new SelectList(_context.Genre, "Name", "Name");
            ViewData["SetId"] = new SelectList(_context.Set<Set>(), "Id", "Id");
            ViewData["ShelfId"] = new SelectList(_context.Set<Shelf>(), "Id", "Id");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Genre,Height,Width,ShelfId,SetId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SetId"] = new SelectList(_context.Set<Set>(), "Id", "Id", book.SetId);
            ViewData["ShelfId"] = new SelectList(_context.Set<Shelf>(), "Id", "Id", book.ShelfId);
            return View(book);
        }
        
        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["SetId"] = new SelectList(_context.Set<Set>(), "Id", "Id", book.SetId);
            ViewData["ShelfId"] = new SelectList(_context.Set<Shelf>(), "Id", "Id", book.ShelfId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Genre,Height,Width,ShelfId,SetId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["SetId"] = new SelectList(_context.Set<Set>(), "Id", "Id", book.SetId);
            ViewData["ShelfId"] = new SelectList(_context.Set<Shelf>(), "Id", "Id", book.ShelfId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Set)
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            
            if (book != null)
            {
                _context.Book.Remove(book);
                _context.SaveChanges();
                if (book.SetId != null)
                {
                    Set set = _context.Set.FirstOrDefault(s => s.Id == book.SetId);
                    var bookCount = _context.Book.Count(b => b.SetId == book.SetId);
                    if (bookCount == 0)
                    {
                        _context.Remove(set);
                        _context.SaveChanges();
                    }
                }
            }

            
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
