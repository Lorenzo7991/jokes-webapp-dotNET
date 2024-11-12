using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JokesWebApp.Data;
using JokesWebApp.Models;

namespace JokesWebApp.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jokes       -- INDEX
        public async Task<IActionResult> Index()
        {
            // Ritorna una vista (Index) con il metodo "await _context.Joke.ToListAsync()" che ottiene tutte le barzellete in modo asincrono.
            return View(await _context.Joke.ToListAsync());
        }


        // GET: Jokes/Details/5     -- EDIT
        // Edit riceve un id(parametro opzionale)
        public async Task<IActionResult> Details(int? id)
        {
            // Condizione: Se l'id è nullo...
            if (id == null)
            {
                // Ritorna "NotFound";
                return NotFound();
            }

            // Cerca joke con id specificato.
            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.id == id);
            // Condizione: Se non lo trova...
            if (joke == null)
            {
                // Ritorna null.
                return NotFound();
            }

            // Se trova l'id lo passa alla vista, altrimenti restituisce "NotFound".
            return View(joke);
        }

        // GET: Jokes/Create        -- CREATE

        // In GET cerca joke con id fornito e passa tutto alla vista
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // In POST riceve un oggetto Joke...
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,JokeQuestion,JokeAnswer")] Joke joke)
        {
            // Condizione: Se i dati sono validi...
            if (ModelState.IsValid)
            {
                // Aggiunge la barzelletta e salva...
                _context.Add(joke);
                await _context.SaveChangesAsync();
                // Dopo avere creato l'oggetto, reinderizza alla vista.
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }

        // GET: Jokes/Edit/5    -- UPDATE

        // In GET cerca joke con l'id fornito, se la trova, la passa alla vista per le modifiche...
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }
            return View(joke);
        }

        // POST: Jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // In POST...
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,JokeQuestion,JokeAnswer")] Joke joke)
        {
            if (id != joke.id)
            {
                return NotFound();
            }

            // Verifica che l'id della joke sia valido.
            if (ModelState.IsValid)
            {
                // Prova ad aggiornare la joke...
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                }
                // Se intercetta errore di concorrenza, verifica l'esistenza della joke.
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeExists(joke.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Altrimenti reinderizza alla index.
                return RedirectToAction(nameof(Index));
            }
            // Restituisce la joke alla vista.
            return View(joke);
        }

        // GET: Jokes/Delete/5      -- DELETE

        // Verifica se esiste la joke con l'id corrispondente...
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.id == id);
            if (joke == null)
            {
                return NotFound();
            }
            // La passa alla vista per confermare la cancellazione...
            return View(joke);
        }

        // POST: Jokes/Delete/5

        // In POST cerca la joke tramite id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var joke = await _context.Joke.FindAsync(id);
            if (joke != null)
            {
                // Se la trova la rimuove...
                _context.Joke.Remove(joke);
            }

            await _context.SaveChangesAsync();
            // Reinderizza alla vista index.
            return RedirectToAction(nameof(Index));
        }

        private bool JokeExists(int id)
        {
            return _context.Joke.Any(e => e.id == id);
        }
    }
}
