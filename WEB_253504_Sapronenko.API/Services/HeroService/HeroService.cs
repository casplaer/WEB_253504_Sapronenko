﻿using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.API.Data;
using WEB_253504_Sapronenko.API.Services.CategoryService;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;

namespace WEB_253504_Sapronenko.API.Services.HeroService
{
    public class HeroService : IHeroService
    {
        private readonly int _maxPageSize = 5;
        private readonly AppDbContext _context;

        public HeroService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<DotaHero>> CreateHeroAsync(DotaHero hero)
        {
            _context.Entry(hero.Category).State = EntityState.Unchanged;

            await _context.Heroes.AddAsync(hero);
            await _context.SaveChangesAsync();
            return ResponseData<DotaHero>.Success(hero);
        }

        public Task DeleteHeroAsync(int id)
        {
            var toRemove = _context.Heroes.Where(dh => dh.Id == id).First();
            _context.Heroes.Remove(toRemove);
            return Task.FromResult(_context.SaveChanges());
        }

        public Task<ResponseData<DotaHero>> GetHeroByIdAsync(int id)
        {
            var hero = _context.Heroes.Where(a => a.Id == id).First();
            return Task.FromResult(ResponseData<DotaHero>.Success(hero));
        }

        public async Task<ResponseData<ListModel<DotaHero>>> GetHeroListAsync(string? categoryNormalizedName, int pageNo = 0, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Heroes.AsQueryable();
            var dataList = new ListModel<DotaHero>();

            if(categoryNormalizedName.ToLower() != "any")
            {
                query = query
                .Where(d => categoryNormalizedName == null
                ||
                d.Category.NormalizedName.Equals(categoryNormalizedName));
            }

            var count = await query.CountAsync();
            if (count == 0)
            {
                return ResponseData<ListModel<DotaHero>>.Success(dataList);
            }
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return ResponseData<ListModel<DotaHero>>.Error("No such page");

            if(pageNo != 0)
                dataList.Items = await query
                .OrderBy(d => d.Id)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            else dataList.Items = await query
                .OrderBy(d => d.Id)
                .ToListAsync();

            dataList.CurrentPage = pageNo == 0 ? 1 : pageNo;
            dataList.TotalPages = totalPages;

            return ResponseData<ListModel<DotaHero>>.Success(dataList);

        }

        public Task UpdateHeroAsync(int id, DotaHero hero, IFormFile? formFile)
        {
            _context.Heroes.Attach(hero).State = EntityState.Modified;
            return Task.FromResult(_context.SaveChanges());
        }
    }
}
