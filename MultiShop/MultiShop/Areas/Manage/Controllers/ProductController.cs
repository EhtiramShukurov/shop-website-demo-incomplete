using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiShop.DAL;
using MultiShop.Models;
using MultiShop.Utilities;
using MultiShop.ViewModels;
using NuGet.Packaging;

namespace MultiShop.Areas.Manage.Controllers
{
	[Area("Manage")]

	public class ProductController : Controller
    {
        readonly AppDbContext _context;
        IWebHostEnvironment _env { get; }
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Products.Include(p => p.ProductImages).Include(p => p.Discount).Include(p => p.ProductColors).ThenInclude(pc=>pc.Color).Include(p => p.ProductSizes).ThenInclude(ps=>ps.Size).Include(p => p.Category));
        }
        public IActionResult Create()
        {
            ViewBag.Discounts = new SelectList(_context.Discounts, nameof(Discount.Id), nameof(Discount.Price));
            ViewBag.Categories = new SelectList(_context.Categories,nameof(Category.Id),nameof(Category.Name));
            ViewBag.Informations = new SelectList(_context.productInformations,nameof(ProductInformation.Id),nameof(ProductInformation.Name));
            ViewBag.Colors = new SelectList(_context.Colors,nameof(Color.Id),nameof(Color.Name));
            ViewBag.Sizes = new SelectList(_context.Sizes,nameof(Size.Id),nameof(Size.Name));

            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateProductVM cp)
        {
            var coverImg = cp.CoverImage;
            var otherImgs = cp.OtherImages ?? new List<IFormFile>();
            string result = coverImg?.CheckValidate("image/", 600);
            if (result?.Length >0)
            {
                ModelState.AddModelError("CoverImage", result);
            }
            foreach (var img in otherImgs)
            {
                result = img?.CheckValidate("image/", 600);
                if (result?.Length > 0)
                {
                    ModelState.AddModelError("OtherImages", result);
                    break;                }
            }
            foreach (var sizeId in (cp.SizeIds ?? new List<int>()))
            {
                if (!_context.Sizes.Any(s => s.Id == sizeId))
                {
                    ModelState.AddModelError("SizeId", "There is no size in this id!");
                }
            }
            foreach (var sizeId in (cp.SizeIds ?? new List<int>()))
            {
                if (!_context.Sizes.Any(s => s.Id == sizeId))
                {
                    ModelState.AddModelError("SizeIds", "There is no size in this id!");
                }
            }
            foreach (var sizeId in (cp.ColorIds ?? new List<int>()))
            {
                if (!_context.Sizes.Any(s => s.Id == sizeId))
                {
                    ModelState.AddModelError("ColorIds", "There is no color in this id!");
                }
            }
            foreach (var sizeId in (cp.ColorIds ?? new List<int>()))
            {
                if (!_context.Sizes.Any(s => s.Id == sizeId))
                {
                    ModelState.AddModelError("ColorIds", "There is no color in this id!");
                }
            }
            if (!_context.Discounts.Any(d=>d.Id == cp.DiscountId) && cp.DiscountId !=null)
            {
                ModelState.AddModelError("DiscountId", "There is no discount in this id!");
            }
            if (!_context.Categories.Any(c => c.Id == cp.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "There is no category in this id!");
            }
            if (!_context.productInformations.Any(p => p.Id == cp.InformationId))
            {
                ModelState.AddModelError("InformationId", "There is no information in this id!");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Discounts = new SelectList(_context.Discounts, nameof(Discount.Id), nameof(Discount.Price));
                ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                ViewBag.Informations = new SelectList(_context.productInformations, nameof(ProductInformation.Id), nameof(ProductInformation.Name));
                ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                return View();
            }
            var sizes = _context.Sizes.Where(s=>cp.SizeIds.Contains(s.Id)).ToList();
            var colors = _context.Colors.Where(c=>cp.ColorIds.Contains(c.Id)).ToList();
            Product product = new Product
            {
                Name = cp.Name,
                Description = cp.Description,
                CostPrice = cp.CostPrice,
                SellPrice = cp.SellPrice,
                DiscountId = cp.DiscountId,
                CategoryId = cp.CategoryId,
                InformationId = cp.InformationId


            };
            List<ProductImage> images = new List<ProductImage>();
            images.Add(new ProductImage
            {
                ImageUrl = coverImg?.SaveFile(Path.Combine(_env.WebRootPath, "assets", "img", "product")),
                IsCover = true,
                Product = product
            });
            foreach (var item in otherImgs)
            {
                images.Add(new ProductImage
                {
                    ImageUrl = item?.SaveFile(Path.Combine(_env.WebRootPath, "assets", "img", "product")),
                    IsCover = false,
                    Product = product
                });
            }
            product.ProductImages = images;
            _context.Products.Add(product);
            foreach (var size in sizes)
            {
                _context.ProductSizes.Add(new ProductSize { Product = product, Size = size });
            }
            foreach (var color in colors)
            {
                _context.ProductColors.Add(new ProductColor { Product = product, Color = color });
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if(id is null ||id == 0) return BadRequest();
            var product = _context.Products.Include(p=>p.Category).Include(p=>p.Discount).Include(p=>p.Information).Include(p=>p.ProductColors)
                .Include(p=>p.ProductSizes).Include(p=>p.ProductImages).FirstOrDefault(p=> p.Id == id);
            if (product == null) return NotFound();
            UpdateProductVM update = new UpdateProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CostPrice = product.CostPrice,
                SellPrice = product.SellPrice,
                ProductImages = product.ProductImages.ToList(),
                ColorIds = product.ProductColors.Select(pc => pc.ColorId).ToList(),
                SizeIds = product.ProductSizes.Select(ps => ps.SizeId).ToList(),
                DiscountId= product.DiscountId,
                CategoryId= product.CategoryId,
                InformationId = product.InformationId
            };

            ViewBag.Discounts = new SelectList(_context.Discounts, nameof(Discount.Id), nameof(Discount.Price));
            ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            ViewBag.Informations = new SelectList(_context.productInformations, nameof(ProductInformation.Id), nameof(ProductInformation.Name));
            ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
            ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
            ViewBag.Images = _context.ProductImages.Where(pi => pi.ProductId == id).ToList();


            return View(update);
        }
        [HttpPost]
        public IActionResult Update(int? id, UpdateProductVM update)
        {
            if (id == null || id == 0) return BadRequest();
            foreach (var colorid in update.ColorIds)
            {
                if (!_context.Colors.Any(c => c.Id == colorid))
                {
                    ModelState.AddModelError("ColorIds", "There is no color in this id!");
                    break;
                }
            }
            foreach (var sizeId in update.SizeIds)
            {
                if (!_context.Sizes.Any(c => c.Id == sizeId))
                {
                    ModelState.AddModelError("SizeIds", "There is no size in this id");
                    break;
                }
            }
            if (!_context.Discounts.Any(d => d.Id == update.DiscountId))
            {
                ModelState.AddModelError("DiscountId", "There is no discount in this id!");
            }
            if (!_context.Categories.Any(d => d.Id == update.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "There is no category in this id!");
            }
            if (!_context.productInformations.Any(d => d.Id == update.InformationId))
            {
                ModelState.AddModelError("InformationId", "There is no information in this id!");
            }
            var coverImg = update.CoverImage;
            var otherImgs = update.OtherImages ?? new List<IFormFile>();
            string result = coverImg?.CheckValidate("image/", 600);
            if (result?.Length > 0)
            {
                ModelState.AddModelError("CoverImage", result);
            }
            foreach (var img in otherImgs)
            {
                result = img.CheckValidate("image/", 600);
                if (result?.Length > 0)
                {
                    ModelState.AddModelError("OtherImages", result);
                    break;
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Discounts = new SelectList(_context.Discounts, nameof(Discount.Id), nameof(Discount.Price));
                ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                ViewBag.Informations = new SelectList(_context.productInformations, nameof(ProductInformation.Id), nameof(ProductInformation.Name));
                ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                ViewBag.Images = _context.ProductImages.Where(pi=>pi.ProductId == id).ToList();
                return View();
            }
            var product = _context.Products.Include(p=>p.ProductSizes).Include(p=>p.ProductColors).Include(p=>p.Category).Include(p=>p.Discount)
                .Include(p=>p.ProductImages).Include(p=>p.Information).FirstOrDefault(p=>p.Id == id);
            if (product is null) return NotFound();
            foreach (var item in product.ProductSizes)
            {
                if (update.SizeIds.Contains(item.SizeId))
                {
                    update.SizeIds.Remove(item.SizeId);
                }
                else
                {
                    _context.ProductSizes.Remove(item);
                }
            }
            foreach (var sizeId in update.SizeIds)
            {
                _context.ProductSizes.Add(new ProductSize { Product = product, SizeId = sizeId });
            };
            foreach (var item in product.ProductColors)
            {
                if (update.ColorIds.Contains(item.ColorId))
                {
                    update.ColorIds.Remove(item.ColorId);
                }
                else
                {
                    _context.ProductColors.Remove(item);
                }
            }
            foreach (var colorId in update.ColorIds)
            {
                _context.ProductColors.Add(new ProductColor { Product = product, ColorId = colorId });
            }
            List<ProductImage> images = new List<ProductImage>();
            if (coverImg != null)
            {
                var oldcover = product.ProductImages.FirstOrDefault(p=>p.IsCover == true);
                _context.ProductImages.Remove(oldcover);
                oldcover.ImageUrl.DeleteFile(_env.WebRootPath, "assets/img/product");
                images.Add( new ProductImage{ImageUrl = coverImg.SaveFile(Path.Combine(_env.WebRootPath, "assets", "img", "product")),IsCover = true,Product = product});
            }
            foreach (var image in otherImgs)
            {
                images.Add(new ProductImage { ImageUrl = image?.SaveFile(Path.Combine(_env.WebRootPath, "assets", "img", "product")), IsCover = false, Product = product });

            }
            foreach (var item in product.ProductImages)
            {
                if (update.ImageIds.Contains(item.Id))
                {
                    product.ProductImages.Remove(item);
                    item.ImageUrl?.DeleteFile(_env.WebRootPath, "assets/img/product");
                }
            }
            product.Name = update.Name;
            product.Description = update.Description;
            product.InformationId = update.InformationId;
            product.DiscountId = update.DiscountId;
            product.CategoryId = update.CategoryId;
            product.CostPrice = update.CostPrice;
            product.SellPrice = update.SellPrice;
            foreach (var item in images)
            {
            product.ProductImages.Add(item);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
