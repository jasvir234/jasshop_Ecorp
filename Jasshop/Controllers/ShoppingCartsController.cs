using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jasshop.Data;
using Jasshop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Jasshop.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _session;

        public ShoppingCartsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor session)
        {
            _userManager = userManager;
            _context = context;
            _session = session;
        }

        // GET: ShoppingCarts
        
        public async Task<IActionResult> Index()
        {
            var cartId = _session.HttpContext.Session.GetString("cartId");

            var carts = _context.ShoppingCart
                .Where(c => c.CartId == cartId);
            
            return View(await _context.ShoppingCart.ToListAsync());
        }

        // GET: ShoppingCarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemName,Description,Price,Quantity,Seller,Buyer")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCart.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemName,Description,Price,Quantity,Seller,Buyer")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.Id))
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
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingCart = await _context.ShoppingCart.FindAsync(id);
            _context.ShoppingCart.Remove(shoppingCart);
            await _context.SaveChangesAsync();

            var checkCount = _session.HttpContext.Session.GetInt32("cartCount");
            int cartCount = checkCount == null ? 0 : (int)checkCount;
            _session.HttpContext.Session.SetInt32("cartCount", --cartCount);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Purchase()
        {
            // get the cart id
            var cartId = _session.HttpContext.Session.GetString("cartId");

            // get the cart items
            var carts = _context.ShoppingCart
                .Where(c => c.CartId == cartId);

            // get the buyer
            var buyer = _userManager.GetUserName(User);

            // create the sales
            foreach (ShoppingCart cart in carts.ToList())
            {
                // find the item
                var item = await _context.Items
                    .FirstOrDefaultAsync(m => m.ItemName == cart.ItemName);

                // update the quantity
                item.Quantity -= cart.Quantity;
                _context.Update(item);

                Sales sale = new Sales { Buyer = buyer, ItemName = cart.ItemName, Quantity = cart.Quantity, Price=cart.Price, Seller = cart.Seller, Description=cart.Description };
                _context.Update(sale);
                _context.Remove(cart);
            }

            // Save the changes
            await _context.SaveChangesAsync();

            // delete cart
            _session.HttpContext.Session.SetString("cartId", "");
            _session.HttpContext.Session.SetInt32("cartCount", 0);

            return RedirectToAction(nameof(Index), "Items");
        }


        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCart.Any(e => e.Id == id);
        }
    }
}
